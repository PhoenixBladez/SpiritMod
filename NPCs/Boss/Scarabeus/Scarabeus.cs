using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Sets.ScarabeusDrops;
using SpiritMod.Items.Sets.ScarabeusDrops.Khopesh;
using SpiritMod.Items.Sets.ScarabeusDrops.LocustCrook;
using SpiritMod.Items.Sets.ScarabeusDrops.ScarabExpertDrop;
using SpiritMod.Items.Sets.ScarabeusDrops.AdornedBow;
using SpiritMod.Items.Sets.ScarabeusDrops.RadiantCane;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	[AutoloadBossHead]
	public class Scarabeus : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarabeus");
			Main.npcFrameCount[NPC.type] = 22;
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 64;
			NPC.height = 64;
			NPC.value = 30000;
			NPC.damage = 40;
			NPC.defense = 10;
			NPC.lifeMax = 1750;
			NPC.aiStyle = -1;
			Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Scarabeus");
			NPC.boss = true;
			//npc.stepSpeed /= 20;
			NPC.npcSlots = 15f;
			NPC.HitSound = SoundID.NPCHit31;
			NPC.DeathSound = SoundID.NPCDeath5;

			NPC.buffImmune[BuffID.Confused] = true;

			bossBag = ModContent.ItemType<BagOScarabs>();
		}

		bool trailbehind;
		int frame = 0;
		float extraYoff;
		float skiptimer = 0;
		int timer = 0;
		bool canhitplayer;
		public float AiTimer {
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		public float AttackType {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		public override bool CheckActive()
		{
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
				return false;

			return true;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			canhitplayer = false; //default to being unable to hit the player at the start of each tick, overrided depending on attack pattern

			if (Main.netMode != NetmodeID.Server) {
				if (frame >= 18 && frame < 21)
					SpiritMod.scarabWings.SetTo(Main.ambientVolume * MathHelper.Clamp((800 - NPC.Distance(Main.LocalPlayer.Center)) / 400f, 0, 1));
				else
					SpiritMod.scarabWings.Stop();
			}
			

			if (player.dead || !player.active) {
				NPC.timeLeft = 10;
				Digging(player);
				AiTimer = 0;
				NPC.velocity.Y = 10;
				return;
			}
			
			if (NPC.life >= (NPC.lifeMax/2)) {
				{
					Phase1(player);
				}
			}
			else {
				if(NPC.ai[3] == 0) {
					NextAttack();
					AttackType = 0;
					NPC.ai[3]++;
				}

                Phase2(player);
                NPC.defense = 6;
            }
		}
		#region utilities
		private void CheckPlatform(Player player)
		{
			bool onplatform = true;
			for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4) { //check tiles beneath the boss to see if they are all platforms
				Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.TileType])
					onplatform = false;
			}
			if (onplatform && (NPC.Center.Y < player.position.Y - 20)) //if they are and the player is lower than the boss, temporarily let the boss ignore tiles to go through them
				NPC.noTileCollide = true;
			else
				NPC.noTileCollide = false;
		}

		private void CheckPit(float velmult = 1.7f, bool boostsxvel = true) //quirky lazy bad code but it works mostly and making the boss not break on vanilla random worldgen is tiring
		{
			if (NPC.velocity.Y != 0)
				return;

			bool pit = true;
			int pitwidth = 0;
			int width = 5;
			int height = 8;
			for (int j = 1; j <= width; j++) {
				for (int i = 1; i <= height; i++) {
					Tile forwardtile = Framing.GetTileSafely(new Point((int)(NPC.Center.X / 16) + (NPC.spriteDirection * j), (int)(NPC.Center.Y / 16) + i));
					if (WorldGen.SolidTile(forwardtile) || WorldGen.SolidTile2(forwardtile) || WorldGen.SolidTile3(forwardtile)) {
						pit = false;
						break;
					}
				}
				if (!pit)
					break;

				pitwidth++;
			}
			if (pit && pitwidth <= width * 2) { 
				NPC.velocity.Y -= pitwidth * velmult;
				if (boostsxvel)
					NPC.velocity.X = NPC.spriteDirection * pitwidth * velmult;
			}
			else if (pit)
				NPC.velocity.X *= -1f;
		}

		private void UpdateFrame(int speed, int minframe, int maxframe, bool usesspeed = false) //method of updating the frame without copy pasting this every time animation is needed
		{
			timer++;
			float timeperframe = (usesspeed) ? (5f / Math.Abs(NPC.velocity.X)) * speed : speed;
			if (timer >= timeperframe) {
				frame++;
				timer = 0;
			}
			if (frame >= maxframe) {
				frame = minframe;
			}

			if (frame < minframe)
				frame = minframe;
		}

		private void SyncNPC()
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
		}

		private void NextAttack(bool skipto4 = false) //reset most variables and netupdate to sync the boss in multiplayer
		{
			trailbehind = false;
			if (skipto4)
				AttackType = 4;
			else
				AttackType++;
			AiTimer = 0;
			NPC.ai[2] = 0;
			NPC.rotation = 0;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			hasjumped = false;
			NPC.behindTiles = false;
			NPC.knockBackResist = 0f;
			BaseVel = Vector2.UnitX;
			statictarget[0] = Vector2.Zero;
			statictarget[1] = Vector2.Zero;
			SyncNPC();
		}
		private void StepUp(Player player)
		{
			bool flag15 = true; //copy pasted collision step code from zombies
			if ((player.Center.Y * 16 - 32) > NPC.position.Y)
				flag15 = false;

			if (!flag15 && NPC.velocity.Y == 0f)
				Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);

			if (NPC.velocity.Y >= 0f)
				Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, flag15, 1);
		}
		#endregion

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(trailbehind);
			writer.Write(hasjumped);
			writer.WriteVector2(BaseVel);
			writer.Write(extraYoff);
			writer.Write(canhitplayer);
			writer.Write(NPC.knockBackResist);
			writer.Write(NPC.rotation);
			writer.Write(frame);
			writer.Write(timer);
			writer.Write(skiptimer);
			foreach (Vector2 vector in statictarget)
				writer.WriteVector2(vector);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			trailbehind = reader.ReadBoolean();
			hasjumped = reader.ReadBoolean();
			BaseVel = reader.ReadVector2();
			extraYoff = reader.ReadInt32();
			canhitplayer = reader.ReadBoolean();
			NPC.knockBackResist = reader.ReadSingle();
			NPC.rotation = reader.ReadSingle();
			frame = reader.ReadInt32();
			timer = reader.ReadInt32();
			skiptimer = reader.ReadSingle();
			for (int i = 0; i < statictarget.Length; i++)
				statictarget[i] = reader.ReadVector2();
		}

		public override int SpawnNPC(int tileX, int tileY)
		{
			NPC.velocity.Y = 1;
			return base.SpawnNPC(tileX, tileY);
		}

		private void Phase1(Player player)
		{
			Sandstorm.Happening = false;

			if (!NPC.noTileCollide && !Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0) && AttackType < 4) { //check if it can't reach the player
				if(++skiptimer > 180 || WorldGen.SolidTile((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16))) //wait 3 seconds before skipping to the attack, to mitigate cases where it isnt needed, instant skip if stuck in a tile
					NextAttack(true);
			}
			else
				skiptimer = 0;

			switch (AttackType) {
				case 0:
					Walking(player, 0.15f, 7, 360);
					break;
				case 1:
					Jumping(player);
					break;
				case 2:
					Dash(player);
					break;
				case 3:
					Walking(player, 0.2f, 10, 180);
					break;
				case 4:
					FlyDashes(player);
					break;
				case 5:
					GroundPound(player, 150);
					break;
				case 6:
					Digging(player);
					break;
				default: AttackType = 0; break; //loop attack pattern
			}

		}
		#region phase 1 attacks
		public void Walking(Player player, float acc, float maxspeed, int maxtime)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.knockBackResist = 0.7f;
			AiTimer++;
			CheckPlatform(player);
			CheckPit();
			//canhitplayer = true;

			if (NPC.velocity.Y == 0) { //simple movement ai, accelerates until it hits a cap
				NPC.velocity.X += (NPC.Center.X < player.Center.X) ? acc : -acc;
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxspeed, maxspeed);
			}

			AiTimer++;
			if (AiTimer > maxtime) {

				if (AttackType == 3) //lazy hardcoded way to make it skip flying dashes but if it works it workss
					AttackType++;

				NextAttack();
			}

			StepUp(player);
			if (NPC.collideX) {
				NPC.velocity.X *= -1;
				SyncNPC();
			}

			if (Main.rand.Next(500) == 0)
				SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44);

			UpdateFrame(4, 0, 6, true);
		}

		bool hasjumped = false;
		public void Jumping(Player player)
		{
			NPC.spriteDirection = NPC.direction;
			CheckPlatform(player);
			if (AiTimer < 30) //slow down before jumping
			{
				StepUp(player);
				if (NPC.collideX) {
					NPC.velocity.X *= -1;
					SyncNPC();
				}

				NPC.velocity.X *= 0.9f;
			}


			if (Math.Abs(NPC.velocity.X) < 1 && AiTimer < 30 && NPC.velocity.Y == 0) { //have a "charge" animation after slowing down enough and on the ground
				frame = 10;
				NPC.rotation = -0.15f * NPC.spriteDirection;
				extraYoff = 8;
				AiTimer++;
			}
			else { //normal walking animation and rotation otherwise

				if(AiTimer > 0)
					AiTimer++;

				NPC.rotation = 0;
				extraYoff = 0;
				timer++;

				UpdateFrame(3, 0, 6, true);
			}
			if (AiTimer >= 30 && NPC.velocity.Y != 0 && !hasjumped)
				AiTimer = 0; //reset charge if in the air and hasn't jumped already

			if(AiTimer == 30) { //jump towards player, at faster speed if farther away
				SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.5f, -1f);
				hasjumped = true;
				NPC.noTileCollide = true; 
				Vector2 JumpTo = new Vector2(player.Center.X, player.Center.Y - 300);
				Vector2 vel = JumpTo - NPC.Center;
				float speed = MathHelper.Clamp(vel.Length() / 36, 6, 18);
				vel.Normalize();
				vel.Y -= 0.15f;
				NPC.velocity = vel * speed;
			}

			if(hasjumped && AiTimer % 27 == 0 && AiTimer < 100 && AiTimer > 40) {
				SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					for (float i = -2; i <= 2; i += 1.25f) {
						Vector2 velocity = -Vector2.UnitY.RotatedBy(i * (float)Math.PI / 12);
						velocity *= 10f;
						velocity.Y += 2f;
						Projectile.NewProjectileDirect(NPC.Center, velocity, ModContent.ProjectileType<ScarabSandball>(), NPC.damage / 4, 1f, Main.myPlayer, 0, player.position.Y).netUpdate = true;
					}
				}
			}

			NPC.noTileCollide = hasjumped && AiTimer < 60;//temporarily disable tile collision after jump so it doesn't get stuck
			canhitplayer = trailbehind = hasjumped;

			if (hasjumped && NPC.velocity.Y == 0 && NPC.oldVelocity.Y > 0) {
				Collision.HitTiles(NPC.position, NPC.velocity, NPC.width, NPC.height);
				NPC.velocity.X /= 3;
				SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
				NextAttack();
			}
		}
		public void Dash(Player player)
		{
			AiTimer++;
			CheckPlatform(player);
			NPC.direction = Math.Sign(player.Center.X - NPC.Center.X);

			if (AiTimer <= 80) { //home in on closer side of player, do sandstorm jump if player too high up
				float homevel = (NPC.Center.X < player.Center.X) ? player.Center.X - 300 - NPC.Center.X : player.Center.X + 300 - NPC.Center.X;
				if (Math.Abs(homevel) < 20)
					homevel = 0;
				else 
					homevel = Math.Sign(homevel) * MathHelper.Clamp(Math.Abs(homevel) / 20, 5, 20);

				if(NPC.velocity.Y == 0 || NPC.velocity.Y < 0 && NPC.Center.Y > player.Center.Y) 
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, homevel, 0.05f);

				if (AiTimer > 60 && NPC.Center.Y > player.Center.Y + 100) { //sandstorm jump if too far below the player
					NPC.noTileCollide = true;
					NPC.velocity.Y = -12;
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, homevel, 0.05f);
					NPC.ai[2] ++;
					NPC.spriteDirection = (int)(2 * (Math.Floor(Math.Sin(NPC.ai[2])) + 0.5f));
					if (NPC.ai[2] % 3 == 0 || NPC.oldVelocity.Y > 0) {
						SoundEngine.PlaySound(SoundID.DoubleJump, NPC.Center);
						int g = Gore.NewGore(NPC.Center, NPC.velocity / 2, GoreID.ChimneySmoke1 + Main.rand.Next(3));
						Main.gore[g].timeLeft = 10;
					}
				}
				else
					NPC.spriteDirection = NPC.direction;
			}
			else if (AiTimer < 150)
				NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, 0, 0.05f);

			if (AiTimer >= 90) {
				if (AiTimer == 130) {
					SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.2f, -1f);
					for(int i = 0; i < 6; i++) {
						int g = Gore.NewGore(NPC.Center, Main.rand.NextVector2Circular(4, 4), GoreID.ChimneySmoke1 + Main.rand.Next(3));
						Main.gore[g].timeLeft = 15;
					}
				}

				if (AiTimer == 150) {
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), NPC.Center);
					trailbehind = true;
					NPC.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - NPC.Center.X)/30), 16, 32) * NPC.direction;
					SyncNPC();
				}

				if (NPC.direction != NPC.spriteDirection)
					NPC.velocity.X *= 0.9f;
				else
					CheckPit(0.9f, false);

				if (frame >= 11) {
					NPC.rotation += (0.025f + (Math.Abs(NPC.velocity.X) / 36)) * Math.Sign(NPC.velocity.X);
					if(AiTimer < 120)
						NPC.velocity.X = -NPC.spriteDirection * 2;
					frame = 11;
				}
				else
					UpdateFrame(6, 7, 12);
			}
			else 
				UpdateFrame(4, 0, 6, true);
			

			canhitplayer = AiTimer >= 150;

			StepUp(player);

			if (AiTimer > 210) { NextAttack();}
		}

		float rotation = 0;
		public void FlyDashes(Player player)
		{
			AiTimer++;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			UpdateFrame(4, 18, 21);
			Vector2 ToPlayer = player.Center - NPC.Center;
			ToPlayer.Normalize();

			if (AiTimer == 1)
				rotation = ToPlayer.ToRotation();

			if (AiTimer < 100) { //home in on player, but keeping some distance, also slowly rotating towards them

				NPC.spriteDirection = NPC.direction;
				float vel = MathHelper.Clamp(Math.Abs(ToPlayer.Length() - 400) / 24, 6, 60);
				NPC.velocity = (NPC.Distance(player.Center) > 480) ?
					Vector2.Lerp(NPC.velocity, ToPlayer * vel, 0.05f) :
					(NPC.Distance(player.Center) < 380) ? Vector2.Lerp(NPC.velocity, -ToPlayer * vel, 0.05f) : Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.05f);

				rotation = Utils.AngleLerp(rotation, ToPlayer.ToRotation(), 0.05f);
			}

			if (AiTimer >= 100) { //dash at player, then dash again, swapping between rotating towards the player and only having its rotation be based on its velocity

				if (AiTimer == 100 || AiTimer == 190)
					SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.2f, -1f);

				if (AiTimer == 120 || AiTimer == 210) {
					trailbehind = true;
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), NPC.Center);
					NPC.velocity = ToPlayer * MathHelper.Clamp(NPC.Distance(player.Center) / 22, 16, 30);
				}

				if((AiTimer > 100 && AiTimer < 120) || (AiTimer > 190 && AiTimer < 210)) 
					NPC.velocity = -ToPlayer * 6;

				else
					NPC.velocity *= 0.975f;

				canhitplayer = true;
				if (AiTimer > 120 && AiTimer < 150 || AiTimer > 210)
					rotation = NPC.velocity.ToRotation();
				else {
					rotation = Utils.AngleLerp(rotation, ToPlayer.ToRotation(), 0.05f);
					NPC.spriteDirection = NPC.direction;
				}
			}
			NPC.rotation = rotation + ((NPC.spriteDirection < 0) ? MathHelper.Pi : 0);
			if (AiTimer > 260) {NextAttack();}
		}
		public void GroundPound(Player player, float hometime)
		{
			if(AiTimer < hometime) { //home in on spot above player
				AiTimer++;
				NPC.noTileCollide = true;
				NPC.noGravity = true;
				UpdateFrame(4, 18, 21);
				Vector2 ToPlayer = player.Center - NPC.Center;
				ToPlayer.Y -= 350;
				if (AiTimer > hometime - (hometime / 5))
					ToPlayer.Y -= 180;
				if (Math.Abs(ToPlayer.X) > 50) //flip based on homing direction, but not if too close horizontally
					NPC.spriteDirection = NPC.direction;
				float vel = MathHelper.Clamp(ToPlayer.Length() / 18, 7, 28);
				ToPlayer.Normalize();
				NPC.velocity = Vector2.Lerp(NPC.velocity, ToPlayer * vel, 0.05f);
			}
			else {
				canhitplayer = true;
				trailbehind = true;
				NPC.noGravity = false;
				if(NPC.Center.Y > (player.position.Y - 10))
					CheckPlatform(player);
				else 
					NPC.noTileCollide = true;

				if (AiTimer == hometime) //initial tick of falling
				{
					UpdateFrame(4, 12, 17);
					NPC.rotation = NPC.spriteDirection * MathHelper.PiOver2;
					AiTimer++;
					SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.5f, -1f);
					NPC.velocity = new Vector2(0, 0.25f);
					SyncNPC();
				}
				else if (NPC.velocity.Y <= 0) //check when the boss lands
				{
					UpdateFrame(10, 18, 21);
					NPC.noGravity = true;
					NPC.rotation = 0;
					canhitplayer = false;
					if (AiTimer == 151) {
						SpiritMod.tremorTime = 15;
						NPC.velocity.Y = -3;
						SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
						SyncNPC();
					}
					NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0, 0.07f);
					if (AiTimer % 7 == 0 && AiTimer < hometime + 61) { //make shockwaves ripple outwards, 2 spawn every 10 ticks, distance from boss is based on how many ticks have passed
						for (int i = -1; i <= 1; i += 2) {
							Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 4);
							center.X += (AiTimer - 150) * 32 * i;
							int numtries = 0;
							int x = (int)(center.X / 16);
							int y = (int)(center.Y / 16);//find the lowest solid tile from the given spawn point, then increase the spawn point if inside a tile, with a limit of 10 tiles upwards
							while (y < Main.maxTilesY - 10 && Main.tile[x, y] != null && !WorldGen.SolidTile2(x, y) && Main.tile[x - 1, y] != null && !WorldGen.SolidTile2(x - 1, y) && Main.tile[x + 1, y] != null && !WorldGen.SolidTile2(x + 1, y)) {
								y++;
								center.Y = y * 16;
							}
							while ((WorldGen.SolidOrSlopedTile(x, y) || WorldGen.SolidTile2(x, y)) && numtries < 20) {
								numtries++;
								y--;
								center.Y = y * 16;
							}
							if (numtries >= 20)
								break;

							if(Main.netMode != NetmodeID.MultiplayerClient)
								Projectile.NewProjectile(center, Vector2.Zero, ModContent.ProjectileType<SandShockwave>(), NPC.damage / 4, 5f, Main.myPlayer);
						}
					}

					if (++AiTimer > hometime + 121) { NextAttack(); }
				}
				else { //if it hasnt landed yet, accelerate until at max velocity
					NPC.velocity.Y *= 1.075f;
					NPC.velocity.Y = Math.Min(NPC.velocity.Y, 18);
				}
			}
		}
		public void Digging(Player player)
		{
			bool InSolidTile = WorldGen.SolidTile((int)NPC.Center.X / 16, (int)(NPC.Center.Y / 16));
			NPC.noGravity = (InSolidTile || hasjumped);
			UpdateFrame(4, 12, 17);
			NPC.spriteDirection = Math.Sign(NPC.velocity.X);
			canhitplayer = AiTimer > 170; //only do contact damage after jumping
			NPC.direction = Math.Sign(player.Center.X - NPC.Center.X);
			float targetrotation;
			if (NPC.velocity.Length() > 3) {
				targetrotation = NPC.velocity.ToRotation();
				if (Math.Abs(targetrotation) > MathHelper.PiOver2)
					targetrotation -= MathHelper.Pi;
			}
			else
				targetrotation = 0;

			NPC.behindTiles = true;
			NPC.rotation = Utils.AngleLerp(NPC.rotation, targetrotation, 0.1f);
			trailbehind = hasjumped;
			if (InSolidTile) {

				if (AiTimer % 20 == 0)
					SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 1);

				float distfromsurface = 0; //check the npc's distance from the surface to determine dust velocity, by checking each tile and increasing this float for every solid tile above this npc until a non-solid tile is found
				Vector2 tilecheck = NPC.Center;
				while (WorldGen.SolidTile((int)(tilecheck.X / 16), (int)(tilecheck.Y / 16))) {
					tilecheck.Y -= 16;
					distfromsurface += 2;
				}
				for (int i = 0; i < 2; i++) {
					Dust dust = Dust.NewDustPerfect(NPC.Center + (Vector2.UnitY * 20), Mod.Find<ModDust>("SandDust").Type, -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / (12 + distfromsurface)) * distfromsurface * Main.rand.NextFloat(0.8f, 1.2f));
					dust.position.X += Main.rand.NextFloat(-NPC.width / 6, NPC.width / 6);
					dust.position.Y += Main.rand.NextFloat(-NPC.height / 6, -NPC.height / 6);
					dust.noGravity = true;
					dust.scale = Main.rand.NextFloat(1.8f, 2.4f);
				}
				if (NPC.velocity.Length() > 4) {
					for (int i = -2; i < 2; i++) {
						if (i != 0) {
							Dust dust = Dust.NewDustPerfect(NPC.Center, Mod.Find<ModDust>("SandDust").Type, NPC.velocity.RotatedBy(Math.Sign(i) * MathHelper.Pi / 12));
							dust.position += Main.rand.NextVector2Circular(8, 8);
							dust.noGravity = true;
							dust.scale = 1.2f;
						}
					}
				}

				if (++AiTimer < 110) { //only loosely home in on player for a few seconds, and only while in the ground
					NPC.velocity.X += (NPC.Center.X < player.Center.X) ? 0.2f : -0.2f;
					NPC.velocity.Y += (NPC.Center.Y < player.Center.Y) ? 0.1f : -0.1f;
					NPC.velocity = new Vector2(MathHelper.Clamp(NPC.velocity.X, -10, 10), MathHelper.Clamp(NPC.velocity.Y, -4, 4));
				}
				else if (AiTimer <= 200) { //if enough time has passed and the boss is in the ground, stop homing and pause its velocity
					NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);
					if ((AiTimer == 120 || AiTimer == 160) && Main.netMode != NetmodeID.Server) 
						SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1").WithVolume(0.5f).WithPitchVariance(0.2f), NPC.Center);

					if (AiTimer == 200) { //jump at the player

						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), NPC.Center);

						statictarget[0] = NPC.Center;
						statictarget[1] = player.Center;
						NPC.velocity = NPC.DirectionTo(statictarget[1]) * MathHelper.Clamp(NPC.Distance(statictarget[1]) / 40, 16, 20);
						hasjumped = true;
						SyncNPC();
					}
				}
			}
			//slow down after the distance the npc has passed is greater than the original distance between it and the player, increase ai 2 to make the distance a one time check
			if (!InSolidTile && hasjumped && (NPC.Distance(statictarget[0]) > Math.Max(Vector2.Distance(statictarget[0], statictarget[1]) * 1.3f, 640) || NPC.ai[2] > 0)) {
				NPC.ai[2]++;
				NPC.noTileCollide = false;
				CheckPlatform(player);
				if (Math.Abs(NPC.velocity.X) > 3)
					NPC.velocity.X *= 0.9f;

				if (NPC.velocity.Y == 0 && NPC.oldVelocity.Y > 0) {
					Collision.HitTiles(NPC.position, NPC.velocity, NPC.width, NPC.height);
					NPC.velocity.X = 0;
					SoundEngine.PlaySound(SoundID.Dig, NPC.Center);
					NextAttack();
				}

				if (NPC.velocity.Y < 14) {
					NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 14, 0.03f);
				}
			}
			else 
				NPC.noTileCollide = true;
		}
		#endregion
		private void Phase2(Player player)
		{
			Sandstorm.Happening = true;
			Sandstorm.TimeLeft = 2;
			Sandstorm.Severity = 1;
			Sandstorm.IntendedSeverity = 1;
			foreach (Player Player in Main.player.Where(x => x.active && !x.dead)) //probably a cleaner way to do visual only sandstorm??
				Player.buffImmune[BuffID.WindPushed] = true;

			switch (AttackType) {
				case 0:
					CircleDash(player);
					break;
				case 1:
					SideDash(player, 90);
					break;
				case 2:
					SideDash(player, 40);
					break;
				case 3:
					if (Main.expertMode)
						SideDash(player, 20);
					else
						NextAttack();
					break;
				case 4:
					ChainGroundPound(player);
					break;
				case 5:
					SandSpit(player);
					break;
				case 6:
					LargeScarabs(player);
					break;
				case 7: 
					SideDash(player, 80, 1); 
					break;
				default: AttackType = 0; break; //loop attack pattern
			}
		}
		Vector2 BaseVel = Vector2.UnitX;
		#region phase 2 attacks
		private void CircleDash(Player player)
		{
			AiTimer++;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			UpdateFrame(4, 18, 21);
			float targetrotation;

			if (AiTimer < 60) { //fly away from player until attack starts
				NPC.spriteDirection = NPC.direction;
				targetrotation = NPC.AngleTo(player.Center);
				if (Math.Abs(targetrotation) > MathHelper.PiOver2)
					targetrotation -= MathHelper.Pi;
				NPC.rotation = Utils.AngleLerp(NPC.rotation, targetrotation, 0.1f);
				NPC.velocity = (NPC.Distance(player.Center) < 600) ? Vector2.Lerp(NPC.velocity, NPC.DirectionFrom(player.Center) * 12, 0.06f) :
					(NPC.Distance(player.Center) > 700) ? Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 8, 0.05f) : Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.05f);
			}
			else {
				trailbehind = true;
				canhitplayer = true;
				targetrotation = NPC.velocity.ToRotation();
				if (NPC.spriteDirection < 0)
					targetrotation -= MathHelper.Pi;

				int numwaves = (Main.expertMode) ? 2 : 1;
				if (AiTimer == 60) { //when the spin starts, save the initial velocity of the spin to rotate each tick, and store the player's center and a random spot far away from them
					BaseVel = Vector2.UnitX.RotatedBy(NPC.rotation) * NPC.spriteDirection * 2;
					statictarget[0] = Main.rand.NextVector2CircularEdge(1200, 1200);
					statictarget[1] = player.Center;
					if (Main.netMode != NetmodeID.MultiplayerClient) { //spawn the telegraph for the scarab storm, going from the random spot to the player's center
						for (int i = 0; i < numwaves; i++) {
							Vector2 spawnpos = statictarget[0].RotatedBy(MathHelper.PiOver2 * i) + statictarget[1];
							Projectile proj = Projectile.NewProjectileDirect(spawnpos, Vector2.Normalize(statictarget[1] - spawnpos) * 6, ModContent.ProjectileType<SwarmTelegraph>(), 0, 0, Main.myPlayer);
							SoundEngine.PlaySound(SoundID.Item, (int)statictarget[1].X, (int)statictarget[1].Y, 117, 1, 2);
							proj.netUpdate = true;
						}
					}
					NPC.netUpdate = true;
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), NPC.Center);
				}
				if(BaseVel.Length() < 24)
					BaseVel *= 1.1f;

				NPC.rotation = targetrotation;
				NPC.velocity = BaseVel.RotatedBy(MathHelper.ToRadians((AiTimer - 60) * 12));

				if(AiTimer >= 90 && AiTimer % 7 == 0) { //spawn the swarm of beetles, going from the stored random position to the player's center
					SoundEngine.PlaySound(SoundID.Item, (int)statictarget[1].X, (int)statictarget[1].Y, 1, 1, Main.rand.NextFloat(1.5f, 2f));
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						for (int i = 0; i < numwaves; i++) {
							Vector2 spawnpos = statictarget[0].RotatedBy(MathHelper.PiOver2 * i) + Main.rand.NextVector2Circular(60, 60) + statictarget[1];
							NPC Npc = Main.npc[NPC.NewNPC((int)spawnpos.X, (int)spawnpos.Y, ModContent.NPCType<SwarmScarab>(), NPC.whoAmI,
								Vector2.Normalize(statictarget[1] - spawnpos).ToRotation(), 1)];
							SoundEngine.PlaySound(SoundID.Zombie, (int)Npc.position.X, (int)Npc.position.Y, 44, 0.5f);
							Npc.netUpdate = true;
						}
					}
				}
				if(AiTimer > 180) { NextAttack(); }
			}
		}
		private void SideDash(Player player, int hometime, int hometype = 0)
		{
			AiTimer++;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			UpdateFrame(4, 18, 21);
			NPC.direction = Math.Sign(player.Center.X - NPC.Center.X);
			if (AiTimer < hometime) {
				Vector2 homeCenter = player.Center;
				NPC.spriteDirection = NPC.direction;
				if (hometype == 0) //choose closest spot
					homeCenter.X += (NPC.Center.X < player.Center.X) ? -280 : 280;
				else //choose spot in front of player
				{
					if(NPC.ai[2] == 0)  //choose spot only on first tick
						NPC.ai[2] = (player.velocity.X < 0) ? -1 : 1;
					
					homeCenter.X += NPC.ai[2] * 340;
				}
				float vel = MathHelper.Clamp(NPC.Distance(homeCenter) / 18, 7, 18);
				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(homeCenter) * vel, 0.05f);
			}
			else if(AiTimer < hometime + 28) { //rotate backwards to telegraph dash
				if(AiTimer == hometime)
					SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.5f, -1f);

				NPC.rotation = Utils.AngleLerp(NPC.rotation, NPC.spriteDirection * -MathHelper.Pi / 6, 0.1f);
				NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);
			}
			else if(AiTimer == hometime + 28) { //dash towards sprite direction
				NPC.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - NPC.Center.X) / 14), 14, 26) * NPC.spriteDirection;
				canhitplayer = true;
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), NPC.Center);
				trailbehind = true;
				NPC.rotation = NPC.velocity.X * 0.04f;
			}
			else if(AiTimer < hometime + 88) { //slow down after enough time has passed and the boss has already moved past the player
				canhitplayer = true;
				if(NPC.spriteDirection != NPC.direction)
					NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.05f);
				NPC.rotation = NPC.velocity.X * 0.04f;
			}
			else { NextAttack(); }
		}
		private void ChainGroundPound(Player player)
		{
			float groundpoundtimer = (NPC.ai[2] == 0) ? 100 : 30;
			if (NPC.ai[2] < 3) {
				if (AiTimer <= groundpoundtimer) //reuse the same ground pound ai until the boss starts falling
					GroundPound(player, groundpoundtimer);
				else { //mostly the same ground pound code, however shockwaves are all spawned at the same time, and the boss's velocity is increased to help it get to its target spot in time
					canhitplayer = true;
					if (NPC.Center.Y > (player.position.Y - 10))
						CheckPlatform(player);
					else
						NPC.noTileCollide = true;
					trailbehind = true;
					NPC.noGravity = false;
					if (NPC.velocity.Y <= 0 && NPC.ai[2] < 3) {
						SpiritMod.tremorTime = 15;
						NPC.velocity.Y = (NPC.ai[2] < 2) ? -7 : -4;
						if (NPC.ai[2] < 2)
							NPC.velocity.X = NPC.direction * 7;
						SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
						if (NPC.ai[2] == 0 || Main.expertMode) {
							for (int j = 1; j <= 4; j++) {
								for (int i = -1; i <= 1; i += 2) {
									Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 4);
									center.X += j * 240 * i;
									int numtries = 0;
									int x = (int)(center.X / 16);
									int y = (int)(center.Y / 16);
									while (y < Main.maxTilesY - 10 && Main.tile[x, y] != null && !WorldGen.SolidTile2(x, y) && Main.tile[x - 1, y] != null && !WorldGen.SolidTile2(x - 1, y) && Main.tile[x + 1, y] != null && !WorldGen.SolidTile2(x + 1, y)) {
										y++;
										center.Y = y * 16;
									}
									while ((WorldGen.SolidOrSlopedTile(x, y) || WorldGen.SolidTile2(x, y)) && numtries < 10) {
										numtries++;
										y--;
										center.Y = y * 16;
									}
									if (numtries >= 10)
										break;

									if (Main.netMode != NetmodeID.MultiplayerClient)
										Projectile.NewProjectile(center, Vector2.Zero, ModContent.ProjectileType<SandShockwave>(), NPC.damage / 4, 5f, Main.myPlayer);
								}
							}
						}
						AiTimer = 0;
						NPC.rotation = 0;
						NPC.ai[2]++;
					}
					else { //if it hasnt landed yet, accelerate until at max velocity
						NPC.velocity.Y *= 1.075f;
						NPC.velocity.Y = Math.Min(NPC.velocity.Y, 18);
					}
				}
			}
			if(NPC.ai[2] >= 3) { //rest for a bit after the attack, for easy hits
				AiTimer++;
				UpdateFrame(10, 18, 21);
				NPC.noGravity = true;
				NPC.rotation = 0;
				canhitplayer = false;
				trailbehind = false;
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, 0, 0.07f);
				if (AiTimer > 150)
					NextAttack(); 
			}
		}
		readonly Vector2[] statictarget = new Vector2[2] { Vector2.Zero, Vector2.Zero };
		private void SandSpit(Player player)
		{
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			UpdateFrame(3, 18, 21);
			NPC.spriteDirection = Math.Sign(NPC.velocity.X);
			if (NPC.ai[2] == 0) {
				NPC.ai[2] = Main.rand.NextBool() ? 1 : -1;
				NPC.netUpdate = true;
			}
			Vector2 homepos = statictarget[0];
			homepos.Y -= 220;
			homepos.X -= 500 * NPC.ai[2];
			if (AiTimer == 0) //initial tick increase to check if the npc is close enough to its target point before beginning the rest of its ai, also stores initial player center
			{
				statictarget[0] = player.Center;
				if(NPC.Distance(homepos) < 30)
					AiTimer++;
			}
			else if (AiTimer > 0) {
				AiTimer++;
				statictarget[0].Y = player.Center.Y;
				if (NPC.Distance(homepos) < 30) //switch directions when close enough to its target position
					NPC.ai[2] *= -1;

				if (AiTimer % 16 == 0) { //spit out sand with random angles and slightly varying velocity, rng is ok to use here since everything has plenty of time to be reacted to
					int numproj = 2;
					SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						for (int i = 0; i < numproj; i++) {
							Projectile proj = Projectile.NewProjectileDirect(NPC.Center,
								new Vector2(-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 2).X * 1.5f, -1) * Main.rand.NextFloat(8, 11),
								ModContent.ProjectileType<ScarabSandball>(), NPC.damage / 5, 1, Main.myPlayer, 1, player.position.Y);
							proj.netUpdate = true;
						}
					}
				}
				if (AiTimer % 30 == 0) { //trap player in with shockwaves, location is based on the player's initial x position and current y position
					for (int i = -1; i <= 1; i += 2) {
						Vector2 center = new Vector2(statictarget[0].X, statictarget[0].Y + player.height / 4);
						center.X += 600 * i;
						int numtries = 0;
						int x = (int)(center.X / 16);
						int y = (int)(center.Y / 16);
						while (y < Main.maxTilesY - 10 && Main.tile[x, y] != null && !WorldGen.SolidTile2(x, y) && Main.tile[x - 1, y] != null && !WorldGen.SolidTile2(x - 1, y) && Main.tile[x + 1, y] != null && !WorldGen.SolidTile2(x + 1, y)) {
							y++;
							center.Y = y * 16;
						}
						while ((WorldGen.SolidOrSlopedTile(x, y) || WorldGen.SolidTile2(x, y)) && numtries < 10) {
							numtries++;
							y--;
							center.Y = y * 16;
						}
						if (numtries >= 10)
							break;

						if (Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectile(center, Vector2.Zero, ModContent.ProjectileType<SandShockwave>(), NPC.damage / 5, 5f, Main.myPlayer);
					}
				}
				NPC.rotation = NPC.velocity.X * -0.05f;
			}
			float vel = MathHelper.Clamp(NPC.Distance(homepos) / 16, 14, 24);
			NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(homepos) * vel, 0.09f);
			if (AiTimer > 250)
				NextAttack();
		}
		private void LargeScarabs(Player player)
		{
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			UpdateFrame(5, 18, 21);
			//npc.knockBackResist = 0.5f;
			NPC.spriteDirection = NPC.direction;
			NPC.velocity.X += (NPC.Center.X < player.Center.X) ? 0.3f : -0.3f;
			NPC.velocity.Y += (NPC.Center.Y < player.Center.Y) ? 0.2f : -0.2f;
			NPC.velocity = new Vector2(MathHelper.Clamp(NPC.velocity.X, -10, 10), MathHelper.Clamp(NPC.velocity.Y, -4, 4));
			if((++AiTimer == 30 || AiTimer == 90)) { //spawn 2 waves of large scarabs
				SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 44, 1.5f, -1f);
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					for (int i = 0; i < 3; i++) {
						Projectile proj = Projectile.NewProjectileDirect(player.Center - new Vector2(Main.rand.Next(-200, 200), 200), -Vector2.UnitY, ModContent.ProjectileType<LargeScarab>(), NPC.damage / 5, 1, Main.myPlayer, player.whoAmI, Main.rand.Next(20));
						proj.netUpdate = true;
					}
				}
			}
			if(AiTimer >= 220) {
				NextAttack();
			}
		}
		#endregion
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => canhitplayer;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(-10 * NPC.spriteDirection, NPC.gfxOffY - 16 + extraYoff).RotatedBy(NPC.rotation), NPC.frame,
							 lightColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			if (trailbehind) {
				Vector2 drawOrigin = NPC.frame.Size() / 2;
				for (int k = 0; k < NPC.oldPos.Length; k++) {
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + new Vector2(NPC.width/2, NPC.height/2) + new Vector2(-10 * NPC.spriteDirection, NPC.gfxOffY - 16 + extraYoff).RotatedBy(NPC.rotation);
					Color color = NPC.GetAlpha(lightColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Mod.GetTexture("NPCs/Boss/Scarabeus/Scarabeus_Glow"), NPC.Center - Main.screenPosition + new Vector2(-10 * NPC.spriteDirection, NPC.gfxOffY - 16 + extraYoff).RotatedBy(NPC.rotation), NPC.frame,
							 Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection, -1f, 0, default, 1f);
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback *= 0.7f;
			if(Main.player[projectile.owner].HeldItem.type == ItemID.Minishark) { //shadow nerfing minishark on scarab because meme balance weapon
				knockback *= 0.5f;
				int maxdamage = (Main.rand.Next(3, 6));
				while(damage - (NPC.defense / 2) + (Main.player[projectile.owner].armorPenetration * 0.33f) > maxdamage) {
					damage--;
				}
			}

			if (!Main.player[projectile.owner].ZoneDesert)
				damage /= 3;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (!player.ZoneDesert)
				damage /= 3;
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7143f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.626f);
		}

		public override bool PreKill()
		{
			MyWorld.downedScarabeus = true;
			Sandstorm.Happening = false;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			NPC.PlayDeathSound("ScarabDeathSound");
			return true;
		}

		public override void OnKill()
		{
			Gores();
			if (Main.expertMode) {
				NPC.DropBossBags();
				return;
			}
			NPC.DropItem(ModContent.ItemType<Chitin>(), 25, 36);

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<LocustCrook>(),
				ModContent.ItemType<RoyalKhopesh>(),
				ModContent.ItemType<RadiantCane>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			NPC.DropItem(lootTable[loot]);

			NPC.DropItem(ModContent.ItemType<ScarabMask>(), 1f / 7);
			NPC.DropItem(ModContent.ItemType<Trophy1>(), 1f / 10);
			NPC.DropItem(ModContent.ItemType<SandsOfTime>(), 1f / 15);
		}

		private void Gores()
		{
			for (int i = 1; i <= 7; i++) 
				Gore.NewGoreDirect(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Scarabeus/Scarab" + i.ToString()).Type, 1f);

			NPC.position += NPC.Size / 2;
			NPC.Size = new Vector2(100, 60);
			NPC.position -= NPC.Size / 2;

			int randomDustType()
			{
				switch (Main.rand.Next(3)) {
					case 0: return 5;
					case 1: return 36;
					default: return 32;
				}
			}

			for (int i = 0; i < 30; i++)
				Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, randomDustType(), 0f, 0f, 100, default, Main.rand.NextBool() ? 2f : 0.5f).velocity *= 3f;

			for (int j = 0; j < 50; j++) {
				Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, randomDustType(), 0f, 0f, 100, default, 1f);
				dust.velocity *= 5f;
				dust.noGravity = true;

				Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, randomDustType(), 0f, 0f, 100, default, .82f).velocity *= 2f;
			}
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 1.4f;
			name = "Scarabeus";
			downedCondition = () => MyWorld.downedScarabeus;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Scarabeus>()
				},
				new List<int> {
					ModContent.ItemType<ScarabIdol>()
				},
				new List<int> {
					ModContent.ItemType<Trophy1>(),
					ModContent.ItemType<ScarabMask>(),
					ModContent.ItemType<ScarabBox>()
				},
				new List<int> {
					ModContent.ItemType<ScarabPendant>(),
					ModContent.ItemType<Chitin>(),
					ModContent.ItemType<ScarabBow>(),
					ModContent.ItemType<LocustCrook>(),
					ModContent.ItemType<RoyalKhopesh>(),
					ModContent.ItemType<RadiantCane>(),
					ModContent.ItemType<SandsOfTime>(),
					ItemID.LesserHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<ScarabIdol>()}] in the Desert during the daytime. A [i:{ModContent.ItemType<ScarabIdol>()}] can be found upon completing a certain Adventurer quest, or can be crafted, and is non-consumable.";
			texture = "SpiritMod/Textures/BossChecklist/ScarabeusTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Scarabeus/Scarabeus_Head_Boss";
		}
	}
}
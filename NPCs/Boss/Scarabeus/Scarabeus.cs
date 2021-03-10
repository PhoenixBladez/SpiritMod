using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow.AdornedBow;
using SpiritMod.Items.Weapon.Summon.LocustCrook;
using SpiritMod.Items.Weapon.Swung.Khopesh;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	[AutoloadBossHead]
	public class Scarabeus : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarabeus");
			Main.npcFrameCount[npc.type] = 22;
			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 64;
			npc.value = 30000;
			npc.damage = 30;
			npc.defense = 10;
			npc.lifeMax = 1750;
			npc.knockBackResist = 0.3f;
			npc.aiStyle = -1;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Scarabeus");
			npc.boss = true;
			//npc.stepSpeed /= 20;
			npc.npcSlots = 15f;
			npc.HitSound = SoundID.NPCHit31;
			npc.DeathSound = SoundID.NPCDeath5;
			bossBag = ModContent.ItemType<BagOScarabs>();
		}
		bool trailbehind;
		int frame = 0;
		float extraYoff;
		float skiptimer = 0;
		int timer = 0;
		bool canhitplayer;
		public float AiTimer {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		public float AttackType {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		public override bool CheckActive()
		{
			Player player = Main.player[npc.target];
			if (!player.active || player.dead)
				return false;

			return true;
		}
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			canhitplayer = false; //default to being unable to hit the player at the start of each tick, overrided depending on attack pattern

			if (Main.netMode != NetmodeID.Server) {
				if (frame >= 18 && frame < 21)
					SpiritMod.scarabWings.SetTo(Main.ambientVolume * MathHelper.Clamp((800 - npc.Distance(Main.LocalPlayer.Center)) / 400f, 0, 1));
				else
					SpiritMod.scarabWings.Stop();
			}
			

			if (player.dead || !player.active) {
				npc.timeLeft = 10;
				Digging(player);
				AiTimer = 0;
				npc.velocity.Y = 10;
				return;
			}
			
			if (npc.life >= (npc.lifeMax/2)) {
				{
					Phase1(player);
				}
			}
			else {
				if(npc.ai[3] == 0) {
					NextAttack();
					AttackType = 0;
					npc.ai[3]++;
				}

                Phase2(player);
                npc.defense = 4;
            }
		}
		#region utilities
		private void CheckPlatform(Player player)
		{
			bool onplatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4) { //check tiles beneath the boss to see if they are all platforms
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onplatform = false;
			}
			if (onplatform && (npc.Center.Y < player.position.Y - 20)) //if they are and the player is lower than the boss, temporarily let the boss ignore tiles to go through them
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}

		private void CheckPit(float velmult = 1.7f, bool boostsxvel = true) //quirky lazy bad code but it works mostly and making the boss not break on vanilla random worldgen is tiring
		{
			if (npc.velocity.Y != 0)
				return;

			bool pit = true;
			int pitwidth = 0;
			int width = 5;
			int height = 8;
			for (int j = 1; j <= width; j++) {
				for (int i = 1; i <= height; i++) {
					Tile forwardtile = Framing.GetTileSafely(new Point((int)(npc.Center.X / 16) + (npc.spriteDirection * j), (int)(npc.Center.Y / 16) + i));
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
				npc.velocity.Y -= pitwidth * velmult;
				if (boostsxvel)
					npc.velocity.X = npc.spriteDirection * pitwidth * velmult;
			}
			else if (pit)
				npc.velocity.X *= -1f;
		}

		private void UpdateFrame(int speed, int minframe, int maxframe, bool usesspeed = false) //method of updating the frame without copy pasting this every time animation is needed
		{
			timer++;
			float timeperframe = (usesspeed) ? (5f / Math.Abs(npc.velocity.X)) * speed : speed;
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
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
		}

		private void NextAttack(bool skipto4 = false) //reset most variables and netupdate to sync the boss in multiplayer
		{
			trailbehind = false;
			if (skipto4)
				AttackType = 4;
			else
				AttackType++;
			AiTimer = 0;
			npc.ai[2] = 0;
			npc.rotation = 0;
			npc.noTileCollide = false;
			npc.noGravity = false;
			hasjumped = false;
			npc.behindTiles = false;
			npc.knockBackResist = 0f;
			BaseVel = Vector2.UnitX;
			statictarget[0] = Vector2.Zero;
			statictarget[1] = Vector2.Zero;
			SyncNPC();
		}
		private void StepUp(Player player)
		{
			bool flag15 = true; //copy pasted collision step code from zombies
			if ((player.Center.Y * 16 - 32) > npc.position.Y)
				flag15 = false;

			if (!flag15 && npc.velocity.Y == 0f)
				Collision.StepDown(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);

			if (npc.velocity.Y >= 0f)
				Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, flag15, 1);
		}
		#endregion

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(trailbehind);
			writer.Write(hasjumped);
			writer.WriteVector2(BaseVel);
			writer.Write(extraYoff);
			writer.Write(canhitplayer);
			writer.Write(npc.knockBackResist);
			writer.Write(npc.rotation);
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
			npc.knockBackResist = reader.ReadSingle();
			npc.rotation = reader.ReadSingle();
			frame = reader.ReadInt32();
			timer = reader.ReadInt32();
			skiptimer = reader.ReadSingle();
			for (int i = 0; i < statictarget.Length; i++)
				statictarget[i] = reader.ReadVector2();
		}

		public override int SpawnNPC(int tileX, int tileY)
		{
			npc.velocity.Y = 1;
			return base.SpawnNPC(tileX, tileY);
		}

		private void Phase1(Player player)
		{
			Sandstorm.Happening = false;

			if (!npc.noTileCollide && !Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) && AttackType < 4) { //check if it can't reach the player
				if(++skiptimer > 180) //wait 3 seconds before skipping to the attack, to mitigate cases where it isnt needed
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
			npc.spriteDirection = npc.direction;
			npc.knockBackResist = 1f;
			AiTimer++;
			CheckPlatform(player);
			CheckPit();
			canhitplayer = true;

			if (npc.velocity.Y == 0) { //simple movement ai, accelerates until it hits a cap
				npc.velocity.X += (npc.Center.X < player.Center.X) ? acc : -acc;
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -maxspeed, maxspeed);
			}

			AiTimer++;
			if (AiTimer > maxtime) {

				if (AttackType == 3) //lazy hardcoded way to make it skip flying dashes but if it works it workss
					AttackType++;

				NextAttack();
			}

			StepUp(player);
			if (npc.collideX) {
				npc.velocity.X *= -1;
				SyncNPC();
			}

			if (Main.rand.Next(500) == 0)
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44);

			UpdateFrame(4, 0, 6, true);
		}

		bool hasjumped = false;
		public void Jumping(Player player)
		{
			npc.spriteDirection = npc.direction;
			CheckPlatform(player);
			if (AiTimer < 30) //slow down before jumping
			{
				StepUp(player);
				if (npc.collideX) {
					npc.velocity.X *= -1;
					SyncNPC();
				}

				npc.velocity.X *= 0.9f;
			}


			if (Math.Abs(npc.velocity.X) < 1 && AiTimer < 30 && npc.velocity.Y == 0) { //have a "charge" animation after slowing down enough and on the ground
				frame = 10;
				npc.rotation = -0.15f * npc.spriteDirection;
				extraYoff = 8;
				AiTimer++;
			}
			else { //normal walking animation and rotation otherwise

				if(AiTimer > 0)
					AiTimer++;

				npc.rotation = 0;
				extraYoff = 0;
				timer++;

				UpdateFrame(3, 0, 6, true);
			}
			if (AiTimer >= 30 && npc.velocity.Y != 0 && !hasjumped)
				AiTimer = 0; //reset charge if in the air and hasn't jumped already

			if(AiTimer == 30) { //jump towards player, at faster speed if farther away
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
				hasjumped = true;
				npc.noTileCollide = true; 
				Vector2 JumpTo = new Vector2(player.Center.X, player.Center.Y - 300);
				Vector2 vel = JumpTo - npc.Center;
				float speed = MathHelper.Clamp(vel.Length() / 36, 6, 18);
				vel.Normalize();
				vel.Y -= 0.15f;
				npc.velocity = vel * speed;
			}

			if(hasjumped && AiTimer % 27 == 0 && AiTimer < 100 && AiTimer > 40) {
				Main.PlaySound(SoundID.Item5, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					for (float i = -2; i <= 2; i += 1.25f) {
						Vector2 velocity = -Vector2.UnitY.RotatedBy(i * (float)Math.PI / 12);
						velocity *= 10f;
						velocity.Y += 2f;
						Projectile.NewProjectileDirect(npc.Center, velocity, mod.ProjectileType("ScarabSandball"), npc.damage / 4, 1f, Main.myPlayer, 0, player.position.Y).netUpdate = true;
					}
				}
			}

			npc.noTileCollide = hasjumped && AiTimer < 60;//temporarily disable tile collision after jump so it doesn't get stuck
			canhitplayer = trailbehind = hasjumped;

			if (hasjumped && npc.velocity.Y == 0 && npc.oldVelocity.Y > 0) {
				Collision.HitTiles(npc.position, npc.velocity, npc.width, npc.height);
				npc.velocity.X /= 3;
				Main.PlaySound(SoundID.Dig, npc.Center);
				NextAttack();
			}
		}
		public void Dash(Player player)
		{
			AiTimer++;
			CheckPlatform(player);
			npc.direction = Math.Sign(player.Center.X - npc.Center.X);

			if (AiTimer <= 80) { //home in on closer side of player, do sandstorm jump if player too high up
				float homevel = (npc.Center.X < player.Center.X) ? player.Center.X - 300 - npc.Center.X : player.Center.X + 300 - npc.Center.X;
				if (Math.Abs(homevel) < 20)
					homevel = 0;
				else 
					homevel = Math.Sign(homevel) * MathHelper.Clamp(Math.Abs(homevel) / 20, 5, 20);

				if(npc.velocity.Y == 0 || npc.velocity.Y < 0 && npc.Center.Y > player.Center.Y) 
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, homevel, 0.05f);

				if (AiTimer > 60 && npc.Center.Y > player.Center.Y + 100) { //sandstorm jump if too far below the player
					npc.noTileCollide = true;
					npc.velocity.Y = -12;
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, homevel, 0.05f);
					npc.ai[2] ++;
					npc.spriteDirection = (int)(2 * (Math.Floor(Math.Sin(npc.ai[2])) + 0.5f));
					if (npc.ai[2] % 3 == 0 || npc.oldVelocity.Y > 0) {
						Main.PlaySound(SoundID.DoubleJump, npc.Center);
						int g = Gore.NewGore(npc.Center, npc.velocity / 2, GoreID.ChimneySmoke1 + Main.rand.Next(3));
						Main.gore[g].timeLeft = 10;
					}
				}
				else
					npc.spriteDirection = npc.direction;
			}
			else if (AiTimer < 150)
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.05f);

			if (AiTimer >= 90) {
				if (AiTimer == 130) {
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.2f, -1f);
					for(int i = 0; i < 6; i++) {
						int g = Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(4, 4), GoreID.ChimneySmoke1 + Main.rand.Next(3));
						Main.gore[g].timeLeft = 15;
					}
				}

				if (AiTimer == 150) {
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), npc.Center);
					trailbehind = true;
					npc.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - npc.Center.X)/30), 16, 32) * npc.direction;
					SyncNPC();
				}

				if (npc.direction != npc.spriteDirection)
					npc.velocity.X *= 0.9f;
				else
					CheckPit(0.9f, false);

				if (frame >= 11) {
					npc.rotation += (0.025f + (Math.Abs(npc.velocity.X) / 36)) * Math.Sign(npc.velocity.X);
					if(AiTimer < 120)
						npc.velocity.X = -npc.spriteDirection * 2;
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
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(4, 18, 21);
			Vector2 ToPlayer = player.Center - npc.Center;
			ToPlayer.Normalize();

			if (AiTimer == 1)
				rotation = ToPlayer.ToRotation();

			if (AiTimer < 100) { //home in on player, but keeping some distance, also slowly rotating towards them

				npc.spriteDirection = npc.direction;
				float vel = MathHelper.Clamp(Math.Abs(ToPlayer.Length() - 400) / 24, 6, 60);
				npc.velocity = (npc.Distance(player.Center) > 480) ?
					Vector2.Lerp(npc.velocity, ToPlayer * vel, 0.05f) :
					(npc.Distance(player.Center) < 380) ? Vector2.Lerp(npc.velocity, -ToPlayer * vel, 0.05f) : Vector2.Lerp(npc.velocity, Vector2.Zero, 0.05f);

				rotation = Utils.AngleLerp(rotation, ToPlayer.ToRotation(), 0.05f);
			}

			if (AiTimer >= 100) { //dash at player, then dash again, swapping between rotating towards the player and only having its rotation be based on its velocity

				if (AiTimer == 100 || AiTimer == 190)
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.2f, -1f);

				if (AiTimer == 120 || AiTimer == 210) {
					trailbehind = true;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), npc.Center);
					npc.velocity = ToPlayer * MathHelper.Clamp(npc.Distance(player.Center) / 22, 16, 30);
				}

				if((AiTimer > 100 && AiTimer < 120) || (AiTimer > 190 && AiTimer < 210)) 
					npc.velocity = -ToPlayer * 6;

				else
					npc.velocity *= 0.975f;

				canhitplayer = true;
				if (AiTimer > 120 && AiTimer < 150 || AiTimer > 210)
					rotation = npc.velocity.ToRotation();
				else {
					rotation = Utils.AngleLerp(rotation, ToPlayer.ToRotation(), 0.05f);
					npc.spriteDirection = npc.direction;
				}
			}
			npc.rotation = rotation + ((npc.spriteDirection < 0) ? MathHelper.Pi : 0);
			if (AiTimer > 260) {NextAttack();}
		}
		public void GroundPound(Player player, float hometime)
		{
			if(AiTimer < hometime) { //home in on spot above player
				AiTimer++;
				npc.noTileCollide = true;
				npc.noGravity = true;
				UpdateFrame(4, 18, 21);
				Vector2 ToPlayer = player.Center - npc.Center;
				ToPlayer.Y -= 350;
				if (AiTimer > hometime - (hometime / 5))
					ToPlayer.Y -= 180;
				if (Math.Abs(ToPlayer.X) > 50) //flip based on homing direction, but not if too close horizontally
					npc.spriteDirection = npc.direction;
				float vel = MathHelper.Clamp(ToPlayer.Length() / 18, 7, 28);
				ToPlayer.Normalize();
				npc.velocity = Vector2.Lerp(npc.velocity, ToPlayer * vel, 0.05f);
			}
			else {
				canhitplayer = true;
				trailbehind = true;
				npc.noGravity = false;
				if(npc.Center.Y > (player.position.Y - 10))
					CheckPlatform(player);
				else 
					npc.noTileCollide = true;

				if (AiTimer == hometime) //initial tick of falling
				{
					UpdateFrame(4, 12, 17);
					npc.rotation = npc.spriteDirection * MathHelper.PiOver2;
					AiTimer++;
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
					npc.velocity = new Vector2(0, 0.25f);
					SyncNPC();
				}
				else if (npc.velocity.Y <= 0) //check when the boss lands
				{
					UpdateFrame(10, 18, 21);
					npc.noGravity = true;
					npc.rotation = 0;
					canhitplayer = false;
					if (AiTimer == 151) {
						SpiritMod.tremorTime = 15;
						npc.velocity.Y = -3;
						Main.PlaySound(SoundID.Item14, npc.Center);
						SyncNPC();
					}
					npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0, 0.07f);
					if (AiTimer % 7 == 0 && AiTimer < hometime + 61) { //make shockwaves ripple outwards, 2 spawn every 10 ticks, distance from boss is based on how many ticks have passed
						for (int i = -1; i <= 1; i += 2) {
							Vector2 center = new Vector2(npc.Center.X, npc.Center.Y + npc.height / 4);
							center.X += (AiTimer - 150) * 32 * i;
							int numtries = 0;
							int x = (int)(center.X / 16);
							int y = (int)(center.Y / 16);//find the lowest solid tile from the given spawn point, then increase the spawn point if inside a tile, with a limit of 10 tiles upwards
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

							if(Main.netMode != NetmodeID.MultiplayerClient)
								Projectile.NewProjectile(center, Vector2.Zero, mod.ProjectileType("SandShockwave"), npc.damage / 4, 5f, Main.myPlayer);
						}
					}

					if (++AiTimer > hometime + 121) { NextAttack(); }
				}
				else { //if it hasnt landed yet, accelerate until at max velocity
					npc.velocity.Y *= 1.075f;
					npc.velocity.Y = Math.Min(npc.velocity.Y, 18);
				}
			}
		}
		public void Digging(Player player)
		{
			bool InSolidTile = (WorldGen.SolidTile((int)npc.Center.X / 16, (int)(npc.Center.Y / 16)));
			npc.noGravity = (InSolidTile || hasjumped);
			UpdateFrame(4, 12, 17);
			npc.spriteDirection = Math.Sign(npc.velocity.X);
			canhitplayer = AiTimer > 170; //only do contact damage after jumping
			npc.direction = Math.Sign(player.Center.X - npc.Center.X);
			float targetrotation;
			if (npc.velocity.Length() > 3) {
				targetrotation = npc.velocity.ToRotation();
				if (Math.Abs(targetrotation) > MathHelper.PiOver2)
					targetrotation -= MathHelper.Pi;
			}
			else
				targetrotation = 0;

			npc.behindTiles = true;
			npc.rotation = Utils.AngleLerp(npc.rotation, targetrotation, 0.1f);
			trailbehind = hasjumped;
			if (InSolidTile) {

				if (AiTimer % 20 == 0)
					Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 1);

				float distfromsurface = 0; //check the npc's distance from the surface to determine dust velocity, by checking each tile and increasing this float for every solid tile above this npc until a non-solid tile is found
				Vector2 tilecheck = npc.Center;
				while (WorldGen.SolidTile((int)(tilecheck.X / 16), (int)(tilecheck.Y / 16))) {
					tilecheck.Y -= 16;
					distfromsurface += 2;
				}
				for (int i = 0; i < 2; i++) {
					Dust dust = Dust.NewDustPerfect(npc.Center + (Vector2.UnitY * 20), mod.DustType("SandDust"), -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / (12 + distfromsurface)) * distfromsurface * Main.rand.NextFloat(0.8f, 1.2f));
					dust.position.X += Main.rand.NextFloat(-npc.width / 6, npc.width / 6);
					dust.position.Y += Main.rand.NextFloat(-npc.height / 6, -npc.height / 6);
					dust.noGravity = true;
					dust.scale = Main.rand.NextFloat(1.8f, 2.4f);
				}
				if (npc.velocity.Length() > 4) {
					for (int i = -2; i < 2; i++) {
						if (i != 0) {
							Dust dust = Dust.NewDustPerfect(npc.Center, mod.DustType("SandDust"), npc.velocity.RotatedBy(Math.Sign(i) * MathHelper.Pi / 12));
							dust.position += Main.rand.NextVector2Circular(8, 8);
							dust.noGravity = true;
							dust.scale = 1.2f;
						}
					}
				}

				if (++AiTimer < 110) { //only loosely home in on player for a few seconds, and only while in the ground
					npc.velocity.X += (npc.Center.X < player.Center.X) ? 0.2f : -0.2f;
					npc.velocity.Y += (npc.Center.Y < player.Center.Y) ? 0.1f : -0.1f;
					npc.velocity = new Vector2(MathHelper.Clamp(npc.velocity.X, -10, 10), MathHelper.Clamp(npc.velocity.Y, -4, 4));
				}
				else if (AiTimer <= 200) { //if enough time has passed and the boss is in the ground, stop homing and pause its velocity
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
					if ((AiTimer == 120 || AiTimer == 160) && Main.netMode != NetmodeID.Server) 
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1").WithVolume(0.5f).WithPitchVariance(0.2f), npc.Center);

					if (AiTimer == 200) { //jump at the player

						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), npc.Center);

						statictarget[0] = npc.Center;
						statictarget[1] = player.Center;
						npc.velocity = npc.DirectionTo(statictarget[1]) * MathHelper.Clamp(npc.Distance(statictarget[1]) / 40, 16, 20);
						hasjumped = true;
						SyncNPC();
					}
				}
			}
			//slow down after the distance the npc has passed is greater than the original distance between it and the player, increase ai 2 to make the distance a one time check
			if (!InSolidTile && hasjumped && (npc.Distance(statictarget[0]) > Math.Max(Vector2.Distance(statictarget[0], statictarget[1]) * 1.3f, 640) || npc.ai[2] > 0)) {
				npc.ai[2]++;
				npc.noTileCollide = false;
				CheckPlatform(player);
				if (Math.Abs(npc.velocity.X) > 3)
					npc.velocity.X *= 0.9f;

				if (npc.velocity.Y == 0 && npc.oldVelocity.Y > 0) {
					Collision.HitTiles(npc.position, npc.velocity, npc.width, npc.height);
					npc.velocity.X = 0;
					Main.PlaySound(SoundID.Dig, npc.Center);
					NextAttack();
				}

				if (npc.velocity.Y < 14) {
					npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 14, 0.03f);
				}
			}
			else 
				npc.noTileCollide = true;
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
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(4, 18, 21);
			float targetrotation;

			if (AiTimer < 60) { //fly away from player until attack starts
				npc.spriteDirection = npc.direction;
				targetrotation = npc.AngleTo(player.Center);
				if (Math.Abs(targetrotation) > MathHelper.PiOver2)
					targetrotation -= MathHelper.Pi;
				npc.rotation = Utils.AngleLerp(npc.rotation, targetrotation, 0.1f);
				npc.velocity = (npc.Distance(player.Center) < 600) ? Vector2.Lerp(npc.velocity, npc.DirectionFrom(player.Center) * 12, 0.06f) :
					(npc.Distance(player.Center) > 700) ? Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * 8, 0.05f) : Vector2.Lerp(npc.velocity, Vector2.Zero, 0.05f);
			}
			else {
				trailbehind = true;
				canhitplayer = true;
				targetrotation = npc.velocity.ToRotation();
				if (npc.spriteDirection < 0)
					targetrotation -= MathHelper.Pi;

				int numwaves = (Main.expertMode) ? 2 : 1;
				if (AiTimer == 60) { //when the spin starts, save the initial velocity of the spin to rotate each tick, and store the player's center and a random spot far away from them
					BaseVel = Vector2.UnitX.RotatedBy(npc.rotation) * npc.spriteDirection * 2;
					statictarget[0] = Main.rand.NextVector2CircularEdge(1200, 1200);
					statictarget[1] = player.Center;
					if (Main.netMode != NetmodeID.MultiplayerClient) { //spawn the telegraph for the scarab storm, going from the random spot to the player's center
						for (int i = 0; i < numwaves; i++) {
							Vector2 spawnpos = statictarget[0].RotatedBy(MathHelper.PiOver2 * i) + statictarget[1];
							Projectile proj = Projectile.NewProjectileDirect(spawnpos, Vector2.Normalize(statictarget[1] - spawnpos) * 6, mod.ProjectileType("SwarmTelegraph"), 0, 0, Main.myPlayer);
							Main.PlaySound(SoundID.Item, (int)statictarget[1].X, (int)statictarget[1].Y, 117, 1, 2);
							proj.netUpdate = true;
						}
					}
					npc.netUpdate = true;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), npc.Center);
				}
				if(BaseVel.Length() < 24)
					BaseVel *= 1.1f;

				npc.rotation = targetrotation;
				npc.velocity = BaseVel.RotatedBy(MathHelper.ToRadians((AiTimer - 60) * 12));

				if(AiTimer >= 90 && AiTimer % 7 == 0) { //spawn the swarm of beetles, going from the stored random position to the player's center
					Main.PlaySound(SoundID.Item, (int)statictarget[1].X, (int)statictarget[1].Y, 1, 1, Main.rand.NextFloat(1.5f, 2f));
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						for (int i = 0; i < numwaves; i++) {
							Vector2 spawnpos = statictarget[0].RotatedBy(MathHelper.PiOver2 * i) + Main.rand.NextVector2Circular(60, 60) + statictarget[1];
							NPC Npc = Main.npc[NPC.NewNPC((int)spawnpos.X, (int)spawnpos.Y, mod.NPCType("SwarmScarab"), npc.whoAmI,
								Vector2.Normalize(statictarget[1] - spawnpos).ToRotation(), 1)];
							Main.PlaySound(SoundID.Zombie, (int)Npc.position.X, (int)Npc.position.Y, 44, 0.5f);
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
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(4, 18, 21);
			npc.direction = Math.Sign(player.Center.X - npc.Center.X);
			if (AiTimer < hometime) {
				Vector2 homeCenter = player.Center;
				npc.spriteDirection = npc.direction;
				if (hometype == 0) //choose closest spot
					homeCenter.X += (npc.Center.X < player.Center.X) ? -280 : 280;
				else //choose spot in front of player
				{
					if(npc.ai[2] == 0)  //choose spot only on first tick
						npc.ai[2] = (player.velocity.X < 0) ? -1 : 1;
					
					homeCenter.X += npc.ai[2] * 340;
				}
				float vel = MathHelper.Clamp(npc.Distance(homeCenter) / 18, 7, 18);
				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(homeCenter) * vel, 0.05f);
			}
			else if(AiTimer < hometime + 28) { //rotate backwards to telegraph dash
				if(AiTimer == hometime)
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);

				npc.rotation = Utils.AngleLerp(npc.rotation, npc.spriteDirection * -MathHelper.Pi / 6, 0.1f);
				npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
			}
			else if(AiTimer == hometime + 28) { //dash towards sprite direction
				npc.velocity.X = MathHelper.Clamp(Math.Abs((player.Center.X - npc.Center.X) / 14), 14, 26) * npc.spriteDirection;
				canhitplayer = true;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Scarab_Roar1"), npc.Center);
				trailbehind = true;
				npc.rotation = npc.velocity.X * 0.04f;
			}
			else if(AiTimer < hometime + 88) { //slow down after enough time has passed and the boss has already moved past the player
				canhitplayer = true;
				if(npc.spriteDirection != npc.direction)
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.05f);
				npc.rotation = npc.velocity.X * 0.04f;
			}
			else { NextAttack(); }
		}
		private void ChainGroundPound(Player player)
		{
			float groundpoundtimer = (npc.ai[2] == 0) ? 100 : 30;
			if (npc.ai[2] < 3) {
				if (AiTimer <= groundpoundtimer) //reuse the same ground pound ai until the boss starts falling
					GroundPound(player, groundpoundtimer);
				else { //mostly the same ground pound code, however shockwaves are all spawned at the same time, and the boss's velocity is increased to help it get to its target spot in time
					canhitplayer = true;
					if (npc.Center.Y > (player.position.Y - 10))
						CheckPlatform(player);
					else
						npc.noTileCollide = true;
					trailbehind = true;
					npc.noGravity = false;
					if (npc.velocity.Y <= 0 && npc.ai[2] < 3) {
						SpiritMod.tremorTime = 15;
						npc.velocity.Y = (npc.ai[2] < 2) ? -7 : -4;
						if (npc.ai[2] < 2)
							npc.velocity.X = npc.direction * 7;
						Main.PlaySound(SoundID.Item14, npc.Center);
						if (npc.ai[2] == 0 || Main.expertMode) {
							for (int j = 1; j <= 4; j++) {
								for (int i = -1; i <= 1; i += 2) {
									Vector2 center = new Vector2(npc.Center.X, npc.Center.Y + npc.height / 4);
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
										Projectile.NewProjectile(center, Vector2.Zero, mod.ProjectileType("SandShockwave"), npc.damage / 4, 5f, Main.myPlayer);
								}
							}
						}
						AiTimer = 0;
						npc.rotation = 0;
						npc.ai[2]++;
					}
					else { //if it hasnt landed yet, accelerate until at max velocity
						npc.velocity.Y *= 1.075f;
						npc.velocity.Y = Math.Min(npc.velocity.Y, 18);
					}
				}
			}
			if(npc.ai[2] >= 3) { //rest for a bit after the attack, for easy hits
				AiTimer++;
				UpdateFrame(10, 18, 21);
				npc.noGravity = true;
				npc.rotation = 0;
				canhitplayer = false;
				trailbehind = false;
				npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0, 0.07f);
				if (AiTimer > 90)
					NextAttack(); 
			}
		}
		readonly Vector2[] statictarget = new Vector2[2] { Vector2.Zero, Vector2.Zero };
		private void SandSpit(Player player)
		{
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(3, 18, 21);
			npc.spriteDirection = Math.Sign(npc.velocity.X);
			if (npc.ai[2] == 0) {
				npc.ai[2] = Main.rand.NextBool() ? 1 : -1;
				npc.netUpdate = true;
			}
			Vector2 homepos = statictarget[0];
			homepos.Y -= 220;
			homepos.X -= 500 * npc.ai[2];
			if (AiTimer == 0) //initial tick increase to check if the npc is close enough to its target point before beginning the rest of its ai, also stores initial player center
			{
				statictarget[0] = player.Center;
				if(npc.Distance(homepos) < 30)
					AiTimer++;
			}
			else if (AiTimer > 0) {
				AiTimer++;
				statictarget[0].Y = player.Center.Y;
				if (npc.Distance(homepos) < 30) //switch directions when close enough to its target position
					npc.ai[2] *= -1;

				if (AiTimer % 16 == 0) { //spit out sand with random angles and slightly varying velocity, rng is ok to use here since everything has plenty of time to be reacted to
					int numproj = 2;
					Main.PlaySound(SoundID.Item5, npc.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						for (int i = 0; i < numproj; i++) {
							Projectile proj = Projectile.NewProjectileDirect(npc.Center,
								new Vector2(-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 2).X * 1.5f, -1) * Main.rand.NextFloat(8, 11),
								mod.ProjectileType("ScarabSandball"), npc.damage / 5, 1, Main.myPlayer, 1, player.position.Y);
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
							Projectile.NewProjectile(center, Vector2.Zero, mod.ProjectileType("SandShockwave"), npc.damage / 5, 5f, Main.myPlayer);
					}
				}
				npc.rotation = npc.velocity.X * -0.05f;
			}
			float vel = MathHelper.Clamp(npc.Distance(homepos) / 16, 14, 24);
			npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(homepos) * vel, 0.09f);
			if (AiTimer > 250)
				NextAttack();
		}
		private void LargeScarabs(Player player)
		{
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(5, 18, 21);
			npc.knockBackResist = 0.5f;
			npc.spriteDirection = npc.direction;
			npc.velocity.X += (npc.Center.X < player.Center.X) ? 0.2f : -0.2f;
			npc.velocity.Y += (npc.Center.Y < player.Center.Y) ? 0.2f : -0.2f;
			npc.velocity = new Vector2(MathHelper.Clamp(npc.velocity.X, -10, 10), MathHelper.Clamp(npc.velocity.Y, -4, 4));
			if((++AiTimer == 30 || AiTimer == 90)) { //spawn 2 waves of large scarabs
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					for (int i = 0; i < 3; i++) {
						Projectile proj = Projectile.NewProjectileDirect(player.Center - new Vector2(Main.rand.Next(-200, 200), 200), -Vector2.UnitY, mod.ProjectileType("LargeScarab"), npc.damage / 5, 1, Main.myPlayer, player.whoAmI, Main.rand.Next(20));
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 16 + extraYoff).RotatedBy(npc.rotation), npc.frame,
							 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if (trailbehind) {
				Vector2 drawOrigin = npc.frame.Size() / 2;
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + new Vector2(npc.width/2, npc.height/2) + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 16 + extraYoff).RotatedBy(npc.rotation);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(mod.GetTexture("NPCs/Boss/Scarabeus/Scarabeus_Glow"), npc.Center - Main.screenPosition + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 16 + extraYoff).RotatedBy(npc.rotation), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default, 1f);
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback *= 0.6f;
			if(Main.player[projectile.owner].HeldItem.type == ItemID.Minishark) { //shadow nerfing minishark on scarab because meme balance weapon
				knockback *= 0.5f;
				int maxdamage = (Main.rand.Next(3, 6));
				while(damage - (npc.defense / 2) + (Main.player[projectile.owner].armorPenetration * 0.33f) > maxdamage) {
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

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7143f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6667f);
		}

		public override bool PreNPCLoot()
		{
			MyWorld.downedScarabeus = true;
			Sandstorm.Happening = false;
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.WorldData);
			Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/ScarabDeathSound"));
			return true;
		}

		public override void NPCLoot()
		{
			Gores();
			if (Main.expertMode) {
				npc.DropBossBags();
				return;
			}
			npc.DropItem(ModContent.ItemType<Chitin>(), 25, 36);

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<LocustCrook>(),
				ModContent.ItemType<RoyalKhopesh>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<ScarabMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy1>(), 1f / 10);
			npc.DropItem(ModContent.ItemType<DesertSnowglobe>(), 1f / 4);
		}

		private void Gores()
		{
			for (int i = 1; i <= 7; i++) 
				Gore.NewGoreDirect(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/Scarab" + i.ToString()), 1f);
			

			npc.position.X = npc.position.X + (float)(npc.width / 2);
			npc.position.Y = npc.position.Y + (float)(npc.height / 2);
			npc.width = 100;
			npc.height = 60;
			npc.position.X = npc.position.X - (float)(npc.width / 2);
			npc.position.Y = npc.position.Y - (float)(npc.height / 2);
			for (int num621 = 0; num621 < 30; num621++) {
				int randomDustType = Main.rand.Next(3);
				if (randomDustType == 0)
					randomDustType = 5;
				else if (randomDustType == 1)
					randomDustType = 36;
				else
					randomDustType = 32;

				int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0) {
					Main.dust[num622].scale = 0.5f;
				}
			}
			for (int num623 = 0; num623 < 50; num623++) {
				int randomDustType = Main.rand.Next(3);
				if (randomDustType == 0)
					randomDustType = 5;
				else if (randomDustType == 1)
					randomDustType = 36;
				else
					randomDustType = 32;

				int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default, 1f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default, .82f);
				Main.dust[num624].velocity *= 2f;
			}
		}
	}
}
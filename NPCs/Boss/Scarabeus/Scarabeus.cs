using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Projectiles.Boss;
using Steamworks;
using System;
using System.Security.Cryptography.X509Certificates;
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
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 42;
			npc.height = 42;
			npc.value = 10000;
			npc.damage = 30;
			npc.defense = 14;
			npc.lifeMax = 1700;
			npc.knockBackResist = 0f;
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
		int timer = 0;
		bool canhitplayer;
		int spriteDirectionY = 1;
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
			if (!player.ZoneDesert) {
				npc.defense = 50;
				npc.damage = 60;
			}
			else {
				npc.damage = 30;
				npc.defense = 14;
			}

			if (player.dead || !player.active) {
				npc.timeLeft = 10;
				npc.velocity.Y = 10;
				return;
			}
			
			//if (npc.life >= 800) {
			//	{
					Phase1(player);
			//	}
			//}
		}

		public void Phase1(Player player)
		{
			Sandstorm.Happening = false;

			if (!npc.noTileCollide && !Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) && AttackType != 5) { //check if it can't reach the player
				AttackType = -1;
				AiTimer = 0;
			}

			switch (AttackType) {
				case -1: FlyToPlayer(player); break; //"attack" used when the boss can't reach the player
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
				case 5: GroundPound(player);
					break;
				default: AttackType = 0; break; //loop attack pattern
			}

		}

		private void CheckPlatform(Player player)
		{
			bool onplatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4) {
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onplatform = false;
			}
			if (onplatform && (npc.Center.Y < player.position.Y)) //if they are, temporarily let the boss ignore tiles to go through them
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}

		private void UpdateFrame(int speed, int minframe, int maxframe)
		{
			timer++;
			if (timer >= speed * (5f / Math.Abs(npc.velocity.X))) {
				frame++;
				timer = 0;
			}
			if (frame >= maxframe) {
				frame = minframe;
			}

			if (frame < minframe)
				frame = minframe;
		}

		private void NextAttack()
		{
			trailbehind = false;
			AttackType++;
			AiTimer = 0;
			npc.rotation = 0;
			npc.noTileCollide = false;
			npc.noGravity = false;
			hasjumped = false;
			spriteDirectionY = 1;
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
		#region attacks
		public void Walking(Player player, float acc, float maxspeed, int maxtime)
		{
			npc.spriteDirection = npc.direction;
			AiTimer++;
			CheckPlatform(player);
			canhitplayer = true;

			if (npc.Center.X < player.Center.X)
				npc.velocity.X += acc;

			if (npc.Center.X > player.Center.X)
				npc.velocity.X -= acc;

			if (npc.velocity.X > maxspeed)
				npc.velocity.X = maxspeed;

			if (npc.velocity.X < -maxspeed)
				npc.velocity.X = -maxspeed;

			AiTimer++;
			if (AiTimer > maxtime) { NextAttack(); }

			StepUp(player);
			if (npc.collideX)
				npc.velocity.X *= -1;

			if (Main.rand.Next(500) == 0)
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44);

			UpdateFrame(4, 0, 6);
		}

		bool hasjumped = false;
		public void Jumping(Player player)
		{
			npc.spriteDirection = npc.direction;
			CheckPlatform(player);
			if (AiTimer < 30) //slow down before jumping
			{
				StepUp(player);
				if (npc.collideX)
					npc.velocity.X *= -1;

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

				UpdateFrame(3, 0, 6);
			}
			if (AiTimer >= 30 && npc.velocity.Y != 0 && !hasjumped)
				AiTimer = 0; //reset charge if in the air and hasn't jumped already

			if(AiTimer == 30) { //jump towards player, at faster speed if farther away
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
				hasjumped = true;
				npc.noTileCollide = true; 
				Vector2 JumpTo = new Vector2(player.Center.X, player.Center.Y - 300);
				Vector2 vel = JumpTo - npc.Center;
				float speed = Math.Max(Math.Min(vel.Length()/30, 18f), 6f);
				vel.Normalize();
				vel.Y -= 0.15f;
				npc.velocity = vel * speed;
			}

			if(hasjumped && AiTimer % 27 == 0 && AiTimer < 100 && AiTimer > 40) {
				Main.PlaySound(SoundID.Item5, npc.Center);
				for(float i = -2; i <= 2; i += 1.25f) {
					Vector2 velocity = -Vector2.UnitY.RotatedBy( i * (float)Math.PI / 12);
					velocity *= 10f;
					velocity.Y += 2f;
					Projectile.NewProjectile(npc.Center, velocity, mod.ProjectileType("ScarabSandball"), npc.damage / 4, 1f, Main.myPlayer);
				}
			}

			npc.noTileCollide = hasjumped && AiTimer < 60;//temporarily disable tile collision after jump so it doesn't get stuck
			canhitplayer = trailbehind = hasjumped;

			if(AiTimer > 100 && npc.velocity.Y == 0 && hasjumped) { NextAttack(); }
		}
		public void Dash(Player player)
		{
			if(npc.velocity.Y == 0)
				AiTimer++;
			CheckPlatform(player);

			if (AiTimer <= 80) { //home in on closer side of player, do sandstorm jump if player too high up
				float homevel = (npc.Center.X < player.Center.X) ? player.Center.X - 300 - npc.Center.X : player.Center.X + 300 - npc.Center.X;
				homevel = Math.Sign(homevel) * 20;
				if(npc.velocity.Y == 0)
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, homevel, 0.06f);


				if (AiTimer > 60 && npc.Center.Y > player.Center.Y + 100) { //sandstorm jump if too far below the player
					npc.noTileCollide = true;
					npc.velocity.Y = -12;
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, homevel, 0.06f);
					npc.ai[2] ++;
					npc.spriteDirection = (int)(2 * (Math.Floor(Math.Sin(npc.ai[2])) + 0.5f));
					if (npc.ai[2] % 4 == 0) {
						Main.PlaySound(SoundID.DoubleJump, npc.Center);
						int g = Gore.NewGore(npc.Center, npc.velocity / 2, GoreID.ChimneySmoke1 + Main.rand.Next(3));
						Main.gore[g].timeLeft = 10;
					}
				}
				else
					npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);
			}
			else if (AiTimer < 120 || AiTimer > 140)
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.05f);

			if (AiTimer >= 100 && AiTimer < 120) {

				frame = 10;
				npc.rotation = -0.15f * npc.spriteDirection;
				extraYoff = 8;
				if (AiTimer == 100)
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
			}
			else {
				npc.rotation = 0;
				extraYoff = 0;
				UpdateFrame(4, 0, 6);
			}

			if (AiTimer == 120) {
				Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0);
				trailbehind = true;
				npc.velocity.X = 18 * npc.direction;
			}

			canhitplayer = AiTimer >= 120;

			StepUp(player);
			if (npc.collideX)
				npc.velocity.X *= -0.25f;

			if (AiTimer > 180) { NextAttack(); }
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

			if (AiTimer < 100) {

				npc.spriteDirection = npc.direction;
				npc.velocity = (npc.Distance(player.Center) > 380) ?
					Vector2.Lerp(npc.velocity, ToPlayer * 10, 0.05f) :
					Vector2.Lerp(npc.velocity, -ToPlayer * 10, 0.05f);

				rotation = Utils.AngleLerp(rotation, ToPlayer.ToRotation(), 0.05f);
			}

			if (AiTimer >= 100) {
				npc.velocity *= 0.98f;

				if (AiTimer == 100 || AiTimer == 190)
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);

				if (AiTimer == 120 || AiTimer == 210) {
					trailbehind = true;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BossSFX/Roar"), npc.Center);
					npc.velocity = ToPlayer * 20;
				}
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

		public void FlyToPlayer(Player player)
		{
			npc.noTileCollide = true;
			npc.noGravity = true;
			UpdateFrame(4, 18, 21);

			Vector2 ToPlayer = player.Center - npc.Center;
			ToPlayer.Normalize();

			npc.velocity = Vector2.Lerp(npc.velocity, ToPlayer * 16, 0.1f);
			if(Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0)) {NextAttack();}
		}
		public void GroundPound(Player player)
		{
			AiTimer++;
			if(AiTimer < 150) { //home in on spot above player

				npc.noTileCollide = true;
				npc.noGravity = true;
				UpdateFrame(4, 18, 21);
				Vector2 ToPlayer = player.Center - npc.Center;
				ToPlayer.Y -= 350;
				ToPlayer.Normalize();
				npc.velocity = Vector2.Lerp(npc.velocity, ToPlayer * 12, 0.05f);
			}
			else {
				canhitplayer = true;
				npc.noTileCollide = false;
				trailbehind = true;
				npc.noGravity = false;
				CheckPlatform(player);
				UpdateFrame(4, 0, 6);
				if (AiTimer == 150) //initial tick of falling
				{
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 44, 1.5f, -1f);
					npc.velocity = new Vector2(0, 5);
				}
				else if (npc.velocity.Y == 0) //check when the boss lands
				{
					Main.PlaySound(SoundID.Item14, npc.Center);
					Vector2 spawnpos = new Vector2(npc.Center.X, npc.Center.Y + npc.height / 4);
					Projectile.NewProjectile(spawnpos, new Vector2(10, 0), mod.ProjectileType("DustTornado"), npc.damage / 4, 5f, Main.myPlayer);
					Projectile.NewProjectile(spawnpos, new Vector2(-10, 0), mod.ProjectileType("DustTornado"), npc.damage / 4, 5f, Main.myPlayer);
					NextAttack();
				}

			}
		}
		#endregion
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => canhitplayer;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 26 + extraYoff), npc.frame,
							 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if (trailbehind) {
				Vector2 drawOrigin = npc.frame.Size() / 2;
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + new Vector2(npc.width/2, npc.height/2) + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 26 + extraYoff);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(mod.GetTexture("NPCs/Boss/Scarabeus/Scarabeus_Glow"), npc.Center - Main.screenPosition + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 26 + extraYoff), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0) {
				for(int i = 1; i <= 7; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/Scarab" + i.ToString()), 1f);

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

					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 2f);
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

					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), .82f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.5f);
		}

		public override bool PreNPCLoot()
		{
			MyWorld.downedScarabeus = true;
			Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DeathSounds/ScarabDeathSound"));
			return true;
		}

		public override void NPCLoot()
		{
			Sandstorm.Happening = false;
			if (Main.expertMode) {
				npc.DropBossBags();
				return;
			}

			npc.DropItem(ModContent.ItemType<Chitin>(), 25, 36);

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<OrnateStaff>(),
				ModContent.ItemType<ScarabSword>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<ScarabMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy1>(), 1f / 10);
		}
	}
}
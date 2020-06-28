using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	[AutoloadBossHead]
	public class AncientFlyer : ModNPC
	{
		
		int tornadoX = 0;
		int tornadoY = 0;
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 330f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Avian");
			Main.npcFrameCount[npc.type] = 6;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 148;
			npc.height = 120;
			npc.damage = 23;
			npc.defense = 14;
			npc.lifeMax = 3100;
			npc.knockBackResist = 0;
			npc.boss = true;
			npc.noGravity = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/AncientAvian");
			npc.noTileCollide = true;
			npc.npcSlots = 5;
			bossBag = ModContent.ItemType<FlyerBag>();
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.scale = 1.1f;
		}
		bool displaycircle = false;
		private int Counter;
		float framenum = .2f;
		public override void AI()
		{
			npc.spriteDirection = -npc.direction;
			npc.rotation = npc.velocity.X * 0.07f;
			bool expertMode = Main.expertMode;
			Player player = Main.player[npc.target];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(npc.Center.X >= player.Center.X && moveSpeed >= -120) // flies to players x position
				moveSpeed--;
			else if(npc.Center.X <= player.Center.X && moveSpeed <= 120)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.10f;

			if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 350f;
			} else if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.13f;

			timer++;
			if(timer == 200 || timer == 400 && npc.life >= (npc.lifeMax / 2)) //Fires desert feathers like a shotgun
			{
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73);

				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 2);
				Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 11f;
				direction.Y *= 11f;

				int amountOfProjectiles = Main.rand.Next(8, 11);
				for(int i = 0; i < amountOfProjectiles; ++i) {
					float A = (float)Main.rand.Next(-200, 200) * 0.01f;
					float B = (float)Main.rand.Next(-200, 200) * 0.01f;
					int damage = expertMode ? 15 : 17;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<DesertFeather>(), damage, 1, Main.myPlayer, 0, 0);
				}
			} else if(timer == 300 || timer == 400 || timer == 500 || timer == 550) {
				if(Main.expertMode && npc.life >= (npc.lifeMax / 2)) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);

					Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 2);
					Main.PlaySound(SoundID.DD2_LightningBugZap, npc.position);

					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 11.5f;
					direction.Y *= 11.5f;

					int amountOfProjectiles = Main.rand.Next(5, 9);
					for(int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-300, 300) * 0.01f;
						float B = (float)Main.rand.Next(-300, 300) * 0.01f;
						int damage = expertMode ? 18 : 20;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<ExplodingFeather>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			} else if(timer == 600 || timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880) // Fires bone waves
			  {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				Main.PlayTrackedSound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				int damage = expertMode ? 15 : 19;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X * 14f, direction.Y * 14f, ModContent.ProjectileType<BoneWave>(), damage, 1, Main.myPlayer, 0, 0);
			}
			if(timer == 500 || timer == 700) {
				HomeY = -35f;
			}
			if(timer == 900) {
				Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/AvianScreech"));
			} else if(timer >= 900 && timer <= 1400) //Rains red comets
			  {
				npc.defense = 24;
				if(expertMode) {
					player.AddBuff(BuffID.WindPushed, 90);
					modPlayer.windEffect = true;
				}
				framenum = .4f;
				{
					npc.velocity = Vector2.Zero;
				}
				if(npc.life >= 1000) {
					if(Main.rand.Next(8) == 0) {
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);

						int A = Main.rand.Next(-200, 200) * 6;
						int B = Main.rand.Next(-200, 200) - 1000;
						int damage = expertMode ? 18 : 22;
						Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				} else {
					if(Main.rand.Next(9) == 0) {
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);

						int A = 1000 * (int)(Main.windSpeed / Main.windSpeed);
						int B = Main.rand.Next(-460, 460);
						int damage = expertMode ? 18 : 22;
						Projectile.NewProjectile(player.Center.X - A, player.Center.Y + B, 10f * (Main.windSpeed / Main.windSpeed), 0f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				displaycircle = true;
			} else {
				npc.defense = 14;
				framenum = .2f;
				displaycircle = false;
				modPlayer.windEffect = false;
			}

			if(timer >= 1400) {
				timer = 0;
			}


			if(npc.life <= npc.lifeMax * .6f) //Fires comets when low on health in expert
			{
				if(Main.expertMode) {
					modPlayer.windEffect = true;
					if(Main.rand.Next(22) == 0) {
						int A = Main.rand.Next(-2500, 2500) * 2;
						int B = Main.rand.Next(-1000, 1000) - 700;
						int damage = expertMode ? 15 : 17;
						Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				if(timer == 60) {
					tornadoX = (int)player.Center.X;
					tornadoY = (int)player.Center.Y;
				}
				if(timer > 60 && timer < 120) {
					for(int k = 0; k < 3; k++) {
						int dust = Dust.NewDust(new Vector2(tornadoX - 50, tornadoY + player.height - 10), 50, 10, DustID.SilverCoin, 0, -15);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].noLight = true;
						Main.dust[dust].scale = .85f;
						int dust1 = Dust.NewDust(new Vector2(tornadoX - 360, tornadoY - 250), 50, 10, DustID.SilverCoin, 0, -15);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].noLight = true;
						Main.dust[dust1].scale = .85f;
						int dust2 = Dust.NewDust(new Vector2(tornadoX + 300, tornadoY - 250), 50, 10, DustID.SilverCoin, 0, -15);
						Main.dust[dust2].noGravity = true;
						Main.dust[dust2].noLight = true;
						Main.dust[dust2].scale = .85f;
						if(npc.life <= 1000) {
							int dust6 = Dust.NewDust(new Vector2(tornadoX - 500, tornadoY + player.height - 10), 50, 10, DustID.SilverCoin, 0, -15);
							Main.dust[dust6].noGravity = true;
							Main.dust[dust6].noLight = true;
							Main.dust[dust6].scale = .85f;
							int dust7 = Dust.NewDust(new Vector2(tornadoX + 500, tornadoY + player.height - 10), 50, 10, DustID.SilverCoin, 0, -15);
							Main.dust[dust7].noGravity = true;
							Main.dust[dust7].noLight = true;
							Main.dust[dust7].scale = .85f;
						}
					}
				}
				if(timer == 120) {
					Main.PlaySound(SoundID.Item82, new Vector2(tornadoX, tornadoY));
					Main.PlaySound(SoundID.Item82, new Vector2(tornadoX - 260, tornadoY - 400));
					Main.PlaySound(SoundID.Item82, new Vector2(tornadoX + 200, tornadoY - 400));
					int damage = expertMode ? 16 : 20;
					Projectile.NewProjectile(tornadoX, tornadoY, 0f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(tornadoX - 360, tornadoY - 400, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(tornadoX + 300, tornadoY - 400, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
					if(npc.life <= 1000) {
						Projectile.NewProjectile(tornadoX - 500, tornadoY, -5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
						Projectile.NewProjectile(tornadoX + 500, tornadoY, 5f, 0f, ModContent.ProjectileType<AvianNado>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}

			if(!player.active || player.dead) //despawns when player is ded
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -50;
				timer = 0;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
		}
		public override bool PreNPCLoot()
		{
			MyWorld.downedAncientFlier = true;
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
			if(npc.velocity != Vector2.Zero) {
				for(int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			if(displaycircle) {
				Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));

				int r1 = (int)color1.R;
				drawOrigin.Y += 30f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = npc.Bottom - Main.screenPosition;
				Texture2D texture2D2 = Main.glowMaskTexture[239];
				float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
				float num12 = num11;
				if((double)num12 > 0.5)
					num12 = 1f - num11;
				if((double)num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)(((double)num11 + 0.5) % 1.0);
				float num14 = num13;
				if((double)num14 > 0.5)
					num14 = 1f - num13;
				if((double)num14 < 0.0)
					num14 = 0.0f;
				Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -40f);
				Color color3 = new Color(252, 3, 50) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * 0.75f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * 0.75f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * 0.75f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				Texture2D texture2D3 = Main.extraTexture[89];
				Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				drawOrigin = r3.Size() / 2f;
				Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
				float num17 = 1f + num13 * 0.75f;


			}
			return true;
		}
		public override void NPCLoot()
		{
			if(Main.expertMode) {
				npc.DropBossBags();
				return;
			}

			npc.DropItem(ModContent.ItemType<FossilFeather>(), 3, 6);

			int[] lootTable = { 
				ModContent.ItemType<SkeletalonStaff>(), 
				ModContent.ItemType<Talonginus>(), 
				ModContent.ItemType<SoaringScapula>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(lootTable[loot]);

			npc.DropItem(ModContent.ItemType<FlierMask>(), 1f / 7);
			npc.DropItem(ModContent.ItemType<Trophy2>(), 1f / 10);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d1 = 1;
			for(int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if(npc.life <= 0) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(42, 39));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore4"), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += framenum;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}

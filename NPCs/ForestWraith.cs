using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace SpiritMod.NPCs
{
	public class ForestWraith : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glade Wraith");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 60;
			npc.damage = 28;
			npc.defense = 10;
			npc.lifeMax = 400;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 641f;
			npc.knockBackResist = .12f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		bool rotationspawns1 = false;
		int timer = 0;
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);
			bool expertMode = Main.expertMode;
			timer++;
			if (timer == 240 || timer == 280 || timer == 320)
			{
				Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y, 0);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 10f;
				direction.Y *= 10f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-120, 120) * 0.01f;
					float B = (float)Main.rand.Next(-120, 120) * 0.01f;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("OvergrowthLeaf"), 6, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].hostile = true;
					Main.projectile[p].friendly = false;
				}
                npc.netUpdate = true;
			}
			if (timer >= 420 && timer <= 720)
			{
				int d = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, -2.5f, 0,  default(Color), 0.6f);
				npc.defense = 0;
				npc.velocity = Vector2.Zero;
				if (Main.rand.Next(52) == 0)
				{
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 3f;
					direction.Y *= 11f;					
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
					int amountOfProjectiles = Main.rand.Next(1, 1);
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-120, 120) * 0.01f;
						float B = (float)Main.rand.Next(-120, -10) * 0.01f;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, -direction.Y + B, ProjectileID.BallofFire, 8, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
					}
				}
                npc.netUpdate = true;
            }
			if (timer >= 730)
			{
				npc.defense = 10;
				Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				npc.velocity.Y = direction.Y * 6f;
				npc.velocity.X = direction.X * 6f;
				timer = 0;
                npc.netUpdate = true;
            }
			if (timer >= 750)
			{
				timer = 0;
                npc.netUpdate = true;
            }
			npc.spriteDirection = npc.direction;
			return true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.55f * bossLifeScale);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
				int d = 3;
				int d1 = 7;
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0,  default(Color), 0.3f);
					Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
				}			
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 167, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/ForestWraith_Glow"));
        }
		public override void NPCLoot()
		{
			string[] lootTable = { "SacredVine", "OakHeart" };
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientBark"), 3);
		}

	}
}

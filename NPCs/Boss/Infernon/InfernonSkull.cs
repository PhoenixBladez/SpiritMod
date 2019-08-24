using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class InfernonSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernus Skull");
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 80;

			npc.damage = 0;
			npc.lifeMax = 10;
			Main.npcFrameCount[npc.type] = 4;
			npc.alpha = 255;

			npc.boss = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.dontTakeDamage = true;
		}

		public override bool PreAI()
		{
			if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != mod.NPCType("Infernon"))
				npc.ai[0] = -1;

			if (npc.ai[0] == -1)
			{
				npc.alpha += 3;
				if (npc.alpha > 255)
					npc.active = false;
			}
			else if (npc.ai[0] == 0)
			{
				npc.ai[1]++;
				if (npc.ai[1] >= 60)
				{
					npc.ai[0] = 1;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
				}
			}
			else if (npc.ai[0] == 1)
			{
				if (npc.ai[1] == 0)
				{
					/* npc.TargetClosest(false);
                     Vector2 spinningpoint = Main.player[npc.target].Center - npc.Center;
                     spinningpoint.Normalize();
                     float dir = -1f;
                     if ((double)spinningpoint.X < 0.0)
                         dir = 1f;
                     Vector2 pos = Utils.RotatedBy(spinningpoint, -dir * 6.28318548202515 / 6.0, new Vector2());
                     Projectile.NewProjectile(npc.Center.X, npc.Center.Y, pos.X, pos.Y, mod.ProjectileType("InfernonSkull_Laser"), 5, 0.0f, Main.myPlayer, (dir * 6.28F / 540), npc.whoAmI);
                     npc.netUpdate = true;*/
					float spread = 45f * 0.0174f;
					double startAngle = Math.Atan2(1, 0) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					int i;
					for (i = 0; i < 4; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), mod.ProjectileType("InfernalWave"), 28, 0, Main.myPlayer);
						Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), mod.ProjectileType("InfernalWave"), 28, 0, Main.myPlayer);
						npc.netUpdate = true;
					}
					bool expertMode = Main.expertMode;
					{
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 33);
						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						direction.X *= 12f;
						direction.Y *= 12f;

						int amountOfProjectiles = 2;
						for (int z = 0; z < amountOfProjectiles; ++z)
						{
							float A = (float)Main.rand.Next(-200, 200) * 0.01f;
							float B = (float)Main.rand.Next(-200, 200) * 0.01f;
							int damage = expertMode ? 20 : 24;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("SunBlast"), damage, 1, Main.myPlayer, 0, 0);

						}
					}

				}

				npc.ai[1]++;
				if (npc.ai[1] >= 120)
				{
					npc.ai[0] = 2;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
				}
			}
			else if (npc.ai[0] == 2)
			{
				if (npc.ai[1] == 0)
				{
					npc.alpha += 3;
					if (npc.alpha > 255)
					{
						// Teleport.
						NPC target = Main.npc[(int)npc.ai[3]];
						Vector2 newPos = target.Center + new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201));
						npc.Center = newPos;

						npc.ai[1] = 1;
					}
				}
				else
				{
					npc.alpha -= 3;

					if (npc.alpha <= 0)
					{
						npc.ai[0] = 0;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
					}
				}
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (npc.ai[0] == 0)
				npc.frame.Y = 0;
			else if (npc.ai[0] == 1)
				npc.frame.Y = frameHeight;
			else if (npc.ai[0] == 2)
			{
				if (npc.alpha >= 0 && npc.alpha < 100)
					npc.frame.Y = 0;
				else if (npc.alpha >= 100 && npc.alpha < 175)
					npc.frame.Y = frameHeight * 2;
				else if (npc.alpha >= 175 && npc.alpha < 255)
					npc.frame.Y = frameHeight * 3;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[0] == 1)
			{
				Texture2D glowmask = mod.GetTexture("Effects/Glowmasks/InfernonSkull_Glowmask");
				Vector2 origin = new Vector2(glowmask.Width * 0.5F, glowmask.Height * 0.5F);
				spriteBatch.Draw(glowmask, (npc.Center - origin) - Main.screenPosition, Color.White);
			}
			return true;
		}
	}
}
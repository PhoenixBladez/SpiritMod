using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 80;

			npc.damage = 0;
			npc.lifeMax = 10;
			npc.alpha = 255;

			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.dontTakeDamage = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
		}

		public override bool PreAI()
		{
			if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != ModContent.NPCType<Infernon>())
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
					float spread = 45f * 0.0174f;
					double startAngle = Math.Atan2(1, 0) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					for (int i = 0; i < 4; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
						npc.netUpdate = true;
					}
					bool expertMode = Main.expertMode;
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 33);
					Vector2 direction = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 12f;

					int amountOfProjectiles = 2;
					for (int z = 0; z < amountOfProjectiles; ++z)
					{
						float A = Main.rand.Next(-200, 200) * 0.01f;
						float B = Main.rand.Next(-200, 200) * 0.01f;
						int damage = expertMode ? 20 : 24;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SunBlast>(), damage, 1, Main.myPlayer, 0, 0);
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
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Infernon/InfernonSkull_Glow"));
	}
}
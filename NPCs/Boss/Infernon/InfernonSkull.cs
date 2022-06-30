using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 100;
			NPC.height = 80;

			NPC.damage = 0;
			NPC.lifeMax = 10;
			NPC.alpha = 255;

			NPC.boss = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
		}

		public override bool PreAI()
		{
			if (!Main.npc[(int)NPC.ai[3]].active || Main.npc[(int)NPC.ai[3]].type != ModContent.NPCType<Infernon>())
				NPC.ai[0] = -1;

			if (NPC.ai[0] == -1)
			{
				NPC.alpha += 3;
				if (NPC.alpha > 255)
					NPC.active = false;
			}
			else if (NPC.ai[0] == 0)
			{
				NPC.ai[1]++;
				if (NPC.ai[1] >= 60)
				{
					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
				}
			}
			else if (NPC.ai[0] == 1)
			{
				if (NPC.ai[1] == 0)
				{
					float spread = 45f * 0.0174f;
					double startAngle = Math.Atan2(1, 0) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					for (int i = 0; i < 4; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
						NPC.netUpdate = true;
					}
					bool expertMode = Main.expertMode;
					SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 12f;

					int amountOfProjectiles = 2;
					for (int z = 0; z < amountOfProjectiles; ++z)
					{
						float A = Main.rand.Next(-200, 200) * 0.01f;
						float B = Main.rand.Next(-200, 200) * 0.01f;
						int damage = expertMode ? 20 : 24;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SunBlast>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}

				NPC.ai[1]++;
				if (NPC.ai[1] >= 120)
				{
					NPC.ai[0] = 2;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
				}
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.ai[1] == 0)
				{
					NPC.alpha += 3;
					if (NPC.alpha > 255)
					{
						// Teleport.
						NPC target = Main.npc[(int)NPC.ai[3]];
						Vector2 newPos = target.Center + new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201));
						NPC.Center = newPos;

						NPC.ai[1] = 1;
					}
				}
				else
				{
					NPC.alpha -= 3;

					if (NPC.alpha <= 0)
					{
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
					}
				}
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 0)
				NPC.frame.Y = 0;
			else if (NPC.ai[0] == 1)
				NPC.frame.Y = frameHeight;
			else if (NPC.ai[0] == 2)
			{
				if (NPC.alpha >= 0 && NPC.alpha < 100)
					NPC.frame.Y = 0;
				else if (NPC.alpha >= 100 && NPC.alpha < 175)
					NPC.frame.Y = frameHeight * 2;
				else if (NPC.alpha >= 175 && NPC.alpha < 255)
					NPC.frame.Y = frameHeight * 3;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/Infernon/InfernonSkull_Glow").Value);
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class AtlasArmRight : ModNPC
	{
		int collideTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas Arm");
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

		public override void SetDefaults()
		{
			NPC.width = 154;
			NPC.height = 324;
			NPC.damage = 100;
			NPC.lifeMax = 10000;
			NPC.boss = true;
			NPC.timeLeft = NPC.activeTime * 30;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			bool atlasAlive = false;
			int attachedTo = (int)NPC.ai[0];
			if (attachedTo > -1 && attachedTo < Main.maxNPCs && Main.npc[attachedTo].active && Main.npc[attachedTo].type == ModContent.NPCType<Atlas>())
				atlasAlive = true;
			if (!atlasAlive)
				NPC.ai[1] = 5;

			float num801 = NPC.position.X + (float)(NPC.width / 2) - Main.player[NPC.target].position.X - (float)(Main.player[NPC.target].width / 2);
			float num802 = NPC.position.Y + (float)NPC.height - 59f - Main.player[NPC.target].position.Y - (float)(Main.player[NPC.target].height / 2);
			float num803 = (float)Math.Atan2((double)num802, (double)num801) + 1.57f;
			if (num803 < 0f)
				num803 += 6.283f;
			else if ((double)num803 > 6.283)
				num803 -= 6.283f;

			if (NPC.ai[1] == 0f) {
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= (160f + 30f)) {
					NPC.alpha -= 4;
					if (NPC.alpha <= 0) {
						NPC.ai[1] = 1f;
						NPC.ai[2] = 0f;
						NPC.alpha = 0;
					}
				}
			}
			else if (NPC.ai[1] == 1f) {
				if (NPC.ai[2] == 0f) {
					NPC.velocity.Y = 16f;
					if (NPC.collideY) {
						SpiritMod.tremorTime = 20;
						NPC.ai[2] = 1f;
					}
				}
				else if (NPC.ai[2] == 1f) {
					NPC.velocity *= 0f;
					NPC.ai[1] = 2f;
				}
			}
			else if (NPC.ai[1] == 2f) {
				NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -10f, 10f);
				NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -10f, 10f);
				NPC.rotation = NPC.velocity.X / 20f;
				NPC.ai[3] += 2;
				if (NPC.ai[3] >= 120f) {
					NPC.ai[1] = 3f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.TargetClosest(true);
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[1] == 3f) {
				NPC.rotation = num803;
				float num383 = expertMode ? 25f : 20f;
				Vector2 vector37 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num384 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector37.X;
				float num385 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector37.Y;
				float num386 = (float)Math.Sqrt((double)(num384 * num384 + num385 * num385));
				num386 = num383 / num386;
				NPC.velocity.X = num384 * num386;
				NPC.velocity.Y = num385 * num386;
				NPC.ai[1] = 4f;
			}
			else if (NPC.ai[1] == 4f) {
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 25f) {
					NPC.velocity.X = NPC.velocity.X * 0.985f;
					NPC.velocity.Y = NPC.velocity.Y * 0.985f;
					if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
						NPC.velocity.X = 0f;
					if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
						NPC.velocity.Y = 0f;
				}
				else
					NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) - 1.57f;

				if (NPC.ai[2] >= 70f) {
					NPC.ai[3] += 1f;
					NPC.ai[2] = 0f;
					NPC.target = 255;
					NPC.rotation = num803;
					if (NPC.ai[3] >= 1f) {
						NPC.ai[1] = 2f;
						NPC.ai[3] = 0f;
					}
					else {
						NPC.ai[1] = 3f;
					}
				}
			}
			else if (NPC.ai[1] == 5f) {
				NPC.active = false;
			}

			collideTimer++;
			if (collideTimer == 400)
				NPC.noTileCollide = true;
			if (NPC.ai[1] <= 2f)
				NPC.direction = NPC.spriteDirection = 1;
			return false;
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = 10;
			NPC.damage = (int)(NPC.damage * 0.65f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.velocity != Vector2.Zero) {
                Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
                }
            }
            return true;
		}
	}
}
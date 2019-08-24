using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class AtlasArmLeft : ModNPC
	{
		int collideTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas Arm");
		}

		public override void SetDefaults()
		{
			npc.width = 120;
			npc.height = 300;
			npc.damage = 110;
			npc.lifeMax = 10;
			npc.boss = true;
			npc.timeLeft = NPC.activeTime * 30;
			npc.noGravity = true;
			npc.dontTakeDamage = true;
			npc.alpha = 255;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			bool atlasAlive = false;
			int attachedTo = (int)npc.ai[0];
			if (attachedTo > -1 && attachedTo < Main.maxNPCs && Main.npc[attachedTo].active && Main.npc[attachedTo].type == Atlas._type)
				atlasAlive = true;
			if (!atlasAlive)
				npc.ai[1] = 5;

			float num801 = npc.position.X + (float)(npc.width / 2) - Main.player[npc.target].position.X - (float)(Main.player[npc.target].width / 2);
			float num802 = npc.position.Y + (float)npc.height - 59f - Main.player[npc.target].position.Y - (float)(Main.player[npc.target].height / 2);
			float num803 = (float)Math.Atan2((double)num802, (double)num801) + 1.57f;
			if (num803 < 0f)
				num803 += 6.283f;
			else if ((double)num803 > 6.283)
				num803 -= 6.283f;

			if (npc.ai[1] == 0f)
			{
				npc.ai[2] += 1f;
				if (npc.ai[2] >= (160f - 30f))
				{
					npc.alpha -= 4;
					if (npc.alpha <= 0)
					{
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.alpha = 0;
					}
				}
			}
			else if (npc.ai[1] == 1f)
			{
				if (npc.ai[2] == 0f)
				{
					npc.velocity.Y = 16f;
					if (npc.collideY)
					{
						SpiritMod.shittyModTime = 60;
						npc.ai[2] = 1f;
					}
				}
				else if (npc.ai[2] == 1f)
				{
					npc.velocity *= 0f;
					npc.ai[1] = 2f;
				}
			}
			else if (npc.ai[1] == 2f)
			{
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -10f, 10f);
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -10f, 10f);
				npc.rotation = npc.velocity.X / 20f;
				npc.ai[3] += (float)Main.rand.Next(4);
				if (npc.ai[3] >= 300f)
				{
					npc.ai[1] = 3f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 3f)
			{
				npc.rotation = num803;
				float num383 = expertMode ? 25f : 20f;
				Vector2 vector37 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num384 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector37.X;
				float num385 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector37.Y;
				float num386 = (float)Math.Sqrt((double)(num384 * num384 + num385 * num385));
				num386 = num383 / num386;
				npc.velocity.X = num384 * num386;
				npc.velocity.Y = num385 * num386;
				npc.ai[1] = 4f;
			}
			else if (npc.ai[1] == 4f)
			{
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 25f)
				{
					npc.velocity.X = npc.velocity.X * 0.96f;
					npc.velocity.Y = npc.velocity.Y * 0.96f;
					if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
						npc.velocity.X = 0f;
					if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
						npc.velocity.Y = 0f;
				}
				else
					npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) - 1.57f;

				if (npc.ai[2] >= 70f)
				{
					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.rotation = num803;
					if (npc.ai[3] >= 1f)
					{
						npc.ai[1] = 2f;
						npc.ai[3] = 0f;
					}
					else
					{
						npc.ai[1] = 3f;
					}
				}
			}
			else if (npc.ai[1] == 5f)
			{
				npc.active = false;
			}

			collideTimer++;
			if (collideTimer == 400)
				npc.noTileCollide = true;
			if (npc.ai[1] <= 2f)
				npc.direction = npc.spriteDirection = 1;
			return false;
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = 10;
			npc.damage = (int)(npc.damage * 0.65f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.velocity != Vector2.Zero)
			{
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int i = 1; i < npc.oldPos.Length; ++i)
				{
					Vector2 vector2_2 = npc.oldPos[i];
					Microsoft.Xna.Framework.Color color2 = Color.White * npc.Opacity;
					color2.R = (byte)(0.5 * (double)color2.R * (double)(10 - i) / 20.0);
					color2.G = (byte)(0.5 * (double)color2.G * (double)(10 - i) / 20.0);
					color2.B = (byte)(0.5 * (double)color2.B * (double)(10 - i) / 20.0);
					color2.A = (byte)(0.5 * (double)color2.A * (double)(10 - i) / 20.0);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.oldPos[i].X - Main.screenPosition.X + (npc.width / 2),
						npc.oldPos[i].Y - Main.screenPosition.Y + npc.height / 2), new Rectangle?(npc.frame), color2, npc.oldRot[i], origin, npc.scale, SpriteEffects.None, 0.0f);
				}
			}
			return true;
		}
	}
}
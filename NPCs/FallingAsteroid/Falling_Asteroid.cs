using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.FallingAsteroid
{
	public class Falling_Asteroid : ModNPC
	{
		public int visualTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Falling Asteroid");
			NPCID.Sets.TrailCacheLength[npc.type] = 30; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 52;
			npc.defense = 6;
			npc.value = 350f;
			npc.knockBackResist = 0f;
			npc.width = 30;
			npc.height = 50;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.damage = 30;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath43;
		}
		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			target.AddBuff(24, 60*3);
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.spriteDirection = 1;
			
			npc.ai[0]++;
			if (npc.ai[0] <= 320 && npc.ai[0] >= 0)
				movement();
			else if (npc.ai[0] > 360)
				doExplosion();
			if (npc.ai[0] == 320 ||npc.ai[0] == 360) {
				npc.netUpdate = true;
			}
			Vector2 vector2 = npc.Center + Vector2.Normalize(npc.velocity) / 2f;
			Dust dust1 = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 6, 0.0f, 0.0f, 0, new Color(), 1f)];
			dust1.position = vector2;
			dust1.velocity = npc.velocity.RotatedBy(1.57079637050629, new Vector2()) * 0.33f + npc.velocity / 120f;
			dust1.position += npc.velocity.RotatedBy(1.57079637050629, new Vector2());
			dust1.fadeIn = 0.5f;
			dust1.noGravity = true;
			Dust dust2 = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 6, 0.0f, 0.0f, 0, new Color(), 1f)];
			dust2.position = vector2;
			dust2.velocity = npc.velocity.RotatedBy(-1.57079637050629, new Vector2()) * 0.33f + npc.velocity / 120f;
			dust2.position += npc.velocity.RotatedBy(-1.57079637050629, new Vector2());
			dust2.fadeIn = 0.5f;
			dust2.noGravity = true;
			
			if (player.dead)
				npc.velocity.Y -= 0.15f;
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.5f, 0.25f, 0f);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Meteor.Chance * 0.15f;
		}
		public void doExplosion()
		{
			Player player = Main.player[npc.target];

			npc.velocity.Y += 0.15f;
			npc.noTileCollide = false;
			if (npc.collideY && npc.ai[0] > 420)
			{
				for (int index = 0; index < 30; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 50), npc.width, npc.height, 240, 0.0f, 0.0f, 0, new Color(), 1.2f)];
					dust.velocity.Y -= (float)(3.0 + (double)2 * 2.5);
					dust.velocity.Y *= Main.rand.NextFloat();
					dust.scale += (float)8 * 0.03f;
				}
				Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 14, 1f, 0.0f);
				npc.ai[0] = -90;
				npc.netUpdate = true;
				for (int k = 0; k < 10; k++)
				{
					Gore.NewGore(npc.position, new Vector2(npc.velocity.X * 0.5f, -npc.velocity.Y * 0.5f), Main.rand.Next(61, 64), 1f);
				}
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 6, npc.velocity.X * 2f, -npc.velocity.Y * 2f, 150, new Color(), 1.2f);
				}
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc2 = Main.npc[i];
					if ((double)Vector2.Distance(npc.Center, npc2.Center) <= (double)150f && !npc2.boss && npc2.knockBackResist != 0f)
					{
						Vector2 vector2 = new Vector2(npc2.position.X + (float)npc2.width * 0.5f, npc2.position.Y + (float)npc2.height * 0.5f);
						float num2 = npc.position.X + Main.rand.Next(-10, 10) + (float)(npc.width / 2) - vector2.X;
						float num3 = npc.position.Y + Main.rand.Next(-10, 10) + (float)(npc.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
						npc2.velocity.X = num2 * -1 * num4 * (1.7f);
						npc2.velocity.Y = num3 * num4 * -1 * (1.7f);
					}
				}
				for (int i = 0; i < Main.item.Length; i++)
				{
					Item item = Main.item[i];
					if ((double)Vector2.Distance(npc.Center, item.Center) <= (double)150f)
					{
						Vector2 vector2 = new Vector2(item.position.X + (float)item.width * 0.5f, item.position.Y + (float)item.height * 0.5f);
						float num2 = npc.position.X + Main.rand.Next(-10, 10) + (float)(npc.width / 2) - vector2.X;
						float num3 = npc.position.Y + Main.rand.Next(-10, 10) + (float)(npc.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
						item.velocity.X = num2 * -1 * num4 * (1.7f);
						item.velocity.Y = num3 * -1 * num4 * (1.7f);
					}
				}
				for (int i = 0; i < Main.player.Length; i++)
				{
					Player patates = Main.player[i];
					if ((double)Vector2.Distance(npc.Center, patates.Center) <= (double)150f)
					{
						Vector2 vector2 = new Vector2(patates.position.X + (float)patates.width * 0.5f, patates.position.Y + (float)patates.height * 0.5f);
						float num2 = npc.position.X + Main.rand.Next(-10, 10) + (float)(npc.width / 2) - vector2.X;
						float num3 = npc.position.Y + Main.rand.Next(-10, 10) + (float)(npc.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
						patates.velocity.X = num2 * num4 * -1 * (1.7f);
						patates.velocity.Y = num3 * num4 * -1 * (1.7f);
					}
				}
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile aga = Main.projectile[i];
					if ((double)Vector2.Distance(npc.Center, aga.Center) <= (double)150f)
					{
						Vector2 vector2 = new Vector2(aga.position.X + (float)aga.width * 0.5f, aga.position.Y + (float)aga.height * 0.5f);
						float num2 = npc.position.X + Main.rand.Next(-10, 10) + (float)(npc.width / 2) - vector2.X;
						float num3 = npc.position.Y + Main.rand.Next(-10, 10) + (float)(npc.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
						aga.velocity.X = num2 * -1 * num4 * (1.7f);
						aga.velocity.Y = num3 * num4 * -1 * (1.7f);
					}
				}
			}
		}
		public void movement()
		{
			Player player = Main.player[npc.target];
			
			npc.noTileCollide = true;
			float num1 = 4f;
			float number2 = 0.35f;
			Vector2 vektor2 = new Vector2(npc.Center.X, npc.Center.Y);
			float number3 = Main.player[npc.target].Center.X - vektor2.X;
			float number4 = (float)((double)Main.player[npc.target].Center.Y - (double)vektor2.Y - 300.0);
			float num5 = (float)Math.Sqrt((double)number3 * (double)number3 + (double)number4 * (double)number4);
			float num6;
			float num7;
			if (player == Main.player[npc.target])
			{
				if ((double)num5 < 20.0)
				{
					num6 = npc.velocity.X;
					num7 = npc.velocity.Y;
				}
				else
				{
					float num8 = num1 / num5;
					num6 = number3 * num8;
					num7 = number4 * num8;
				}
				if ((double)npc.velocity.X < (double)num6)
				{
					npc.velocity.X += number2;
					if ((double)npc.velocity.X < 0.0 && (double)num6 > 0.0)
					{
						npc.velocity.X += number2 * 2f;
					}
				}
				else if ((double)npc.velocity.X > (double)num6)
				{
					npc.velocity.X -= number2;
					if ((double)npc.velocity.X > 0.0 && (double)num6 < 0.0)
					{
						npc.velocity.X -= number2 * 2f;
					}
				}
				if ((double)npc.velocity.Y < (double)num7)
				{
					npc.velocity.Y += number2;
					if ((double)npc.velocity.Y < 0.0 && (double)num7 > 0.0)
					{
						npc.velocity.Y += number2 * 2f;
					}
				}
				else if ((double)npc.velocity.Y > (double)num7)
				{
					npc.velocity.Y -= number2;
					if ((double)npc.velocity.Y > 0.0 && (double)num7 < 0.0)
					{
						npc.velocity.Y -= number2 * 2f;
					}
				}
			}
			
			++npc.ai[1];
			if ((double) npc.ai[1] >= (double) 5f)
			{
				npc.ai[1] = 0.0f;
				npc.netUpdate = true;
          }
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;
			const int Frame_5 = 4;

			Player player = Main.player[npc.target];

			if (npc.active && player.active)
			{
				npc.frameCounter++;
				if (npc.frameCounter < 6)
				{
					npc.frame.Y = Frame_1 * frameHeight;
				}
				else if (npc.frameCounter < 12)
				{
					npc.frame.Y = Frame_2 * frameHeight;
				}
				else if (npc.frameCounter < 18)
				{
					npc.frame.Y = Frame_3 * frameHeight;
				}
				else if (npc.frameCounter < 24)
				{
					npc.frame.Y = Frame_4 * frameHeight;
				}
				else if (npc.frameCounter < 28)
				{
					npc.frame.Y = Frame_5 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FallenAsteroid/FallenAsteroidGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FallenAsteroid/FallenAsteroidGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FallenAsteroid/FallenAsteroidGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FallenAsteroid/FallenAsteroidGore4"), 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 116, Main.rand.Next(2, 5));
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[0] > 360)
			{
				/*for (int index = 0; index < 5; ++index)
				{
					int num20 = 0;
					int num19 = 0;
					SpriteEffects effects1 = SpriteEffects.None;
					if (npc.spriteDirection == -1)
						effects1 = SpriteEffects.FlipHorizontally;
					float x5 = (float) ((double) (Main.npcTexture[npc.type].Width - npc.width) * 0.5 + (double) npc.width * 0.5);
					Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
					Microsoft.Xna.Framework.Color alpha = npc.GetAlpha(color3);
					float num1 = (float) (9 - index) / 9f;
					alpha.R = (byte) ((double) alpha.R * (double) num1);
					alpha.G = (byte) ((double) alpha.G * (double) num1);
					alpha.B = (byte) ((double) alpha.B * (double) num1);
					alpha.A = (byte) ((double) alpha.A * (double) num1);
					float num2 = (float) (9 - index) / 9f;
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.oldPos[index].X - Main.screenPosition.X + x5 + (float) num20, npc.oldPos[index].Y - Main.screenPosition.Y + (float) (npc.height / 2) + npc.gfxOffY), npc.frame, alpha, npc.rotation, new Vector2(x5, (float) (npc.height / 2 + num19)), num2 * npc.scale, effects1, 0.0f);
				}*/
				Player player = Main.player[npc.target];
				++visualTimer;
				bool flag2 = (double) Vector2.Distance(npc.Center, player.Center) > (double) 0f && (double) npc.Center.Y == (double) player.Center.Y;
				if ((double) visualTimer >= (double) 30f && flag2)
				{
					visualTimer = 0;
				}
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addY = 0.0f;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
				Texture2D texture2D = Main.extraTexture[55];
				if (npc.velocity.X == 0)
				{
					addHeight = 0f;
					addWidth = 0f;
					texture2D = Main.extraTexture[55];
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) visualTimer / 2;
				float num2 = -1.570796f * (float) npc.rotation;
				float amount = visualTimer / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = npc.oldPos[index];
					Microsoft.Xna.Framework.Color color2 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Gold, Microsoft.Xna.Framework.Color.OrangeRed, amount);
					color2 = Microsoft.Xna.Framework.Color.Lerp(color2, Microsoft.Xna.Framework.Color.Blue, (float) index / 12f);
					color2.A = (byte) (64.0 * (double) amount);
					color2.R = (byte) ((int) color2.R * (10 - index) / 20);
					color2.G = (byte) ((int) color2.G * (10 - index) / 20);
					color2.B = (byte) ((int) color2.B * (10 - index) / 20);
					color2.A = (byte) ((int) color2.A * (10 - index) / 20);
					color2 *= amount;
					int frameY = (num3 - index) % 4;
					if (frameY < 0)
						frameY += 4;
					Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 4, 0, frameY);
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) npc.oldPos[index].X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale) + addWidth, (float) ((double) npc.oldPos[index].Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, (float) ((10.0 - (double) index) / 15.0)), spriteEffects, 0.0f);
				}
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/FallingAsteroid/Falling_Asteroid_Glow"));
		}
	}
}
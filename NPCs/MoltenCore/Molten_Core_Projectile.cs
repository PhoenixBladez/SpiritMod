using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoltenCore
{
	public class Molten_Core_Projectile : ModProjectile
	{
		protected int killtimer = 0;
		protected int visualTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Lava");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 0;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.hostile = true;
		}
		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			target.AddBuff(24, 60*3);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			killtimer++;
			projectile.velocity.X = 0f;
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0.0f, 0.0f, 0, new Color(), 2f);
			}
			Main.PlaySound(42, (int)projectile.position.X, (int)projectile.position.Y, 6, 1f, 0f);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!projectile.wet)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addY = 0.0f;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (Main.projectileTexture[projectile.type].Width / 2), (float) (Main.projectileTexture[projectile.type].Height / 1 / 2));
				Texture2D texture2D = Main.extraTexture[55];
				if (projectile.velocity.X == 0)
				{
					addHeight = -8f;
					addWidth = -6f;
					texture2D = ModContent.GetTexture("SpiritMod/Textures/Flames");
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) visualTimer / 2;
				float num2 = -1.570796f * (float) projectile.rotation;
				float amount = visualTimer / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = projectile.oldPos[index];
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
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) projectile.oldPos[index].X - (double) Main.screenPosition.X + (double) (projectile.width / 2) - (double) Main.projectileTexture[projectile.type].Width * (double) projectile.scale / 2.0 + (double) vector2_3.X * (double) projectile.scale) + addWidth, (float) ((double) projectile.oldPos[index].Y - (double) Main.screenPosition.Y + (double) projectile.height - (double) Main.projectileTexture[projectile.type].Height * (double) projectile.scale / (double) 1 + 4.0 + (double) vector2_3.Y * (double) projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, (float) ((10.0 - (double) index) / 40.0)), spriteEffects, 0.0f);
				}
			}
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			
			if (projectile.wet)
			{
				projectile.hostile = false;
			}
			
			if (projectile.velocity.X < 0f)
				projectile.spriteDirection = -1;
			else
				projectile.spriteDirection = 1;
			++visualTimer;
			bool flag2 = (double) Vector2.Distance(projectile.Center, player.Center) > (double) 0f && (double) projectile.Center.Y == (double) player.Center.Y;
			if ((double) visualTimer >= (double) 30f && flag2)
			{
				visualTimer = 0;
			}		
			if (killtimer >= 270)
				projectile.Kill();
			if (Main.npc[(int)projectile.ai[1]].life <= 0 || !Main.npc[(int)projectile.ai[1]].active)
			{
				projectile.ai[1] = 0;
				projectile.aiStyle = 1;
				projectile.tileCollide = true;
				projectile.hostile = true;
				if (Main.rand.Next(3) == 0 && !projectile.wet)
				{
					int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0.0f, 0.0f, 100, new Color(), 3f);
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity.X = 0f;
					Main.dust[index].velocity.Y = -2f;
				}
			}
			else
			{
				
				if (Main.rand.Next(30) == 0 && !projectile.wet)
				{
					int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0.0f, 0.0f, 100, new Color(), 1f);
					Main.dust[index].noGravity = false;
					Main.dust[index].velocity.X = 0f;
					Main.dust[index].velocity.Y = 2f;
				}

				float num5 = 13f;
				if ((double)projectile.ai[0] == 1.0)
				{
					num5 = 13f;
				}

				Vector2 vector2_1 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num6 = (Main.npc[(int)projectile.ai[1]].Center.X - vector2_1.X) - Main.rand.Next(-200, 200);
				float num7 = (float)((double)Main.npc[(int)projectile.ai[1]].Center.Y - (double)vector2_1.Y) - Main.rand.Next(-60, 60);
				float num8 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num7 * (double)num7);
				if ((double)num8 < 100.0 && (double)projectile.ai[0] == 1.0 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0.0f;
				}

				if ((double)num8 > 2000.0)
				{
					projectile.position.X = Main.npc[(int)projectile.ai[1]].Center.X - (float)(projectile.width / 2);
					projectile.position.Y = Main.npc[(int)projectile.ai[1]].Center.Y - (float)(projectile.width / 2);
				}
				if ((double)num8 > 20.0)
				{
					float num9 = num5 / num8;
					float num10 = num6 * num9;
					float num11 = num7 * num9;
					projectile.velocity.X = (float)(((double)projectile.velocity.X * 7.0 + (double)num10) / 8.0);
					projectile.velocity.Y = (float)(((double)projectile.velocity.Y * 7.0 + (double)num11) / 8.0);
				}
				else
				{
					if ((double)projectile.velocity.X == 0.0 && (double)projectile.velocity.Y == 0.0)
					{
						projectile.velocity.X = -0.5f;
						projectile.velocity.Y = -0.5f;
					}
					Vector2 vector2_2 = projectile.velocity * 0.99f;
					projectile.velocity = vector2_2;
				}
			}
		}
	}
}
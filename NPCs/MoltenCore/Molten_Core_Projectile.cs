using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
		}

		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			target.AddBuff(24, 60*3);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			killtimer++;
			Projectile.velocity.X = 0f;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 2f);
			}
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
		}

		public override void PostDraw(Color lightColor)
		{
			if (!Projectile.wet)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				float addHeight = -4f;
				float addWidth = 0f;
				Vector2 vector2_3 = new Vector2((float) (TextureAssets.Projectile[Projectile.type].Value.Width / 2), (float) (TextureAssets.Projectile[Projectile.type].Value.Height / 1 / 2));
				Texture2D texture2D = TextureAssets.Extra[55].Value;
				if (Projectile.velocity.X == 0)
				{
					addHeight = -8f;
					addWidth = -6f;
					texture2D = ModContent.Request<Texture2D>("SpiritMod/Textures/Flames", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				}
				Vector2 origin = new Vector2((float) (texture2D.Width / 2), (float) (texture2D.Height / 8 + 14));
				int num1 = (int) visualTimer / 2;
				float num2 = -1.570796f * (float) Projectile.rotation;
				float amount = visualTimer / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = Projectile.oldPos[index];
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
					Main.spriteBatch.Draw(texture2D, new Vector2((float) ((double) Projectile.oldPos[index].X - (double) Main.screenPosition.X + (double) (Projectile.width / 2) - (double) TextureAssets.Projectile[Projectile.type].Value.Width * (double) Projectile.scale / 2.0 + (double) vector2_3.X * (double) Projectile.scale) + addWidth, (float) ((double) Projectile.oldPos[index].Y - (double) Main.screenPosition.Y + (double) Projectile.height - (double) TextureAssets.Projectile[Projectile.type].Value.Height * (double) Projectile.scale / (double) 1 + 4.0 + (double) vector2_3.Y * (double) Projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, (float) ((10.0 - (double) index) / 40.0)), spriteEffects, 0.0f);
				}
			}
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			
			if (Projectile.wet)
			{
				Projectile.hostile = false;
			}
			
			if (Projectile.velocity.X < 0f)
				Projectile.spriteDirection = -1;
			else
				Projectile.spriteDirection = 1;
			++visualTimer;
			bool flag2 = (double) Vector2.Distance(Projectile.Center, player.Center) > (double) 0f && (double) Projectile.Center.Y == (double) player.Center.Y;
			if ((double) visualTimer >= (double) 30f && flag2)
			{
				visualTimer = 0;
			}		
			if (killtimer >= 270)
				Projectile.Kill();
			if (Main.npc[(int)Projectile.ai[1]].life <= 0 || !Main.npc[(int)Projectile.ai[1]].active || Projectile.aiStyle == 1)
			{
				Projectile.aiStyle = 1;
				Projectile.tileCollide = true;
				Projectile.hostile = true;
				if (Main.rand.NextBool(3) && !Projectile.wet)
				{
					int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 3f);
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity.X = 0f;
					Main.dust[index].velocity.Y = -2f;
				}
				return;
			}
			else
			{
				
				if (Main.rand.NextBool(30) && !Projectile.wet)
				{
					int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 1f);
					Main.dust[index].noGravity = false;
					Main.dust[index].velocity.X = 0f;
					Main.dust[index].velocity.Y = 2f;
				}

				float num5 = 13f;
				if ((double)Projectile.ai[0] == 1.0)
					num5 = 13f;

				Vector2 vector2_1 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num6 = (Main.npc[(int)Projectile.ai[1]].Center.X - vector2_1.X) - Main.rand.Next(-200, 200);
				float num7 = (float)((double)Main.npc[(int)Projectile.ai[1]].Center.Y - (double)vector2_1.Y) - Main.rand.Next(-60, 60);
				float num8 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num7 * (double)num7);
				if ((double)num8 < 100.0 && (double)Projectile.ai[0] == 1.0 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0.0f;
				}

				if ((double)num8 > 2000.0)
				{
					Projectile.position.X = Main.npc[(int)Projectile.ai[1]].Center.X - (float)(Projectile.width / 2);
					Projectile.position.Y = Main.npc[(int)Projectile.ai[1]].Center.Y - (float)(Projectile.width / 2);
				}
				if ((double)num8 > 20.0)
				{
					float num9 = num5 / num8;
					float num10 = num6 * num9;
					float num11 = num7 * num9;
					Projectile.velocity.X = (float)(((double)Projectile.velocity.X * 7.0 + (double)num10) / 8.0);
					Projectile.velocity.Y = (float)(((double)Projectile.velocity.Y * 7.0 + (double)num11) / 8.0);
				}
				else
				{
					if ((double)Projectile.velocity.X == 0.0 && (double)Projectile.velocity.Y == 0.0)
					{
						Projectile.velocity.X = -0.5f;
						Projectile.velocity.Y = -0.5f;
					}
					Vector2 vector2_2 = Projectile.velocity * 0.99f;
					Projectile.velocity = vector2_2;
				}
			}
		}
	}
}
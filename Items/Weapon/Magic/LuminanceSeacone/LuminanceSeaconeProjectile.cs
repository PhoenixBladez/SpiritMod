using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.LuminanceSeacone
{
	public class LuminanceSeaconeProjectile : ModProjectile
	{
		public bool hasCreatedSound = false;
		public bool hasGottenColor = false;
		public bool wetCheck = false;
		public int r = 0;
		public int g = 0;
		public int b = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminance Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.hide = false;
			projectile.magic = true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			float addHeight = -4f;
			float addWidth = 0f;
			Vector2 vector2_3 = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 1 / 2));
			Texture2D texture2D = ModContent.GetTexture("SpiritMod/Textures/Circle_Outline");
			if (projectile.velocity.X == 0)
			{
				addHeight = -8f;
				addWidth = -6f;
				texture2D = ModContent.GetTexture("SpiritMod/Textures/Circle_Outline");
			}
			Vector2 origin = new Vector2((float)(texture2D.Width / 2), (float)(texture2D.Height / 8 + 14));
			int num1 = (int)projectile.ai[1] / 2;
			float num2 = -1.570796f * (float)projectile.rotation;
			float amount = projectile.ai[1] / 45f;
			if ((double)amount > 1.0)
				amount = 1f;
			int num3 = num1 % 4;
			for (int index = 10; index >= 0; --index)
			{
				Vector2 oldPo = projectile.oldPos[index];
				Microsoft.Xna.Framework.Color color2 = Microsoft.Xna.Framework.Color.Lerp(new Color(b, r, g), new Color(r, g, b), amount);
				color2 = Microsoft.Xna.Framework.Color.Lerp(color2, new Color(g, b, r), (float)index / 12f);
				color2.A = (byte)(64.0 * (double)amount);
				color2.R = (byte)((int)color2.R * (10 - index) / 20);
				color2.G = (byte)((int)color2.G * (10 - index) / 20);
				color2.B = (byte)((int)color2.B * (10 - index) / 20);
				color2.A = (byte)((int)color2.A * (10 - index) / 20);
				color2 *= amount;
				int frameY = (num3 - index) % 4;
				if (frameY < 0)
					frameY += 4;
				Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 4, 0, frameY);
				Main.spriteBatch.Draw(texture2D, new Vector2((float)((double)projectile.oldPos[index].X - (double)Main.screenPosition.X + (double)(projectile.width / 2) - (double)Main.projectileTexture[projectile.type].Width * (double)projectile.scale / 2.0 + (double)vector2_3.X * (double)projectile.scale) + addWidth, (float)((double)projectile.oldPos[index].Y - (double)Main.screenPosition.Y + (double)projectile.height - (double)Main.projectileTexture[projectile.type].Height * (double)projectile.scale / (double)1 + 4.0 + (double)vector2_3.Y * (double)projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.05f, 1.4f, (float)((10.0 - (double)index) / -20.0)), spriteEffects, 0.0f);
			}
			for (int index = 10; index >= 0; --index)
			{
				Vector2 oldPo = projectile.oldPos[index];
				Microsoft.Xna.Framework.Color color2 = Microsoft.Xna.Framework.Color.Lerp(new Color(r, g, b), new Color(b, r, g), amount);
				color2 = Microsoft.Xna.Framework.Color.Lerp(color2, new Color(b, g, r), (float)index / 12f);
				color2.A = (byte)(64.0 * (double)amount);
				color2.R = (byte)((int)color2.R * (10 - index) / 20);
				color2.G = (byte)((int)color2.G * (10 - index) / 20);
				color2.B = (byte)((int)color2.B * (10 - index) / 20);
				color2.A = (byte)((int)color2.A * (10 - index) / 20);
				color2 *= amount;
				int frameY = (num3 - index) % 4;
				if (frameY < 0)
					frameY += 4;
				Microsoft.Xna.Framework.Rectangle rectangle = texture2D.Frame(1, 4, 0, frameY);
				Main.spriteBatch.Draw(texture2D, new Vector2((float)((double)projectile.oldPos[index].X - (double)Main.screenPosition.X + (double)(projectile.width / 2) - (double)Main.projectileTexture[projectile.type].Width * (double)projectile.scale / 2.0 + (double)vector2_3.X * (double)projectile.scale) + addWidth, (float)((double)projectile.oldPos[index].Y - (double)Main.screenPosition.Y + (double)projectile.height - (double)Main.projectileTexture[projectile.type].Height * (double)projectile.scale / (double)1 + 4.0 + (double)vector2_3.Y * (double)projectile.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, num2, origin, MathHelper.Lerp(0.05f, 1.2f, (float)((10.0 - (double)index) / 30.0)), spriteEffects, 0.0f);
			}
		}
		public override void Kill(int timeLeft)
		{
			Vector2 spinningpoint = new Vector2(0.0f, -3f).RotatedByRandom(3.14159274101257);
			float num1 = (float)28 * projectile.scale;
			Vector2 vector2 = new Vector2(1.1f, 1f);
			for (float num2 = 0.0f; (double)num2 < (double)num1; ++num2)
			{
				int dustIndex = Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0.0f, 0.0f, 0, new Color(r, g, b), .6f);
				Main.dust[dustIndex].position = projectile.Center;
				Main.dust[dustIndex].velocity = spinningpoint.RotatedBy(6.28318548202515 * (double)num2 / (double)num1, new Vector2()) * vector2 * (float)(0.800000011920929 + (double)Main.rand.NextFloat() * 0.400000005960464);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = 2f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				Dust dust = Dust.CloneDust(dustIndex);
				dust.scale /= 2f;
				dust.fadeIn /= 2f;
				dust.color = new Color((int)r, (int)g, (int)b, (int)byte.MaxValue);
			}
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 21, 1f, 0f);
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), r * 0.0005f, g * 0.0005f, b * 0.0005f);
			Player player = Main.player[projectile.owner];
			if (projectile.velocity.X < 0f)
				projectile.spriteDirection = -1;
			else
				projectile.spriteDirection = 1;
			++projectile.ai[1];
			bool flag2 = (double)Vector2.Distance(projectile.Center, player.Center) > (double)0f && (double)projectile.Center.Y == (double)player.Center.Y;
			if ((double)projectile.ai[1] >= (double)30f && flag2)
			{
				projectile.ai[1] = 0.0f;
			}

			if (projectile.timeLeft <= 260)
				projectile.velocity.Y += 0.8f;


			if (!hasCreatedSound)
			{
				hasCreatedSound = true;
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 21, 1f, 0f);
			}

			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(1, 255);
				g = Main.rand.Next(1, 255);
				b = Main.rand.Next(1, 255);
				for (int index = 0; index < 8; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0.0f, 0.0f, 0, new Color(r, g, b), .6f)];
					dust.velocity = (Main.rand.NextFloatDirection() * 3.141593f).ToRotationVector2() * 2f + projectile.velocity.SafeNormalize(Vector2.Zero) * 3f;
					dust.noGravity = true;
					dust.scale = .85f;
					dust.fadeIn = 1.2f;
					dust.position = projectile.Center;
					dust.noLight = false;
				}
			}

			if (projectile.wet)
				wetCheck = true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if (wetCheck) { damage = Main.rand.Next(7, 13) + player.HeldItem.damage; }
		}
	}
}
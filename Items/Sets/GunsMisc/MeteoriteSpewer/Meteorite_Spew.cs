using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.MeteoriteSpewer
{
	public class Meteorite_Spew : ModProjectile
	{
		public bool hasCreatedSound = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteorite Spew");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 1;
			projectile.scale = 1f;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
			projectile.hide = false;
			projectile.ranged = true;
		}
		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 60*3);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity.X = 0f;
			if (Main.rand.Next(3) == 0 && !projectile.wet)
			{
				int index = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0.0f, 0.0f, 100, new Color(), 3f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity.X = 0f;
				Main.dust[index].velocity.Y = -2f;
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 0);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!projectile.wet)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
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
				int num1 = (int) projectile.ai[1] / 2;
				float num2 = -1.570796f * (float) projectile.rotation;
				float amount = projectile.ai[1] / 45f;
				if ((double) amount > 1.0)
					amount = 1f;
				int num3 = num1 % 4;
				for (int index = 5; index >= 0; --index)
				{
					Vector2 oldPo = projectile.oldPos[index];
					var color2 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.Gold, Microsoft.Xna.Framework.Color.OrangeRed, amount);
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
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for (int index = 0; index < 5; ++index)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0.0f, 0.0f, 0, new Color(), 2f);
			}
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 6, 1f, 0f);
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (projectile.velocity.X < 0f)
				projectile.spriteDirection = -1;
			else
				projectile.spriteDirection = 1;
			++projectile.ai[1];
			bool flag2 = (double) Vector2.Distance(projectile.Center, player.Center) > (double) 0f && (double) projectile.Center.Y == (double) player.Center.Y;
			if ((double) projectile.ai[1] >= (double) 30f && flag2)
			{
				projectile.ai[1] = 0.0f;
			}
			
			
			if (!hasCreatedSound)
			{
				hasCreatedSound = true;
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 34, 1f, 0f);
			}
			
			if (projectile.wet)
			{
				projectile.friendly = false;
			}
		}
	}
}
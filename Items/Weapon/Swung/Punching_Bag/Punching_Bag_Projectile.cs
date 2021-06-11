using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Punching_Bag
{
	public class Punching_Bag_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie's Punch");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 60;
			projectile.alpha = 100;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects1 = SpriteEffects.None;
			if (projectile.direction == 1)
				effects1 = SpriteEffects.FlipHorizontally;
			Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int) ((double) projectile.position.X + (double) projectile.width * 0.5) / 16, (int) (((double) projectile.position.Y + (double) projectile.height * 0.5) / 16.0));
			{
				Texture2D texture = Main.projectileTexture[projectile.type];
				Texture2D glow = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, height * projectile.frame, texture.Width, height);
				Vector2 origin = r.Size() / 2f;
				int num2 = 10;
				int num3 = 1;
				int num4 = 1;
				float num5 = 1f;
				float num6 = 0.0f;
				num3 = 1;
				num5 = 2f;
				int index1 = num4;
				while (num3 > 0 && index1 < num2 || num3 < 0 && index1 > num2)
				{
				  Microsoft.Xna.Framework.Color newColor = color3;
				  newColor = Microsoft.Xna.Framework.Color.Lerp(newColor, Microsoft.Xna.Framework.Color.White, 2.5f);
				  Microsoft.Xna.Framework.Color color1 = projectile.GetAlpha(newColor);
				  float num7 = (float) (num2 - index1);
				  if (num3 < 0)
					num7 = (float) (num4 - index1);
				  Microsoft.Xna.Framework.Color color2 = color1 * (num7 / ((float) ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f));
				  Vector2 oldPo = projectile.oldPos[index1];
				  float rotation = projectile.rotation;
				  SpriteEffects effects2 = effects1;
				  if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				  {
					rotation = projectile.oldRot[index1];
					effects2 = projectile.oldSpriteDirection[index1] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				  }
				  Main.spriteBatch.Draw(glow, oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), color2, rotation + projectile.rotation * num6 * (float) (index1 - 1) * (float) -effects1.HasFlag((Enum) SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale, num5, (float) index1 / 15f), effects2, 0.0f);
		label_709:
				  index1 += num3;
				}

				Microsoft.Xna.Framework.Color color4 = projectile.GetAlpha(color3);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), new Color(255-projectile.alpha, 255-projectile.alpha, 255-projectile.alpha, 175), projectile.rotation, origin, projectile.scale, effects1, 0.0f);
			}
			return false;
		}
		public override void AI()
		{
			projectile.alpha += 10;
			projectile.velocity*=0.97f;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 178, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, 178, projectile.oldVelocity.X, projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int index1 = 4; index1 < 31; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (10f / (float) index1);
				float num2 = projectile.oldVelocity.Y * (10f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, 178, projectile.oldVelocity.X, projectile.oldVelocity.Y, 200, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.8f;
			}
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 178, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
	}
}

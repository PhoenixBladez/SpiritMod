using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class WizardBall_Projectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Converted Moonjelly");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 120;
			projectile.alpha = 100;
		}
		public override void AI()
		{
			int index = Dust.NewDust(projectile.Center, 0, 0, 180, 0.0f, 0.0f, 100, new Color(), 1f);
			Main.dust[index].noLight = false;
			Main.dust[index].noGravity = true;
			Main.dust[index].velocity = projectile.velocity;
			Main.dust[index].position -= Vector2.One * 3f;
			Main.dust[index].scale = 1.5f;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects1 = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
				effects1 = SpriteEffects.FlipHorizontally;
			Microsoft.Xna.Framework.Color color3 = Lighting.GetColor((int) ((double) projectile.position.X + (double) projectile.width * 0.5) / 16, (int) (((double) projectile.position.Y + (double) projectile.height * 0.5) / 16.0));
			{
				Texture2D texture = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, height * projectile.frame, texture.Width, height);
				Vector2 origin = r.Size() / 2f;
				int num1;
				int num2 = 7;
				int num3 = 1;
				int num4 = 1;
				float num5 = 1f;
				float num6 = 0.0f;
				num3 = 1;
				num5 = 0.5f;
				int index1 = num4;
				while (num3 > 0 && index1 < num2 || num3 < 0 && index1 > num2)
				{
				  Microsoft.Xna.Framework.Color newColor = color3;
				  newColor = Microsoft.Xna.Framework.Color.Lerp(newColor, Microsoft.Xna.Framework.Color.Cyan, 1.5f);
				  Microsoft.Xna.Framework.Color color1 = projectile.GetAlpha(newColor);
				  float num7 = (float) (num2 - index1);
				  if (num3 < 0)
					num7 = (float) (num4 - index1);
				  Microsoft.Xna.Framework.Color color2 = color1 * (num7 / ((float) ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.25f));
				  Vector2 oldPo = projectile.oldPos[index1];
				  float rotation = projectile.rotation;
				  SpriteEffects effects2 = effects1;
				  if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
				  {
					rotation = projectile.oldRot[index1];
					effects2 = projectile.oldSpriteDirection[index1] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				  }
				  Main.spriteBatch.Draw(texture, oldPo + projectile.Size / 2f - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), color2, rotation + projectile.rotation * num6 * (float) (index1 - 1) * (float) -effects1.HasFlag((Enum) SpriteEffects.FlipHorizontally).ToDirectionInt(), origin, MathHelper.Lerp(projectile.scale*1.5f, num5, (float) index1 / 15f), effects2, 0.0f);
		label_709:
				  index1 += num3;
				}

				Microsoft.Xna.Framework.Color color4 = projectile.GetAlpha(color3);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(r), color4, projectile.rotation, origin, projectile.scale, effects1, 0.0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 100);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}
		
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			int num2 = Main.rand.Next(5, 10);
			for (int index1 = 0; index1 < num2; ++index1)
			{
				int index2 = Dust.NewDust(projectile.Center, 0, 0, 180, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Main.dust[index2].velocity *= 1.2f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].velocity += projectile.velocity;
				Main.dust[index2].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int index1 = 4; index1 < 14; ++index1)
			{
				float num1 = projectile.oldVelocity.X * (30f / (float) index1);
				float num2 = projectile.oldVelocity.Y * (30f / (float) index1);
				int index2 = Dust.NewDust(new Vector2(projectile.oldPosition.X - num1, projectile.oldPosition.Y - num2), 8, 8, 180, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, new Color(), 1.8f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
			}
		}
	}
}

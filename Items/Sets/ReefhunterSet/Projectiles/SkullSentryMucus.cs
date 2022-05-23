using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentryMucus : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.SentryShot[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.timeLeft = 360;
			projectile.aiStyle = 0;
			projectile.scale = Main.rand.NextFloat(0.7f, 0.8f);
			projectile.alpha = 140;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();

			MakeDust(2, 0.5f, 0.8f, 200, 4);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);

			int trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];
			for (int i = trailLength - 1; i >= 0; i--)
			{
				float progress = 1 - (i / (float)trailLength);
				Vector2 drawPos = projectile.oldPos[i] + projectile.Size / 2 - Main.screenPosition;
				float opacity = Utilities.EaseFunction.EaseCubicOut.Ease(progress);
				float scale = progress * projectile.scale;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, projectile.GetAlpha(lightColor) * opacity, 
					projectile.oldRot[i], Main.projectileTexture[projectile.type].Size() / 2, scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item54, projectile.Center);
			MakeDust(Main.rand.Next(10, 14), 3, 1.5f, 140, 9);
		}

		public void MakeDust(int dustCount, float speed, float scale, int alpha, float radius)
		{
			for (int i = 0; i < dustCount; ++i)
			{
				Vector2 velocity = projectile.velocity.RotatedByRandom(MathHelper.Pi / 5) * Main.rand.NextFloat(0.5f, 1) * speed;
				Vector2 pos = projectile.Center + Main.rand.NextVec2CircularEven(radius, radius) * projectile.scale;
				Dust d = Dust.NewDustPerfect(pos, ModContent.DustType<Dusts.Blood>(), velocity, alpha, new Color(250, 173, 173), Main.rand.NextFloat(0.66f, 1f) * scale);
				d.noGravity = true;
			}
		}
	}
}

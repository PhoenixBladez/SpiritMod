using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentryMucus : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.SentryShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.timeLeft = 360;
			Projectile.aiStyle = 0;
			Projectile.scale = Main.rand.NextFloat(0.7f, 0.8f);
			Projectile.alpha = 140;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			MakeDust(2, 0.5f, 0.8f, 200, 4);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(spriteBatch);

			int trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
			for (int i = trailLength - 1; i >= 0; i--)
			{
				float progress = 1 - (i / (float)trailLength);
				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
				float opacity = Utilities.EaseFunction.EaseCubicOut.Ease(progress);
				float scale = progress * Projectile.scale;
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, Projectile.GetAlpha(lightColor) * opacity, 
					Projectile.oldRot[i], TextureAssets.Projectile[Projectile.type].Value.Size() / 2, scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
			MakeDust(Main.rand.Next(10, 14), 3, 1.5f, 140, 9);
		}

		public void MakeDust(int dustCount, float speed, float scale, int alpha, float radius)
		{
			for (int i = 0; i < dustCount; ++i)
			{
				Vector2 velocity = Projectile.velocity.RotatedByRandom(MathHelper.Pi / 5) * Main.rand.NextFloat(0.5f, 1) * speed;
				Vector2 pos = Projectile.Center + Main.rand.NextVec2CircularEven(radius, radius) * Projectile.scale;
				Dust d = Dust.NewDustPerfect(pos, ModContent.DustType<Dusts.Blood>(), velocity, alpha, new Color(250, 173, 173), Main.rand.NextFloat(0.66f, 1f) * scale);
				d.noGravity = true;
			}
		}
	}
}

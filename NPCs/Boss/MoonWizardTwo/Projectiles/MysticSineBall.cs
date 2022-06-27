using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticSineBall : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.damage = 13;
			//projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.hostile = true;
			Projectile.friendly = false;

		}
		bool initialized = false;
        float alphaCounter;
		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
		{
            Projectile.velocity *= 1.009f;
            alphaCounter += 0.04f;
            int rightValue = (int)Projectile.ai[1] - 1;
			if (rightValue < (double)Main.projectile.Length && rightValue != -1) {
				Projectile other = Main.projectile[rightValue];
				Vector2 direction9 = other.Center - Projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
			}
			if (!initialized) 
			{
				initialSpeed = Projectile.velocity;
				initialized = true;
			}
			if (initialSpeed.Length() < 20)
				initialSpeed *= 1.03f;
			Projectile.spriteDirection = 1;
			if (Projectile.ai[0] > 0) {
				Projectile.spriteDirection = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			distance += 0.4f;
			Projectile.ai[0] += rotationalSpeed;
			
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(Projectile.ai[0] * (Math.PI / 180)) * (distance / 3));
			Projectile.velocity = initialSpeed + offset;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineStart = Projectile.Center;
			int rightValue = (int)Projectile.ai[1] - 1;
			float collisionpoint = 0f;
			if (rightValue < (double)Main.projectile.Length && rightValue != -1) {
				Projectile other = Main.projectile[rightValue];
				Vector2 lineEnd = other.Center;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, 30, ref collisionpoint))
					return true;
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostDraw(Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), (Projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }

        public override void Kill(int timeLeft)
        {
            DustHelper.DrawDustImage(Projectile.Center, 226, 0.15f, "SpiritMod/Effects/DustImages/MoonSigil3", 1f);
        }
    }
}
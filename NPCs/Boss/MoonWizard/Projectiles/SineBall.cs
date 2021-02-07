using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class SineBall : ModProjectile
	{
		float distance = 8;
		int rotationalSpeed = 4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Ball");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		bool initialized = false;
        float alphaCounter;
		Vector2 initialSpeed = Vector2.Zero;
		public override void AI()
		{
            projectile.velocity *= 1.009f;
            alphaCounter += 0.04f;
            int rightValue = (int)projectile.ai[1] - 1;
			if (rightValue < (double)Main.projectile.Length && rightValue != -1) {
				Projectile other = Main.projectile[rightValue];
				Vector2 direction9 = other.Center - projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				if (projectile.timeLeft % 4 == 0 && distance < 1000 && other.active) {
				//	int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("MoonLightning"), 30, 0);
				//	Main.projectile[proj].timeLeft = (int)(distance / 30);
				//	Main.projectile[proj].hostile = projectile.hostile;
				//	Main.projectile[proj].friendly = projectile.friendly;
					DustHelper.DrawElectricity(projectile.Center + (projectile.velocity * 4), other.Center + (other.velocity * 4), 226, 0.35f, 30, default, 0.12f);
				}
			}
				if (!initialized) 
			{
				initialSpeed = projectile.velocity;
				initialized = true;
			}
			projectile.spriteDirection = 1;
			if (projectile.ai[0] > 0) {
				projectile.spriteDirection = 0;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.19f;
			projectile.ai[0] += rotationalSpeed;
			
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(projectile.ai[0] * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineStart = projectile.Center;
			int rightValue = (int)projectile.ai[1] - 1;
			float collisionpoint = 0f;
			if (rightValue < (double)Main.projectile.Length && rightValue != -1) {
				Projectile other = Main.projectile[rightValue];
				Vector2 lineEnd = other.Center;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), lineStart, lineEnd, projectile.scale / 2, ref collisionpoint)) {
					return true;
				}
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }

        public override void Kill(int timeLeft)
        {
            DustHelper.DrawDustImage(projectile.Center, 226, 0.15f, "SpiritMod/Effects/DustImages/MoonSigil3", 1f);
        }
    }
}
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Items.Weapon.Summon.StardustBomb
{
	public class StarShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockwave");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 64;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
		}

		//int counter = -720;
		bool boom = false;
		public override bool PreAI()
		{
			if (!boom) {
				if (Main.netMode != NetmodeID.Server && !Filters.Scene["ShockwaveTwo"].IsActive()) {
					Filters.Scene.Activate("ShockwaveTwo", Projectile.Center).GetShader().UseColor(Color.Cyan.ToVector3()).UseTargetPosition(Projectile.Center);
				}
				boom = true;
			}
			if (Main.netMode != NetmodeID.Server && Filters.Scene["ShockwaveTwo"].IsActive()) {
				float progress = (float)(8 - Math.Sqrt(Projectile.timeLeft)) * 60; // Will range from -3 to 3, 0 being the point where the bomb explodes.
				Filters.Scene["ShockwaveTwo"].GetShader().UseProgress(progress);
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server && Filters.Scene["ShockwaveTwo"].IsActive()) {
				Filters.Scene["ShockwaveTwo"].Deactivate();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            // Custom collision so all chains across the flail can cause impact.
            float collisionPoint = 0f;
			Vector2 dir = targetHitbox.Center.ToVector2() - Projectile.Center;
			dir.Normalize();
			dir *= (float)(8 - Math.Sqrt(Projectile.timeLeft)) * 80;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + dir, (Projectile.width + Projectile.height) * 0.5f * Projectile.scale, ref collisionPoint)) {
                return true;
            }
            return false;
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int cooldown = 40;
            Projectile.localNPCImmunity[target.whoAmI] = 40;
            target.immune[Projectile.owner] = cooldown;
		}
	}
}

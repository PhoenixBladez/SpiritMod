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
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.timeLeft = 64;
			projectile.tileCollide = false;
			projectile.extraUpdates = 2;
		}

		//int counter = -720;
		bool boom = false;
		public override bool PreAI()
		{
			if (!boom) {
				if (Main.netMode != NetmodeID.Server && !Filters.Scene["ShockwaveTwo"].IsActive()) {
					Filters.Scene.Activate("ShockwaveTwo", projectile.Center).GetShader().UseColor(Color.Cyan.ToVector3()).UseTargetPosition(projectile.Center);
				}
				boom = true;
			}
			if (Main.netMode != NetmodeID.Server && Filters.Scene["ShockwaveTwo"].IsActive()) {
				float progress = (float)(8 - Math.Sqrt(projectile.timeLeft)) * 60; // Will range from -3 to 3, 0 being the point where the bomb explodes.
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
			Vector2 dir = targetHitbox.Center.ToVector2() - projectile.Center;
			dir.Normalize();
			dir *= (float)(8 - Math.Sqrt(projectile.timeLeft)) * 80;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + dir, (projectile.width + projectile.height) * 0.5f * projectile.scale, ref collisionPoint)) {
                return true;
            }
            return false;
        }
		public override void OnHitNPC(NPC target, int damage, float knockBack, bool crit)
		{
			int cooldown = 40;
            projectile.localNPCImmunity[target.whoAmI] = 40;
            target.immune[projectile.owner] = cooldown;
		}
	}
}

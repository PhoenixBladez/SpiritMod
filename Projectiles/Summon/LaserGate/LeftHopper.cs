using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class LeftHopper : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Left Gate");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}

		public override bool PreAI()
		{
			projectile.timeLeft = 50;
			int rightValue = (int)projectile.ai[1];
			if (rightValue < (double)Main.projectile.Length && rightValue != 0) {
				Projectile other = Main.projectile[rightValue];
				if (other.active) {
					//rotating
					direction9 = other.Center - projectile.Center;
					int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					direction9.Normalize();
					projectile.rotation = direction9.ToRotation();
					other.ai[1] = projectile.whoAmI;
					//shoot to other guy
					timer++;
					if (timer > 4 && distance < 500) {
						timer = 0;
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, ModContent.ProjectileType<GateLaser>(), 27, 0, Main.myPlayer);
						Main.projectile[proj].timeLeft = (int)(distance / 15) - 1;
						DustHelper.DrawElectricity(projectile.Center, other.Center, 226, 0.3f);
					}
				}
			}
			return true;
		}
	}
}
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class InfernalGuard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Guard");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 8;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.penetrate = -1;
			Projectile.damage = 0;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead || player.GetSpiritPlayer().infernalSetCooldown <= 0) {
				Projectile.Kill();
				return false;
			}

			Projectile.timeLeft = 2;
			float orbitSpeed = 2 / (Projectile.ai[0] / 100);
			float radius = Projectile.ai[0];

			Projectile.localAI[1] += 2f * (float)Math.PI / 600f * orbitSpeed;
			Projectile.localAI[1] %= 2f * (float)Math.PI;

			Vector2 dir = Vector2.Normalize(player.Center - Projectile.Center);
			Projectile.rotation = (float)Math.Atan2((double)dir.Y, (double)dir.X) + 1.57f;
			Projectile.Center = player.Center + radius * new Vector2((float)Math.Cos(Projectile.localAI[1]), (float)Math.Sin(Projectile.localAI[1]));

			Projectile.ai[0] += 0.4F * Projectile.ai[1];
			if (Projectile.ai[0] >= 100)
				Projectile.ai[1] = -1;
			else if (Projectile.ai[0] <= 40)
				Projectile.ai[1] = 1;

			return false;
		}
	}
}

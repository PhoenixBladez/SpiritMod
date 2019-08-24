using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
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
			projectile.width = projectile.height = 8;

			projectile.tileCollide = false;
			projectile.ignoreWater = true;

			projectile.penetrate = -1;
			projectile.damage = 0;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			if (!player.active || player.dead || player.GetModPlayer<MyPlayer>(mod).infernalSetCooldown <= 0)
			{
				projectile.Kill();
				return false;
			}

			projectile.timeLeft = 2;
			float orbitSpeed = 2 / (projectile.ai[0] / 100);
			float radius = projectile.ai[0];

			projectile.localAI[1] += 2f * (float)Math.PI / 600f * orbitSpeed;
			projectile.localAI[1] %= 2f * (float)Math.PI;

			Vector2 dir = Vector2.Normalize(player.Center - projectile.Center);
			projectile.rotation = (float)Math.Atan2((double)dir.Y, (double)dir.X) + 1.57f;
			projectile.Center = player.Center + radius * new Vector2((float)Math.Cos(projectile.localAI[1]), (float)Math.Sin(projectile.localAI[1]));

			projectile.ai[0] += 0.4F * projectile.ai[1];
			if (projectile.ai[0] >= 100)
				projectile.ai[1] = -1;
			else if (projectile.ai[0] <= 40)
				projectile.ai[1] = 1;

			return false;
		}
	}
}

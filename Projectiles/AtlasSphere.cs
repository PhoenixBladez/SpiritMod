using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class AtlasSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas Sphere");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;

			projectile.friendly = true;
			projectile.ignoreWater = true;

			projectile.penetrate = 2;
			projectile.timeLeft = 180;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 3; ++i)
			{
				Vector2 speed = -projectile.velocity + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
				speed *= 0.4F;
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 257);
				int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206);
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("Afflicted"), 180);
		}

	}
}

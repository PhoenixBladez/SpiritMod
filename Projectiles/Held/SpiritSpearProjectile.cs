using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class SpiritSpearProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spear");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);

			aiType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SoulBurn"), 280);
		}

		int timer = 0;
		public override void AI()
		{
			timer--;

			if (timer == 0)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + 10, projectile.velocity.Y, mod.ProjectileType("SoulSpirit"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 60;
			}
		}

	}
}

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class HellTridentProj : ModProjectile
	{
		int timer = 10;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fury of the Underworld");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);
			projectile.height = 122;
			projectile.width = 122;
			aiType = ProjectileID.Trident;
		}

		public override void AI()
		{
			timer--;

			if (timer == 0)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
				Projectile.NewProjectile(projectile.Center, projectile.velocity,
					mod.ProjectileType("HellTridentProj1"), (int)(projectile.damage * 1.5f),
					projectile.knockBack, projectile.owner);
				timer = 20;
			}
		}

	}
}
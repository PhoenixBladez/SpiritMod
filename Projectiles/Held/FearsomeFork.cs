using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class FearsomeFork : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fearsome Fork");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);
			aiType = ProjectileID.Trident;
		}
		public override void AI()
		{
			timer++;
			if (timer % 7 == 1)
			{
				int newProj = Projectile.NewProjectile(projectile.position, new Vector2(0, 0), mod.ProjectileType("Pumpkin"), projectile.damage, 0, projectile.owner);
					Main.projectile[newProj].magic = false;
					Main.projectile[newProj].melee = true;
			}
		}
		
	}
}

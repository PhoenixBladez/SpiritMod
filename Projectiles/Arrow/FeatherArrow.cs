using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class FeatherArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feather Arrow");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Vector2 vel = new Vector2(2,0).RotatedBy((float)(Main.rand.Next(90) * Math.PI / 180));
				Projectile.NewProjectile(projectile.Center, vel, mod.ProjectileType("FeatherFrag"), projectile.damage, 0, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center, vel.RotatedBy(1.57), mod.ProjectileType("FeatherFrag"), projectile.damage, 0, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center, vel.RotatedBy(3.14), mod.ProjectileType("FeatherFrag"), projectile.damage, 0, Main.myPlayer);
				Projectile.NewProjectile(projectile.Center, vel.RotatedBy(4.71), mod.ProjectileType("FeatherFrag"), projectile.damage, 0, Main.myPlayer);
			return true;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 14;
			projectile.penetrate = 1;
			projectile.height = 14;
		}

	}
}

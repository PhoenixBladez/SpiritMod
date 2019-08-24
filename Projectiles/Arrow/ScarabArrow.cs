using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class ScarabArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Arrow");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.damage = 8;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			aiType = ProjectileID.BoneArrow;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.rand.Next(2) == 1)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 8, 5, 0, mod.ProjectileType("ScarabProj"), projectile.damage, 0, projectile.owner);
			else
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 8, -5, 0, mod.ProjectileType("ScarabProj"), projectile.damage, 0, projectile.owner);
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 0);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}

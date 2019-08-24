using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class EyeballProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncy Eyeball");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 30;

			projectile.aiStyle = 2;

			projectile.thrown = true;
			projectile.friendly = true;

			projectile.alpha = 0;
			projectile.penetrate = 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();

			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y * 0.9f;

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 5, 0f, 0f, 100, default(Color), 1f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("Eyeball"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
			}
		}

	}
}

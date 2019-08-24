using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class BouncingSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Spore");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 44;
			projectile.width = 42;
			projectile.friendly = false;
			projectile.aiStyle = 2;
			projectile.penetrate = 4;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();


			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * .5f;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * -1.3f;

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}

		public override void AI()
		{
			projectile.rotation += 0.3f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 5; num621++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height,
					2, 0f, 0f, 100, default(Color), 2f);
			}
		}

	}
}
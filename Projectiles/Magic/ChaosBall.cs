using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ChaosBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ball");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = true;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 27, 0f, 0f);
			int moredust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 27, 0f, 0f);
			int evenmoredust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 27, 0f, 0f);
			projectile.rotation += 0.4f;

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 27, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 27, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
		}

	}
}

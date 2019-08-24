using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class IchorBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Bomb");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 500;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				5, projectile.velocity.X * .5f, projectile.velocity.Y * .5f);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;

			return true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(projectile.position + projectile.velocity,
					projectile.width, projectile.height,
					5, projectile.oldVelocity.X * .5f, projectile.oldVelocity.Y * .5f);
			}

			for (int h = 0; h < 4; h++)
			{
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 5f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					vel.X, vel.Y, 280, projectile.damage, 0, Main.myPlayer);
			}
		}


	}
}

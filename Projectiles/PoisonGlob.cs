using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PoisonGlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Glob");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;

			projectile.penetrate = 1;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowHostile;

			projectile.ranged = true;
			projectile.friendly = false;
			projectile.hostile = true;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 107);
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 107);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);
		}
	}
}

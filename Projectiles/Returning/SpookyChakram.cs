using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class SpookyChakram : ModProjectile
	{
		int Counter = 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Chakram");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.magic = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			int num622 = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X, projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height, 191, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.5f;
			projectile.rotation += 0.1f;
				Counter++;
			if (Counter % 15 == 1)
			{
					int randFire = Main.rand.Next(3);
					int newProj = Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("Pumpkin"), projectile.damage, 0, projectile.owner);
					Main.projectile[newProj].magic = false;
					Main.projectile[newProj].melee = true;
				
			}
		}


	}
}

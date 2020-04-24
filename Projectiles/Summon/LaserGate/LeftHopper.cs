using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class LeftHopper : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Left Gate");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}

		public override bool PreAI()
		{
			int rightValue = (int)projectile.ai[1];
			if (rightValue < (double)Main.projectile.Length && rightValue != 0)
			{
				Projectile other = Main.projectile[rightValue];
				if (other.active)
				{
					//rotating
					direction9 = other.Center - projectile.Center;
					int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y* direction9.Y));
					direction9.Normalize();
					projectile.rotation = direction9.ToRotation();
					other.ai[1] = projectile.whoAmI;
					//shoot to other guy
					timer++;
					if (timer > 3 && distance < 500)
					{
						timer = 0;
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("GateLaser"), 27, 1, Main.myPlayer);
						Main.projectile[proj].timeLeft = (int)(distance / 30) - 1;
					}		
				}
			}
			return true;
		}
	}
}
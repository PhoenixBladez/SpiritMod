using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BubblePumpProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 999999;
		}

		int counter = 7;
		bool firing = false;
		Vector2 direction = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.heldProj = projectile.whoAmI;
			player.itemTime = 5;
			player.itemAnimation = 5;
			player.velocity.X *= 0.97f;
			if (counter == 0)
			{
				 direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
				direction.Normalize();
				direction *= 7f;
			}
			if(player.channel && !firing)
		 	{
				projectile.position = player.Center;
				if (counter < 150)
				{
					counter++;
					if (counter % 30 == 29)
					{
						Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 2);
					}
				}
			} 
			else {
				firing = true;
				if (counter > 0)
				{
					counter -= 2;
					if (counter % 7 == 0)
					{
						Projectile.NewProjectile(player.Center, direction.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.85f, 1.15f), ModContent.ProjectileType<GunBubble>(), projectile.damage, projectile.knockBack, projectile.owner);
					}
				}
				else
				{
					projectile.active = false;
				}
			}
			return true;
		}
	}
}

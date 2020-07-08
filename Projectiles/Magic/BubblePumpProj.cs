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
            projectile.ai[1]++;
            if (projectile.ai[1] >= 60)
            {
                projectile.ai[1] = 0;
                if (player.statMana > 0)
                {
                    player.statMana-= 15;
                }
            }
			if (player.statMana <= 0)
            {
                projectile.Kill();
            }
			player.itemTime = 5;
			player.itemAnimation = 5;
			player.velocity.X *= 0.97f;
			if (counter == 7)
			{
				 direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
				direction.Normalize();
				direction *= 7f;
			}
			if(player.channel && !firing)
		 	{
				projectile.position = player.Center;
				if (counter < 100)
				{
					counter++;
					if (counter % 20 == 19)
					{
						Main.PlaySound(25, (int)projectile.position.X, (int)projectile.position.Y);
					}
				}
			} 
			else {
				firing = true;
				if (counter > 0)
				{
					counter -= 2;
					if (counter % 5 == 0)
					{
						int bubbleproj;
						bubbleproj = Main.rand.Next(new int[] { 
							ModContent.ProjectileType<GunBubble1>(), 
							ModContent.ProjectileType<GunBubble2>(), 
							ModContent.ProjectileType<GunBubble3>(), 
							ModContent.ProjectileType<GunBubble4>(), 
							ModContent.ProjectileType<GunBubble5>()
						});
						Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 85);
						Projectile.NewProjectile(player.Center + (direction * 5), direction.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.85f, 1.15f), bubbleproj, projectile.damage, projectile.knockBack, projectile.owner);
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

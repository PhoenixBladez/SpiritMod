using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class StarMapProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Map");
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
			projectile.alpha = 255;
			projectile.timeLeft = 999999;
		}

        int counter = 0;
        public override bool PreAI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			
			Player player = Main.player[projectile.owner];
			if (player.channel)
			{
				projectile.position = player.position;
				player.velocity.X *= 0.97f;
				counter++;
				if (counter > 200)
				{
					Dust.NewDust(player.position, player.width,player.height, 226);
				}
			}
			else
			{
				if (counter > 200)
				{
					Vector2 direction = Main.MouseWorld - (player.Center - new Vector2(4,4));
					direction.Normalize();
					direction*= 15f;
					Projectile.NewProjectile(player.Center - new Vector2(4,4), direction, mod.ProjectileType("TeleportBolt"), 0, 0, projectile.owner);
				}
				projectile.active = false;
			}
			return true;
		}

	}
}

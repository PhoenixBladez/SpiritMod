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
            DoDustEffect(projectile.Center, 14f);
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			
			Player player = Main.player[projectile.owner];
			if (player.channel)
			{
				projectile.position = player.position;
				player.velocity.X *= 0.97f;
				counter++;
				if (counter > 160)
				{
                    DoDustEffect(projectile.Center, 54f);
                }
			}
			else
			{
				if (counter > 160)
				{
					Vector2 direction = Main.MouseWorld - (player.Center - new Vector2(4,4));
					direction.Normalize();
					direction*= 15f;
					Projectile.NewProjectile(player.Center - new Vector2(4,4), direction, ModContent.ProjectileType<TeleportBolt>(), 0, 0, projectile.owner);
				}
				projectile.active = false;
			}
			return true;
		}
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .3f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}

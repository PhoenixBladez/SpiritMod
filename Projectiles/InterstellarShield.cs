using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class InterstellarShield : ModProjectile
    {
		public bool shooting = false;
		double dist = 80;
		Vector2 direction = Vector2.Zero;
		int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("InterstellarShield");
		}
        public override void SetDefaults()
        {
            projectile.penetrate = 600;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 2;
			projectile.damage = 1;
            projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.width = projectile.height = 24;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int k = 0; k < 15; k++)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<Sparkle>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
			}
			}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			//Factors for calculations
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list)
			{
				if (proj.hostile && proj.active && proj.timeLeft > 2)
				{
					if (proj.damage < 65)
					{
						counter++;
						proj.timeLeft = 2;
						Main.PlaySound(SoundID.Item93, projectile.position);
						proj.active = false;
					}
					else
					{
						counter+= 5;
					}
				}
			}
			if (counter >= 2)
			{
				projectile.active = false;
				((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).shieldsLeft -= 1;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
			}
			Vector2 center = projectile.Center;
            float num8 = (float)player.miscCounter / 60f;
            float num7 = 1.0471975512f * 2;
            for (int i = 0; i < 3; i++)
            {
                int num6 = Dust.NewDust(center, 0, 0, 180, 0f, 0f, 100, default(Color), 1.3f);
                Main.dust[num6].noGravity = true;
                Main.dust[num6].velocity = Vector2.Zero;
                Main.dust[num6].noLight = true;
                Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
            }
			
				double deg = (double) projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
				double rad = deg * (Math.PI / 180); //Convert degrees to radians
 			
				/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
				/distance for the desired distance away from the player minus the projectile's width   /
				/and height divided by two so the center of the projectile is at the right place.     */
 			
				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
				projectile.ai[1] += .38f;
				if (((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).ShieldCore)
				{
					projectile.timeLeft = 2;
				}
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
				direction = player.Center - projectile.Center;
				direction.Normalize();
		}
	}
}
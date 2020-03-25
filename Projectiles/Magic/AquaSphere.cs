using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AquaSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aqua Sphere");
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 120;
            projectile.alpha = 0;
			projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
            for (int i = 0; i < 2; i++)
			{
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
                Main.dust[d].noGravity = true;
			}

			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, -4, mod.ProjectileType("AquaSphere2"), projectile.damage/2, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, -4, mod.ProjectileType("AquaSphere2"), projectile.damage/2, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, 4, mod.ProjectileType("AquaSphere2"), projectile.damage/2, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, 4, mod.ProjectileType("AquaSphere2"), projectile.damage/2, projectile.knockBack, Main.myPlayer);
        }

		public override void AI()
		{
            projectile.alpha+= 3;
            projectile.scale += .011f;
            if (projectile.alpha >= 160)
            {
                projectile.Kill();
            }


            for (int i = 0; i < 5; i++)
            {
                Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)projectile.height, (float)projectile.height) * projectile.scale * 1.45f / 2f;
                int index = Dust.NewDust(projectile.Center + vector2, 0, 0, 187, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].position = projectile.Center + vector2;
                Main.dust[index].velocity = Vector2.Zero;
                Main.dust[index].noGravity = true;
            }
            Lighting.AddLight(projectile.position, 0.1f, 0.2f, 0.3f);
		}

	}
}
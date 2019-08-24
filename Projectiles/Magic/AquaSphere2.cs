using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AquaSphere2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aqua Sphere");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
			}
		}

		public override void AI()
		{
			projectile.velocity *= 0.95f;

			for (int i = 1; i <= 3; i++)
			{
				int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					33, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 2f);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			Lighting.AddLight(projectile.position, 0.4f, 0.1f, 0.1f);
		}

	}
}
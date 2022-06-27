using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hide = true;
			Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++) {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
				Main.dust[d].noGravity = true;
			}
		}

		public override void AI()
		{
			Projectile.velocity *= 0.93f;

			for (int i = 1; i <= 3; i++) {
				int num1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
					DustID.Flare_Blue, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1f);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			Lighting.AddLight(Projectile.position, 0.1f, 0.2f, 0.3f);
		}

	}
}
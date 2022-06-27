using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class NovaBeam1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Beam");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 500;
			Projectile.light = 0;
			Projectile.extraUpdates = 30;
		}

		public override void AI()
		{
			for (int num447 = 0; num447 < 2; num447++) {
				Vector2 vector33 = Projectile.position;
				vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
				Projectile.alpha = 255;
				int num448 = Dust.NewDust(vector33, 1, 1, DustID.GoldCoin, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.25f);
				Main.dust[num448].noGravity = true;
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.2f;
			}
			return;
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 5; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
			}
		}
	}
}
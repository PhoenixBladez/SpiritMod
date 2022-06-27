using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class BatBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 26;

			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;

			Projectile.penetrate = 10;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 200;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int num623 = 0; num623 < 25; num623++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.SpookyWood, 0f, 0f, 100, default, 2f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].velocity *= 1.5f;
				Main.dust[num622].scale = 0.8f;
			}
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
			int num622 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X, Projectile.position.Y - Projectile.velocity.Y), Projectile.width, Projectile.height, DustID.SpookyWood, 0f, 0f, 100, default, 2f);
			Main.dust[num622].noGravity = true;
			Main.dust[num622].scale = 0.5f;

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 5)
					Projectile.frame = 0;
			}
		}


	}
}

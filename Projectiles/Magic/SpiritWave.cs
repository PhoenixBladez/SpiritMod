using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SpiritWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Wave");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 55;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			if (Projectile.localAI[1] == 0f) {
				Projectile.localAI[1] = 1f;
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			}
			else if (Projectile.ai[0] == 1f) {
				Projectile.scale -= 0.01f;
				Projectile.alpha += 50;
				if (Projectile.alpha >= 255) {
					Projectile.ai[0] = 2f;
					Projectile.alpha = 255;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + 4.71F;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;

			Main.dust[dust2].scale = 2f;
			Main.dust[dust].scale = 2f;
			return false;
		}

	}
}
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class GunProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Defier");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 18;
			Projectile.aiStyle = 113;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			Projectile.light = 0;
			AIType = ProjectileID.ThrowingKnife;
		}

		public override void AI()
		{
			Projectile.rotation += 0.3f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Asphalt);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}
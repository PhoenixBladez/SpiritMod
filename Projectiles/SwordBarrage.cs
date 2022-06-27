using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SwordBarrage : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword Barrage");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 48;
			Projectile.aiStyle = 113;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
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
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Asphalt);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}
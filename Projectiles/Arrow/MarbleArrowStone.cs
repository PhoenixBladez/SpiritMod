using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class MarbleArrowStone : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Marble Shard");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 7200;
		}

		public override void AI()
		{
			Projectile.rotation += .1f;

			for(int k = 0; k < 2; k++) {
				Dust.NewDustPerfect(Projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * 0.4f, 0, default, 0.3f);
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for(int k = 0; k < 6; k++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCritter_LessOutline, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			}
		}
	}
}

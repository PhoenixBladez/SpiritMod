using Microsoft.Xna.Framework;
using SpiritMod.Prim;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HeartilleryMinionClump : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Clumped Blood");

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			ProjectileID.Sets.SentryShot[projectile.type] = true;
            projectile.alpha = 255;
			projectile.penetrate = 3;
			projectile.timeLeft = 120;
		}

		bool primsCreated = false;
		public override void AI()
		{
            projectile.velocity.Y += .4325f;

			if (!primsCreated) {
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(projectile));
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(SoundID.NPCHit, 8).WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
			for (int i = 0; i < 20; i++) {
				Dust.NewDustPerfect(projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
			}
		}
	}
}

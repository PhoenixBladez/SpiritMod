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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clumped Blood");

			ProjectileID.Sets.SentryShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
            Projectile.alpha = 255;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 120;
		}

		bool primsCreated = false;

		public override void AI()
		{
            Projectile.velocity.Y += .4325f;

			if (!primsCreated) {
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(Projectile));
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit8 with { PitchVariance = 0.2f, Volume = 0.3f }, Projectile.Center);
			for (int i = 0; i < 20; i++)
				Dust.NewDustPerfect(Projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * Projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
		}
	}
}


using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	class ShadowflameOrbBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
		}
        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = Projectile.velocity;
            }
			if (Main.rand.NextBool(3))
            {
                Vector2 position = Projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = Projectile.velocity;
            }
        }

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			for (float num2 = 0.0f; (double)num2 < 6; ++num2) {
				int dustIndex = Dust.NewDust(Projectile.position, 2, 2, DustID.ShadowbeamStaff, 0f, 0f, 0, default, .6f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = Vector2.Normalize(Projectile.position.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 2.36f;
			}

		}
	}
}

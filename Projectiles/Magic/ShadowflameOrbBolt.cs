
using Microsoft.Xna.Framework;

using Terraria;
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
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
			projectile.height = 14;
			projectile.width = 14;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 position = projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = projectile.velocity;
            }
			if (Main.rand.NextBool(3))
            {
                Vector2 position = projectile.Center;
                Dust dust = Main.dust[Dust.NewDust(position, 0, 0, DustID.ShadowbeamStaff, 0f, 0f, 0, new Color(255, 255, 255), 0.64947368f)];
                dust.noLight = true;
                dust.noGravity = true;
                dust.velocity = projectile.velocity;
            }
        }

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 5;
			projectile.height = 5;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (float num2 = 0.0f; (double)num2 < 6; ++num2) {
				int dustIndex = Dust.NewDust(projectile.position, 2, 2, DustID.ShadowbeamStaff, 0f, 0f, 0, default, .6f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = Vector2.Normalize(projectile.position.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 2.36f;
			}

		}
	}
}

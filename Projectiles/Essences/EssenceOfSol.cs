using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Essences
{
	class EssenceOfSol : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Essence of Sol");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 500;
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;

			AIType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SolarExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.height = 50;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2f);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2f);

			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
				Main.dust[dust].velocity *= 3f;
				if (Main.rand.NextBool(2))
				{
					Main.dust[dust].scale = 0.5f;
					Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}
			for (int i = 0; i < 35; i++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 3f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
				Main.dust[dust].velocity *= 2f;
			}

			for (int i = 0; i < 3; i++)
			{
				float scaleFactor10 = 0.33f;
				if (i == 1)
					scaleFactor10 = 0.66f;
				else if (i == 2)
					scaleFactor10 = 1f;

				for (int j = 0; j < 4; ++j)
				{
					int gore = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2f) - 24f, Projectile.position.Y + (Projectile.height / 2f) - 24f), default, Main.rand.Next(61, 64), 1f);
					Main.gore[gore].velocity *= scaleFactor10;
					Main.gore[gore].velocity.X += 1f;
					Main.gore[gore].velocity.Y += 1f;
				}
			}

			Projectile.position = Projectile.Center;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
		}

		public override void AI()
		{
			for (int i = 0; i < 3; ++i)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CopperCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.6f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.Kill();
	}
}
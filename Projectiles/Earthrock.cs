using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class Earthrock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earth Shard");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 300;
			projectile.height = 16;
			projectile.width = 12;
			projectile.penetrate = 5;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			{
				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 70; num623++) {
					int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BubbleBlock, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 1.5f;
					num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height - 10, DustID.UnusedWhiteBluePurple, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.9f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(ModContent.BuffType<Afflicted>(), 240);
		}

	}
}

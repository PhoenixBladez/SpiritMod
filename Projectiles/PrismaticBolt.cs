using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PrismaticBolt : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Prismatic Bolt");

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.light = 0.5f;
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.damage = 10;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
				{
					for (int num621 = 0; num621 < 40; num621++)
					{
						int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 2f);
						Main.dust[num622].velocity *= 3f;
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num622].scale = 0.5f;
							Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						}
					}
					for (int num623 = 0; num623 < 70; num623++)
					{
						int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BubbleBlock, 0f, 0f, 100, default, 1f);
						Main.dust[num624].noGravity = true;
						Main.dust[num624].velocity *= 1.5f;
						num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 1f);
						Main.dust[num624].velocity *= 2f;
					}
				});
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
			{
				projectile.tileCollide = false;
				projectile.ai[1] = 0f;
				projectile.alpha = 255;
				projectile.position.X = projectile.Center.X;
				projectile.position.Y = projectile.Center.Y;
				projectile.width = 30;
				projectile.height = 30;
				projectile.position.X = projectile.position.X - projectile.width / 2f;
				projectile.position.Y = projectile.position.Y - projectile.height / 2f;
				projectile.knockBack = 4f;
			}

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBlock);
			int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.UnusedWhiteBluePurple);
			Main.dust[dust].noGravity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(ModContent.BuffType<Buffs.DoT.Afflicted>(), 180);
	}
}
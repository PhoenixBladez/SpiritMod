using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class PrismaticBolt : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Prismatic Bolt");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.light = 0.5f;
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.damage = 10;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate
				{
					for (int num621 = 0; num621 < 40; num621++)
					{
						int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 2f);
						Main.dust[num622].velocity *= 3f;
						if (Main.rand.NextBool(2))
						{
							Main.dust[num622].scale = 0.5f;
							Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						}
					}
					for (int num623 = 0; num623 < 70; num623++)
					{
						int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BubbleBlock, 0f, 0f, 100, default, 1f);
						Main.dust[num624].noGravity = true;
						Main.dust[num624].velocity *= 1.5f;
						num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 1f);
						Main.dust[num624].velocity *= 2f;
					}
				});
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.tileCollide = false;
				Projectile.ai[1] = 0f;
				Projectile.alpha = 255;
				Projectile.position.X = Projectile.Center.X;
				Projectile.position.Y = Projectile.Center.Y;
				Projectile.width = 30;
				Projectile.height = 30;
				Projectile.position.X = Projectile.position.X - Projectile.width / 2f;
				Projectile.position.Y = Projectile.position.Y - Projectile.height / 2f;
				Projectile.knockBack = 4f;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBlock);
			int dust1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple);
			Main.dust[dust].noGravity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(ModContent.BuffType<Buffs.DoT.Afflicted>(), 180);
	}
}
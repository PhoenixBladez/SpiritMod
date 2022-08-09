using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class TerraBomb : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Elemental Bomb");

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SpikyBall);
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			Projectile.hide = true;
			Projectile.tileCollide = true;
		}

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			if (Projectile.alpha < 170) {
				for (int i = 0; i < 10; i++) {
					float x = Projectile.Center.X - 1 - Projectile.velocity.X / 10f * i;
					float y = Projectile.Center.Y - 1 - Projectile.velocity.Y / 10f * i;
					int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.TerraBlade);
					Main.dust[num].alpha = Projectile.alpha;
					Main.dust[num].velocity = Vector2.Zero;
					Main.dust[num].noGravity = true;
				}

				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}

			Projectile.alpha = Math.Max(0, Projectile.alpha - 25);
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);

			for (int num621 = 0; num621 < 20; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.NextBool(2)) {
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}
			for (int num623 = 0; num623 < 35; num623++) {
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 3f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 2f);
				Main.dust[num624].velocity *= 2f;
			}

			for (int num625 = 0; num625 < 3; num625++) {
				float scaleFactor10 = 0.33f;
				if (num625 == 1)
					scaleFactor10 = 0.66f;
				else if (num625 == 2)
					scaleFactor10 = 1f;

				for (int i = 0; i < 4; ++i)
				{
					int num626 = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
					Main.gore[num626].velocity *= scaleFactor10;
					Main.gore[num626].velocity.X += 1f;
					Main.gore[num626].velocity.Y += 1f;
				}
			}

			Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);

			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				});
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int debuff = Main.rand.Next(3);
			if (debuff == 0)
				target.AddBuff(BuffID.OnFire, 300, true);
			else if (debuff == 1)
				target.AddBuff(BuffID.Frostburn, 300, true);
			else
				target.AddBuff(BuffID.CursedInferno, 300, true);
		}

	}
}


using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class SpiritStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Star");
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 150;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			for (int i = 0; i < 10; i++) {
				int num = 5;
				for (int k = 0; k < 6; k++) {
					int index2 = Dust.NewDust(Projectile.position, 4, 4, DustID.Clentaminator_Green, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
					Main.dust[index2].scale = .48f;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 4f) {
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Clentaminator_Green, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .68f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			int n = 2;
			int deviation = Main.rand.Next(0, 180);
			for (int i = 0; i < n; i++) {
				float rotation = MathHelper.ToRadians(270 / n * i + deviation);
				Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed.X *= 4.5f;
				perturbedSpeed.Y *= 4.5f;
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<StarSoul>(), Projectile.damage / 3, 2, Projectile.owner);
			}

			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

			for (int num623 = 0; num623 < 25; num623++) {
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, default, .8f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 1f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(ModContent.BuffType<StarFracture>(), 200, true);

			Projectile.Kill();
		}
	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Blood : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Clusters");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 4;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 60;
		}

		int counter;
		public override void AI()
		{

			Projectile.ai[0] += 0.5f * Projectile.direction;
			if (Projectile.ai[0] > 20f || Projectile.ai[0] < -20f) {
				Projectile.Kill();
			}
			for (int num1438 = 0; num1438 < 2; num1438 = counter + 1) {
				Vector2 center22 = Projectile.Center;
				Projectile.scale = 1f - Projectile.localAI[0];
				Projectile.width = (int)(20f * Projectile.scale);
				Projectile.height = Projectile.width;
				Projectile.position.X = center22.X - (float)(Projectile.width / 2);
				Projectile.position.Y = center22.Y - (float)(Projectile.height / 2);
				if ((double)Projectile.localAI[0] < 0.1) {
					Projectile.localAI[0] += 0.01f;
				}
				else {
					Projectile.localAI[0] += 0.025f;
				}
				if (Projectile.localAI[0] >= 0.95f) {
					Projectile.Kill();
				}
				Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
				Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f * Projectile.direction;
				if (Projectile.velocity.Length() > 16f) {
					Projectile.velocity.Normalize();
					Projectile.velocity *= 16f;
				}
				Projectile.ai[0] *= 1.05f;
				if (Projectile.scale < 1f) {
					int num1448 = 0;
					while ((float)num1448 < Projectile.scale * 10f) {
						Vector2 position177 = new Vector2(Projectile.position.X, Projectile.position.Y);
						int width138 = Projectile.width;
						int height138 = Projectile.height;
						float x38 = Projectile.velocity.X;
						float y36 = Projectile.velocity.Y;
						Color newColor5 = default;
						int num1447 = Dust.NewDust(position177, width138, height138, DustID.Blood, x38, y36, 100, newColor5, 1.1f);
						Main.dust[num1447].position = (Main.dust[num1447].position + Projectile.Center) / 2f;
						Main.dust[num1447].noGravity = true;
						Dust dust81 = Main.dust[num1447];
						dust81.velocity *= 0.1f;
						dust81 = Main.dust[num1447];
						dust81.velocity -= Projectile.velocity * (1.3f - Projectile.scale);
						Main.dust[num1447].fadeIn = (float)(100 + Projectile.owner);
						dust81 = Main.dust[num1447];
						dust81.scale += Projectile.scale * 0.45f;
						counter = num1448;
						num1448 = counter + 1;
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			if (crit && player.statLife != player.statLifeMax2) {
				int lifeToHeal = 0;

				if (player.statLife + 3 <= player.statLifeMax2)
					lifeToHeal = 3;
				else
					lifeToHeal = player.statLifeMax2 - player.statLife;

				player.statLife += lifeToHeal;
				player.HealEffect(lifeToHeal);
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
			}
		}
	}
}
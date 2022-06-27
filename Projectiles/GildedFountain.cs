using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class GildedFountain : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Cascade");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 16;
			Projectile.height = 70;
			Projectile.penetrate = 5;
			Projectile.alpha = 255;
			Projectile.timeLeft = 240;
		}
		public override bool PreAI()
		{
			float num1627 = 2f;
			float num1626 = (float)Projectile.timeLeft / 60f;
			if (num1626 < 1f) {
				num1627 *= num1626;
			}

			for (int num1625 = 0; num1625 < 4; num1625 = 10 + 1) {
				Vector2 vector267 = new Vector2(0f, 0f - num1627);
				vector267 *= 0.85f + (float)Main.rand.NextDouble() * 0.2f;
				Vector2 spinningpoint18 = vector267;
				double radians17 = (Main.rand.NextDouble() - 0.5) * 1.5707963705062866;
				Vector2 vector333 = default;
				vector267 = spinningpoint18.RotatedBy(radians17, vector333);
				Vector2 position160 = Projectile.position;
				int width124 = Projectile.width;
				int height124 = Projectile.height;
				int num1624 = Dust.NewDust(position160, width124, height124, DustID.FireworkFountain_Yellow, 0f, 0f, 100, new Color(), 1f);
				Dust dust77 = Main.dust[num1624];
				dust77.scale = 1f + (float)Main.rand.NextDouble() * 0.3f;
				Dust dust81 = dust77;
				dust81.velocity *= 0.5f;
				if (dust77.velocity.Y > 0f) {
					Dust expr_1D344_cp_0 = dust77;
					expr_1D344_cp_0.velocity.Y = expr_1D344_cp_0.velocity.Y * -1f;
				}
				dust81 = dust77;
				dust81.position -= new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0f);
				dust81 = dust77;
				dust81.velocity += vector267;
				dust77.scale = 0.6f;
				dust77.fadeIn = dust77.scale + 0.2f;
				Dust expr_1D3CA_cp_0 = dust77;
				expr_1D3CA_cp_0.velocity.Y = expr_1D3CA_cp_0.velocity.Y * 2f;
			}
			return true;
		}
		public override void AI()
		{
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 1) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(Projectile.position + Projectile.velocity,
				Projectile.width, Projectile.height,
				DustID.FireworkFountain_Yellow, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
		}
	}
}
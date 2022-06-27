using SpiritMod.Buffs;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class EyeOfTheInfernoProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of the Inferno");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Yelets);
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Yelets;
		}

		public override void AI()
		{
			Projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if (Projectile.localAI[1] <= 1.0) {
				return;
			}

			int num4 = (int)Projectile.localAI[1];
			if (num4 <= 30) {
				if (num4 != 10) {
					if (num4 == 30) {
						num2--;
					}
				}
				else {
					num2--;
				}
			}
			else if (num4 != 50) {
				if (num4 == 70) {
					num2--;
				}
			}
			else {
				num2--;
			}

			if ((int)Projectile.localAI[1] == 20) {
				int num5 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, num, num2, ModContent.ProjectileType<SpikeBall>(), 50, Projectile.knockBack, Projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = Projectile.whoAmI;
			}

			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 280);
			Projectile.velocity *= 0f;
		}

	}
}

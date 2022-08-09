using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class FlayedExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flayed Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.alpha = 255;
			Projectile.timeLeft = 1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
		}
		public override void AI()
		{
			ProjectileExtras.Explode(Projectile.whoAmI, 260, 260,
			delegate {
				for (int i = 0; i < 60; i++) {
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NightmareDust>(), 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Main.dust[num].scale = 1.5f;
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
					if (Main.dust[num].position != Projectile.Center) {
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 9f;
					}
				}
			});
			Projectile.active = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(ModContent.BuffType<SurgingAnguish>(), 200, true);
		}

	}
}
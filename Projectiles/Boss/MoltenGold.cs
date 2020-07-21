using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class MoltenGold : ModProjectile
	{
		private int DamageAdditive;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Gold");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			projectile.width = 25;
			projectile.height = 25;
			projectile.friendly = false;
			projectile.alpha = 255;
			projectile.hostile = true;
			projectile.penetrate = 1;
		}
		int counter = -1440;
		public override void AI()
		{
			counter++;
			if (counter >= 1440) {
				counter = -1440;
			}
			for (int i = 0; i < 10; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

				int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].scale *= .9f;
				Main.dust[num].noGravity = true;

			}
			for (int f = 0; f < 10; f++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)f;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)f;

				int num = Dust.NewDust(projectile.Center - new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].scale *= .9f;
				Main.dust[num].noGravity = true;

			}
			for (int j = 0; j < 6; j++) {

				int num2 = Dust.NewDust(projectile.Center, 6, 6, 244, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2].velocity *= 0f;
				Main.dust[num2].scale *= .6f;
				Main.dust[num2].noGravity = true;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 240, true);
		}
	}
}

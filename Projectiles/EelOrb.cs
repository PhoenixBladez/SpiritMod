using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class EelOrb : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Orb");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 500;
			projectile.height = 6;
			projectile.magic = true;
			projectile.width = 6;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			aiType = ProjectileID.Bullet;
		}
		int counter;
		public override void AI()
		{
			counter++;
			if (counter >= 720) {
				counter = -720;
			}

			timer++;
			if (timer == 50) {
				projectile.velocity *= 0.01f;
			}
			if (timer >= 50 && timer <= 100) {
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = 0.9f;
				Main.dust[dust].scale = 0.9f;
			}
			else {
				for (int i = 0; i < 6; i++) {
					float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

					int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 13.2f).RotatedBy(projectile.rotation), 6, 6, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .7f;
					Main.dust[num].noGravity = true;

				}
			}
			if (timer == 100) {
				projectile.velocity *= 80f;
			}
			if (timer >= 110) {
				timer = 0;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) == 0)
				target.AddBuff(mod.BuffType("ElectrifiedV2"), 120, true);
		}

	}
}

using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class CrystalShadow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbral Crystal");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 10;

			projectile.hostile = true;

			projectile.penetrate = -1;
		}

		public float counter = -1440;
		public override void AI()
		{
			counter++;
			if (counter >= 1440) {
				counter = -1440;
			}
			for (int i = 0; i < 10; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

				int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 173, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].position -= projectile.velocity * 0.5f;
				Main.dust[num].scale *= .8f;
				Main.dust[num].noGravity = true;

			}
			if (projectile.ai[0] == 0) {
				projectile.frame = Main.rand.Next(5);
				projectile.ai[0] = 1;
			}
			else if (projectile.ai[0] == 1) {
				projectile.ai[1]++;
				if (projectile.ai[1] >= 60) {
					projectile.velocity *= 5;
					projectile.ai[0] = 2;
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}

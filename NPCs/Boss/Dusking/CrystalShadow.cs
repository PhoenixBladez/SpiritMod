using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class CrystalShadow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbral Crystal");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 10;

			Projectile.hostile = true;

			Projectile.penetrate = -1;
		}

		public float counter = -1440;
		public override void AI()
		{
			counter++;
			if (counter >= 1440) {
				counter = -1440;
			}
			for (int i = 0; i < 10; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;

				int num = Dust.NewDust(Projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(Projectile.rotation), 6, 6, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f);
				Main.dust[num].velocity *= .1f;
				Main.dust[num].position -= Projectile.velocity * 0.5f;
				Main.dust[num].scale *= .8f;
				Main.dust[num].noGravity = true;

			}
			if (Projectile.ai[0] == 0) {
				Projectile.frame = Main.rand.Next(5);
				Projectile.ai[0] = 1;
			}
			else if (Projectile.ai[0] == 1) {
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 60) {
					Projectile.velocity *= 5;
					Projectile.ai[0] = 2;
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

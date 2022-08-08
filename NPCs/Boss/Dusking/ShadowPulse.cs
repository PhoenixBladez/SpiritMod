using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Dusking
{
	public class ShadowPulse : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Pulse");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.height = 20;
			Projectile.width = 8;
			AIType = ProjectileID.DeathLaser;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.3f, 0.06f, 0.05f);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 2; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 3f);
			Main.dust[num624].velocity *= 0f;
			Main.dust[num624].scale *= 0.3f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(8))
				target.AddBuff(ModContent.BuffType<Shadowflame>(), 200);
		}

	}
}

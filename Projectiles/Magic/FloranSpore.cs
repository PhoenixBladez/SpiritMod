using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class FloranSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Spore");
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 180;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 1;
		}
		public float counter = -1440;
		bool stopped = false;
		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.velocity.Length() >= 0.1) {
				if (Projectile.velocity.X > 0)
					Projectile.velocity.X -= 0.2f;
				else if (Projectile.velocity.X < 0)
					Projectile.velocity.X += 0.2f;

				if (Projectile.velocity.Y > 0)
					Projectile.velocity.Y -= 0.2f;
				else if (Projectile.velocity.Y < 0)
					Projectile.velocity.Y += 0.2f;
			}
			if (Projectile.velocity.Length() <= 0.1f) {
				Projectile.velocity = Vector2.Zero;
				stopped = true;
			}
			if (!stopped) {
				if (Main.rand.Next(5) == 0) {
					int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
					Main.dust[d].scale *= 0.42f;
				}
				for (int i = 0; i < 6; i++) {
					float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
					float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;

					int num = Dust.NewDust(Projectile.Center, 6, 6, DustID.JungleGrass, 0f, 0f, 0, default, 1f);
					Main.dust[num].velocity *= .1f;
					Main.dust[num].scale *= .9f;
					Main.dust[num].noGravity = true;

				}
			}
			if (Projectile.ai[0] % 2 == 0)
				Projectile.alpha += 3;
			if (Projectile.alpha >= 250)
				Projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				int d = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.JungleGrass, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
				Main.dust[d].scale *= 0.42f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
		}

	}
}
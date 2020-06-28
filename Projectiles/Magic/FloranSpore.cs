using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

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
			projectile.width = 30;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 180;
			projectile.alpha = 0;
			projectile.extraUpdates = 1;
		}
		public float counter = -1440;
		bool stopped = false;
		public override void AI()
		{
			projectile.ai[0]++;
			if(projectile.velocity.Length() >= 0.1) {
				if(projectile.velocity.X > 0)
					projectile.velocity.X -= 0.2f;
				else if(projectile.velocity.X < 0)
					projectile.velocity.X += 0.2f;

				if(projectile.velocity.Y > 0)
					projectile.velocity.Y -= 0.2f;
				else if(projectile.velocity.Y < 0)
					projectile.velocity.Y += 0.2f;
				if(projectile.velocity.Length() <= 0.1f) {
					projectile.velocity = Vector2.Zero;
					stopped = true;
				}
				if(!stopped) {
					if(Main.rand.Next(5) == 0) {
						int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 39);
						Main.dust[d].scale *= 0.42f;
					}
					for(int i = 0; i < 6; i++) {
						float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
						float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

						int num = Dust.NewDust(projectile.Center, 6, 6, 39, 0f, 0f, 0, default(Color), 1f);
						Main.dust[num].velocity *= .1f;
						Main.dust[num].scale *= .9f;
						Main.dust[num].noGravity = true;

					}
				}
				if(projectile.ai[0] % 2 == 0)
					projectile.alpha += 3;
				if(projectile.alpha >= 250)
					projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++) {
				int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 39, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
				Main.dust[d].scale *= 0.42f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
		}

	}
}
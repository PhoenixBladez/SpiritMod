
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class FireShuriken : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Fiery Shuriken");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.thrown = true;
			projectile.timeLeft = 200;
			projectile.height = 24;
			projectile.width = 24;
			projectile.penetrate = 5;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.position, 0.4f, .12f, .036f);
			projectile.rotation += 0.3f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate {
					for(int i = 0; i < 40; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default, .8f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X += ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y += ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						if(Main.dust[num].position != projectile.Center) {
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
				});
			for(int num625 = 0; num625 < 3; num625++) {
				float scaleFactor10 = 0.33f;
				if(num625 == 1)
					scaleFactor10 = 0.66f;

				if(num625 == 2)
					scaleFactor10 = 1f;

				int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13AB6_cp_0 = Main.gore[num626];
				expr_13AB6_cp_0.velocity.X += 1f;
				Gore expr_13AD6_cp_0 = Main.gore[num626];
				expr_13AD6_cp_0.velocity.Y += 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13B79_cp_0 = Main.gore[num626];
				expr_13B79_cp_0.velocity.X -= 1f;
				Gore expr_13B99_cp_0 = Main.gore[num626];
				expr_13B99_cp_0.velocity.Y += 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13C3C_cp_0 = Main.gore[num626];
				expr_13C3C_cp_0.velocity.X += 1f;
				Gore expr_13C5C_cp_0 = Main.gore[num626];
				expr_13C5C_cp_0.velocity.Y -= 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13CFF_cp_0 = Main.gore[num626];
				expr_13CFF_cp_0.velocity.X -= 1f;
				Gore expr_13D1F_cp_0 = Main.gore[num626];
				expr_13D1F_cp_0.velocity.Y -= 1f;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if(projectile.penetrate <= 0)
				projectile.Kill();
			else {
				projectile.ai[0] += 0.1f;
				if(projectile.velocity.X != oldVelocity.X)
					projectile.velocity.X = -oldVelocity.X;

				if(projectile.velocity.Y != oldVelocity.Y)
					projectile.velocity.Y = -oldVelocity.Y;

				projectile.velocity *= 0.75f;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(8) == 0)
				target.AddBuff(BuffID.OnFire, 240);
		}
	}
}

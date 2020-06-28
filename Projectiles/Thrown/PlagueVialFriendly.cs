using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class PlagueVialFriendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Vial");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 14;

			projectile.aiStyle = 2;

			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 107);
			for(int i = 0; i < 30; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, -2f, 0, default(Color), 1.2f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				if(Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			int n = 2;
			int deviation = Main.rand.Next(0, 300);
			for(int i = 0; i < n; i++) {
				float rotation = MathHelper.ToRadians(270 / n * i + deviation);
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed.X *= Main.rand.NextFloat(1.5f, 3.5f);
				perturbedSpeed.Y *= Main.rand.NextFloat(1.5f, 3.5f);
				int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CursedDartFlame, projectile.damage / 2, 2, projectile.owner);
				Main.projectile[p].hostile = false;
				Main.projectile[p].friendly = true;
			}
		}
	}
}

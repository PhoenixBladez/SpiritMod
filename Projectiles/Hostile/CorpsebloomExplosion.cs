using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class CorpsebloomExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corpsebloom Explosion");
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.width = 60;
			projectile.height = 60;
			projectile.hide = true;
			projectile.alpha = 255;
			projectile.timeLeft = 6;
		}

		public override void AI()
		{

			Player player = Main.LocalPlayer;
			int distance1 = (int)Vector2.Distance(projectile.Center, player.Center);
			if (distance1 < 80) {
				player.AddBuff(BuffID.Poisoned, 600);
			}

		}
		public override void Kill(int timeLeft)
		{
			{
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 167, 0f, -2f, 0, Color.Purple, .8f);
					Main.dust[num].noLight = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-40, 41) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-40, 41) / 20) - 1.5f);
					if (Main.dust[num].position != projectile.Center) {
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 3f;
					}
				}
			}
		}
	}
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class WaterSpout : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Spout");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.width = 26;
			projectile.height = 145;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
		}
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			if (projectile.Hitbox.Intersects(player.Hitbox) && player.velocity.Y > -4f)
			{
				player.velocity.Y -= 3f;
			}
			float num1 = 6f;
			float num2 = (float)projectile.timeLeft / 60f;
			if((double)num2 < 1.0)
				num1 *= num2;

				for(int index3 = 0; index3 < 2; ++index3) {
					Vector2 vector2 = new Vector2(0.0f, -num1);
					vector2 = (vector2 * (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232)).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143, new Vector2());
					int index4 = Dust.NewDust(projectile.position, 4, projectile.height + 10, 33, 0.0f, 0.0f, 100, new Color(), 1f);
					Dust dust1 = Main.dust[index4];
					dust1.scale = (float)(1.0 + Main.rand.NextDouble() * 0.600000011920929);
					dust1.alpha = 0;
					Dust dust2 = dust1;
					dust2.velocity = dust2.velocity * 0.1f;
					Dust dust3 = dust1;
					dust3.position = dust3.position - new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0.0f);
					Dust dust4 = dust1;
					dust4.velocity = dust4.velocity + vector2;
					dust1.scale = 0.6f;
					dust1.fadeIn = dust1.scale + 0.2f;
				}
				if(projectile.timeLeft % 10 == 0) {
					float num3 = (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232);
					for(int index3 = 0; index3 < 9; ++index3) {
						Vector2 vector2 = new Vector2((float)(index3 - 4) / 5f, -num1 * num3);
						int index4 = Dust.NewDust(projectile.position, 4, projectile.height + 10, 33, 0.0f, 0.0f, 100, new Color(), 1f);
						Dust dust1 = Main.dust[index4];
						dust1.scale = (float)(0.699999988079071 + Main.rand.NextDouble() * 0.300000011920929);
						dust1.alpha = 0;
						Dust dust2 = dust1;
						dust2.velocity = dust2.velocity * 0.0f;
						Dust dust3 = dust1;
						dust3.position = dust3.position - new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0.0f);
						Dust dust4 = dust1;
						dust4.velocity = dust4.velocity + vector2;
						dust1.scale = 0.6f;
						dust1.fadeIn = dust1.scale + 0.2f;
					}
				}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				33, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
		}
	}
}
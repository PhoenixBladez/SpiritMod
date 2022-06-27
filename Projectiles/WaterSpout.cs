using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Dusts;
using Terraria.ID;

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
			Projectile.friendly = true;
			Projectile.width = 26;
			Projectile.height = 145;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 150;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.velocity.Y += .1f;
			Projectile.position.X = player.position.X + (player.velocity.X * 1.4f);
			if (Projectile.Hitbox.Intersects(player.Hitbox) && player.velocity.Y > -4f) {
				player.velocity.Y -= 3f;
			}
			float num1 = 6f;
			float num2 = (float)Projectile.timeLeft / 60f;
			if ((double)num2 < 1.0)
				num1 *= num2;

			for (int index3 = 0; index3 < 4; ++index3) {
				Vector2 vector2 = new Vector2(0.0f, -num1);
				vector2 = (vector2 * (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232)).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143, new Vector2());
				int index4 = Dust.NewDust(Projectile.position, 4, Projectile.height + 10, DustID.Water, 0.0f, 0.0f, 100, new Color(), 1f);
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
			if (Projectile.timeLeft % 10 == 0) {
				float num3 = (float)(0.850000023841858 + Main.rand.NextDouble() * 0.200000002980232);
				for (int index3 = 0; index3 < 9; ++index3) {
					Vector2 vector2 = new Vector2((float)(index3 - 4) / 5f, -num1 * num3);
					int index4 = Dust.NewDust(Projectile.position, 4, Projectile.height + 10, DustID.Water, 0.0f, 0.0f, 100, new Color(), 1f);
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Water, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
		}
	}
}
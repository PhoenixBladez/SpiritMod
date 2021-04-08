
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.Glitterfly
{
	public class StarSting : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Sting");
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.height = 20;
			projectile.width = 12;
			aiType = ProjectileID.DeathLaser;
			projectile.extraUpdates = 1;
		}

		int timer = 1;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 2; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 21, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 21, 0f, 0f, 100, default(Color), 3f);
			Main.dust[num624].velocity *= 0f;
			Main.dust[num624].scale *= 0.3f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}

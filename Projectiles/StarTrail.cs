
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class StarTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Trail");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;

			projectile.timeLeft = 30;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
		}
		public override void AI()
		{
			for (int i = 0; i < 6; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.Flare_Blue);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].noGravity = true;
			}

		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class SpiritRainHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 800;
			projectile.aiStyle = 1;
			projectile.alpha = 255;
			projectile.hide = true;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 29; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 2f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 2f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 187, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
			return true;
		}

	}
}
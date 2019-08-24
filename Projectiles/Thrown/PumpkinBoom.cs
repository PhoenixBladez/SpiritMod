using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class PumpkinBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpkin Grenade");
		}

		public override void SetDefaults()
		{
			///for reasons, I have to put a comment here.
			projectile.aiStyle = 16;
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.timeLeft = 8;
			projectile.penetrate = -1;
			projectile.width = 50;
			projectile.height = 50;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			for (int num623 = 0; num623 < 11; num623++)
				{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X, projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height, 189, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 1.4f;
				}
			Vector3 RGB = new Vector3(1f, 0.32f, 0f);
			float multiplier = 1;
			float max = 2.25f;
			float min = 1.0f;
			RGB *= multiplier;
			if (RGB.X > max)
			{
				multiplier = 0.5f;
			}
			if (RGB.X < min)
			{
				multiplier = 1.5f;
			}
			Lighting.AddLight(projectile.position, RGB.X, RGB.Y, RGB.Z);
		}

	}
}
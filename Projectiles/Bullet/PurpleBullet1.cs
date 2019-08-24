using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SpiritMod.Projectiles.Bullet
{

	public class PurpleBullet1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Bullet");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.hide = true;
			projectile.alpha = 255;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 173);
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}


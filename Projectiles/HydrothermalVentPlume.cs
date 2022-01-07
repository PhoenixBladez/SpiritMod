using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class HydrothermalVentPlume : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydrothermal Vent");
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.alpha = 255;

			projectile.hostile = false;
			projectile.friendly = true;

			projectile.penetrate = 4;
		}

		public override bool PreAI()
		{
			int side = Math.Sign(projectile.velocity.Y);
			if (projectile.timeLeft % 10 == 0)
			{
				if (Main.rand.NextBool(2))
					Item.NewItem(new Vector2(projectile.Center.X + Main.rand.Next(-10, 10), projectile.Center.Y + Main.rand.Next(-10, 10)), 0, 0, ModContent.ItemType<Items.Sets.CascadeSet.DeepCascadeShard>(), 1, false, 0, false);
			}
			if (projectile.ai[0] == 0f)
			{
				projectile.ai[1] += 1f;

				if (projectile.ai[1] >= 60f)
					projectile.Kill();

				for (int num1200 = 0; num1200 < 3; num1200++)
				{
					Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f)];
					dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
					dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = projectile.Center + new Vector2(0f, -projectile.height / 2f).RotatedBy(projectile.rotation, default) * 1.1f;
				}
			}
			float speedY = -2.5f * (0f - side);
			Dust dust81 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, Utils.SelectRandom(Main.rand, 6, 259, 31), 0f, speedY, 0, default, 1f)];
			dust81.alpha = 200;
			dust81.velocity *= new Vector2(0.3f, 2f);
			dust81.velocity.Y += 2 * side;
			dust81.scale += Main.rand.NextFloat();
			dust81.position = new Vector2(projectile.Center.X, projectile.Center.Y + projectile.height * 0.5f * (0f - side));
			return true;
		}
	}
}
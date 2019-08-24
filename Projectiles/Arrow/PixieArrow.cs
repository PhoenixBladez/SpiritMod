using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class PixieArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pixie Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 9;
			projectile.height = 17;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;

			projectile.ranged = true;
			projectile.friendly = true;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(3) == 0)
			{
				for (int i = 0; i < 1; i++)
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);

					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * (2* MathHelper.Pi);
					vel = vel.RotatedBy(rand);
					vel *= 3.5f;
					Projectile.NewProjectile(projectile.Center, vel,
						90, projectile.damage, projectile.knockBack, projectile.owner);
				}
			}
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 242);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
			{
				target.AddBuff(31, 120);
				target.AddBuff(mod.BuffType("HolyLight"), 120);
			}
		}

	}
}

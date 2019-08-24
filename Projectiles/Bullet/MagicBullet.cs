using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class MagicBullet : ModProjectile
	{
		int target;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;       //projectile width
			projectile.height = 2;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.ranged = true;         // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 2;      //how many npc will penetrate
			projectile.timeLeft = 210;   //how many time projectile projectile has before disepire // projectile light
			projectile.extraUpdates = 0;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.GoldCoin);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}

			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++)
			{
				if (Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1))
				{
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
					if (num25 < 500f)
					{
						flag25 = true;
						jim = index1;
					}
				}
			}

			if (flag25)
			{
				float num1 = 10f;
				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
				projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
			}
			else
			{
				projectile.velocity *= 0.95f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
			{
				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height,
						DustID.GoldCoin, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != projectile.Center)
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			});
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

	}
}

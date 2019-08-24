using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class GhastLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Laser");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
			projectile.height = 30;
			projectile.width = 8;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		int timer = 1;
		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.06F, 0.01F);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			{
				for (int i = 0; i < 2; i++)
				{
					float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x, y), 26, 26, 175, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num].alpha = projectile.alpha;
					Main.dust[num].position.X = x;
					Main.dust[num].position.Y = y;
					Main.dust[num].velocity *= 0f;
					Main.dust[num].noGravity = true;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
			{
				target.StrikeNPC(projectile.damage / 3 * 2, 0f, 0, crit);
				target.StrikeNPC(projectile.damage / 3 * 2, 0f, 0, crit);
				target.StrikeNPC(projectile.damage / 3 * 2, 0f, 0, crit);
			}
		}

	}
}

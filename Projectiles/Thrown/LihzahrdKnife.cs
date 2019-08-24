using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class LihzahrdKnife : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Dagger");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 16;

			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;

			projectile.thrown = true;
			projectile.friendly = true;

			projectile.penetrate = 5;
			projectile.timeLeft = 600;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.OnFire, 300, true);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("LihzahrdKnife"), 1, false, 0, false, false);

			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 6, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}

	}
}

using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BloodClump : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Clump");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 14;

			projectile.magic = true;
			projectile.friendly = true;
			projectile.tileCollide = false;

			projectile.penetrate = 3;
			projectile.timeLeft = 300;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 3; ++i)
			{
				Vector2 speed = -projectile.velocity + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
				speed *= 0.4F;
				Dust newDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 5, speed.X, speed.Y, 0, default(Color), 1.5f)];
				newDust.noGravity = true;
				newDust.position.X = projectile.Center.X;
				newDust.position.Y = projectile.Center.Y;
				newDust.position.X = newDust.position.X + (float)Main.rand.Next(-10, 11);
				newDust.position.Y = newDust.position.Y + (float)Main.rand.Next(-10, 11);
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("BCorrupt"), 180);
		}

	}
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class NebulaFlameProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Flame");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;

			projectile.thrown = true;
			projectile.friendly = true;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			projectile.ai[0]++;
			if (projectile.ai[0] >= 30)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.4f;
				projectile.velocity.X = projectile.velocity.X * 0.98f;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("NebulaFlame"), 180);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("NebulaFlame"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 242);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
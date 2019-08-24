using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class BladeOfNoah : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade Of Noah");
		}

		public override void SetDefaults()
		{
			projectile.width = 9;
			projectile.height = 9;
			projectile.penetrate = 5;
			projectile.friendly = true;
			projectile.thrown = true;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("BladeOfNoah"), 1, false, 0, false, false);
			
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 167);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(BuffID.Poisoned, 180);
		}

		public override bool PreAI()
		{
			projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

			if (projectile.ai[0] == 0)
				projectile.localAI[1] += 1f;

			if (projectile.localAI[1] >= 20f)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.4f;
				projectile.velocity.X = projectile.velocity.X * 0.98f;
			}
			else
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}

			if (projectile.velocity.Y > 16f)
				projectile.velocity.Y = 16f;

			return false;
		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}

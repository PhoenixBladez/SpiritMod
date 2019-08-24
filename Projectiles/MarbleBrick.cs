using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Projectiles
{
	public class MarbleBrick : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Brick");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.timeLeft = 300;
			projectile.penetrate = 3;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 236);
			}
		}

		public override bool PreAI()
		{
			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 1.005F;
			projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(9) == 0)
				target.AddBuff(mod.BuffType("Marbled"), 70, true);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X)
			{
				projectile.velocity.X = 0;
				projectile.timeLeft = 60;
			}

			return false;
		}
	}
}

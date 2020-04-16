using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BlazingWheelProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Wheel");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 1;
			projectile.alpha = 0;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 360;
			projectile.width = 32;
			projectile.height = 28;
			projectile.penetrate = 4;
		}

		public override void AI()
		{
			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 1.005F;
			projectile.rotation += .2f;
			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 4)
					projectile.frame = 0;
			}
			int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			Main.dust[d].noGravity = true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 200, true);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X)
				projectile.velocity.X = -oldVelocity.X * .95f;

			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			}
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
		}

	}
}

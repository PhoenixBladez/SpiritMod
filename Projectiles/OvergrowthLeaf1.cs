using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class OvergrowthLeaf1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gaea Leaf");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 16;
			projectile.aiStyle = 43;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(mod.BuffType("AcidBurn"), 240);

			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Venom, 240);

			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Poisoned, 240);
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 8)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame > 4)
			{
				projectile.frame = 0;
			}
			projectile.velocity.Y += projectile.ai[0];
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();
			else
			{
				projectile.ai[0] += 0.1f;
				if (projectile.velocity.X != oldVelocity.X)
					projectile.velocity.X = -oldVelocity.X;

				if (projectile.velocity.Y != oldVelocity.Y)
					projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 3, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
			}
		}
	}
}
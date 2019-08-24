using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class MadHatProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mad Hat");
			Main.projFrames[base.projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 38;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 50;
			projectile.timeLeft = 700;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation = 0;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 2)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}

			if (Main.rand.Next(5) == 1)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 180);
		}

	}
}

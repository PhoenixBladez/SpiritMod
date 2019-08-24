using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	public class InfernalWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Wave");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 55;

			projectile.hostile = true;
			projectile.tileCollide = false;

			projectile.penetrate = -1;
			projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
			}

			if (projectile.ai[0] == 0f || projectile.ai[0] == 2f)
			{
				projectile.scale += 0.01f;
				projectile.alpha -= 50;
				if (projectile.alpha <= 0)
				{
					projectile.ai[0] = 1f;
					projectile.alpha = 0;
				}
			}
			else if (projectile.ai[0] == 1f)
			{
				projectile.scale -= 0.01f;
				projectile.alpha += 50;
				if (projectile.alpha >= 255)
				{
					projectile.ai[0] = 2f;
					projectile.alpha = 255;
				}
			}

			projectile.frameCounter++;
			if (projectile.frameCounter >= 5)
				projectile.frame = (projectile.frame++) % Main.projFrames[projectile.type];
			projectile.rotation = projectile.velocity.ToRotation() + 4.71F;
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.OnFire, 300);
			else
				target.AddBuff(BuffID.OnFire, 600);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, projectile.alpha);
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}
	}
}

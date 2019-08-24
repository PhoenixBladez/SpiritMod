using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles
{
	public class FireChain1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Essence");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 240;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.magic = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localAI[1] += 1f;
			target.AddBuff(BuffID.OnFire, 280);
			projectile.velocity *= 0f;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if (projectile.localAI[1] <= 1.0)
			{
				int num3 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, num, num2, mod.ProjectileType("FireChain2"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num3].localAI[0] = projectile.whoAmI;
				return;
			}

			int num4 = (int)projectile.localAI[1];
			if (num4 <= 30)
			{
				if (num4 == 10 || num4 == 30)
				{
					num2--;
				}
			}
			else if (num4 == 50 || num4 == 70)
			{
				num2--;
			}

			if ((int)projectile.localAI[1] == 20)
			{
				int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain2"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 30)
			{
				int num6 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain2"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num6].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 40)
			{
				int num7 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("FireChain2"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num7].localAI[0] = (float)projectile.whoAmI;
			}

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
			projectile.velocity *= 0.95f;
		}
	}
}

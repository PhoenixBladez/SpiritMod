using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ExplodingWispShadow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.hide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			for (int I = 0; I < 8; I++)
			{
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
			target.StrikeNPC(projectile.damage, 0f, 0, crit);

			if (Main.rand.Next(4) == 1)
				target.AddBuff(BuffID.ShadowFlame, 200);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(3) == 1)
			{
				for (int i = 0; i < 6; ++i)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 92);
					Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
					targetDir.Normalize();
					targetDir *= 15;
					int y = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, mod.ProjectileType("ShadowPulse1"), 55, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
					Main.projectile[y].friendly = true;
					Main.projectile[y].hostile = false;
				}
			}
		}

	}
}
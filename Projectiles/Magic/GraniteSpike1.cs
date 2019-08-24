using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteSpike1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grant Spike");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 14;
			projectile.height = 26;
			projectile.penetrate = 3;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height, 187);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].noGravity = true;
			projectile.velocity.Y += projectile.ai[0];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 2;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				187, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 4; i++)
			{
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, projectile.knockBack, projectile.owner);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 4f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (crit)
				target.AddBuff(mod.BuffType("EnergyFlux"), 240);
		}

	}
}
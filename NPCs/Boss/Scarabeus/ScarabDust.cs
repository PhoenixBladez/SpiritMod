using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class ScarabDust : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Dust");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 4;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].noGravity = true;
			projectile.velocity.Y += projectile.ai[0];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

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
			Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, DustID.GoldCoin, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 1; i++)
			{
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("ScarabDust1"), 13, projectile.owner, 0, 0f);

				Main.projectile[proj].velocity *= 3.5f;
			}
		}

	}
}
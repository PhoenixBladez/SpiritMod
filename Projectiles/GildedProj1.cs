using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class GildedProj1 : ModProjectile
	{
		private int lastFrame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Beam");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
			projectile.tileCollide = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(projectile.damage, 0f, 0, crit);
			target.StrikeNPC(projectile.damage / 2, 0f, 0, crit);
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = true;

			return true;
		}

		public override void AI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
				projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
			}
			else if (lastFrame > 0)
			{
				lastFrame++;
				if (lastFrame > 2)
					projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			lastFrame = 1;
			projectile.tileCollide = false;
			return false;
		}

	}
}
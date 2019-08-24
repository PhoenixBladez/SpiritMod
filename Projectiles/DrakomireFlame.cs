using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class DrakomireFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakomire Flame");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.hide = true;
		}

		public override bool PreAI()
		{
			if (projectile.velocity.Y < 8f)
				projectile.velocity.Y += 0.1f;


			Vector2 next = projectile.position + projectile.velocity;
			Tile inside = Main.tile[((int)projectile.position.X + (projectile.width >> 1)) >> 4, ((int)projectile.position.Y + (projectile.height >> 1)) >> 4];
			if (inside.active() && Main.tileSolid[inside.type])
			{
				projectile.position.Y -= 16f;
				return false;
			}

			if (Collision.WetCollision(next, projectile.width, projectile.height))
			{
				if (Main.player[projectile.owner].waterWalk)
				{
					projectile.velocity.Y = 0f;
				}
				else
				{
					projectile.timeLeft = 0;
				}
			}

			int num = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height, 6, (float)Main.rand.Next(-3, 4), (float)Main.rand.Next(-3, 4), 100, default(Color), 1f);
			Dust dust = Main.dust[num];
			dust.position.X = dust.position.X - 2f;
			dust.position.Y = dust.position.Y + 2f;
			dust.scale += (float)Main.rand.Next(50) * 0.01f;
			dust.noGravity = true;
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 60);
		}
	}
}

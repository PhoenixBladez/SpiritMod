using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class AmberSlasher : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amber Slash");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.penetrate = 2;
			projectile.friendly = true;
			projectile.thrown = true;
		}

		public override bool PreAI()
		{
			projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

			if (projectile.ai[0] == 0)
				projectile.localAI[1] += 1f;

			if (projectile.localAI[1] >= 20f)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.4f;
				projectile.velocity.X = projectile.velocity.X * 0.98f;
			}
			else
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}

			if (projectile.velocity.Y > 16f)
				projectile.velocity.Y = 16f;

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.ai[0] == 0)
			{
				int randX = target.position.X > Main.player[projectile.owner].position.X ? 1 : -1;
				int randY = Main.rand.Next(2) == 0 ? -1 : 1;

				Vector2 randPos = target.Center + new Vector2(randX * Main.rand.Next(100, 151), randY * 400);
				Vector2 dir = target.Center - randPos;
				dir.Normalize();
				dir *= 16;
				int newProj = Projectile.NewProjectile(randPos.X, randPos.Y, dir.X, dir.Y, mod.ProjectileType("AmberSlasher"), projectile.damage, projectile.knockBack, projectile.owner, 1);
				Main.projectile[newProj].tileCollide = false;
			}
			else
				projectile.tileCollide = true;

		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			for (int num424 = 0; num424 < 10; num424++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 1, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
			}
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("AmberSlasher"), 1, false, 0, false, false);

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}

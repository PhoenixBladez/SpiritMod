using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class VortexSlasherProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Slasher");
		}

		public override void SetDefaults()
		{
			projectile.width = 13;
			projectile.height = 18;

			projectile.thrown = true;
			projectile.friendly = true;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.immune[projectile.owner] > 0)
				return false;

			return null;
		}

		public override bool PreAI()
		{
			projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

			if (projectile.ai[0] == 0)
				projectile.localAI[1] += 1f;
			else
				projectile.alpha += 1;
			if (projectile.alpha > 255)
				projectile.Kill();

			if (projectile.localAI[1] >= 30f)
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
			target.immune[projectile.owner] = 6;
			if (projectile.ai[0] == 0)
			{
				int randX = target.position.X > Main.player[projectile.owner].position.X ? 1 : -1;
				int randY = Main.rand.Next(2) == 0 ? -1 : 1;

				Vector2 randPos = target.Center + new Vector2(randX * Main.rand.Next(100, 151), randY * 400);
				Vector2 dir = target.Center - randPos;
				dir.Normalize();
				dir *= 12;
				int newProj = Projectile.NewProjectile(randPos.X, randPos.Y, dir.X, dir.Y, mod.ProjectileType("VortexSlasherProjectile"), projectile.damage, projectile.knockBack, projectile.owner, 1);
				Main.projectile[newProj].tileCollide = false;
				Main.projectile[newProj].penetrate = 8;
				Main.projectile[newProj].timeLeft = 40;
				Main.projectile[newProj].extraUpdates = 1;
			}
			else
			{
				projectile.tileCollide = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("VortexSlasher"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 217);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}
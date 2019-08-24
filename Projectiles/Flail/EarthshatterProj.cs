using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class EarthshatterProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthshaker");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 54;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		int timer = 0;
		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(projectile.whoAmI);
			timer++;
			if (timer >= 40)
			{
				for (int i = 0; i < 1; ++i)
				{
					Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
					targetDir.Normalize();
					targetDir *= 3;
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, mod.ProjectileType("EarthshakerProj"), projectile.damage, projectile.knockBack, projectile.owner);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					projectile.Kill();
				}
				timer = 0;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/Earthshatter_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}

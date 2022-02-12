using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System;
using Terraria;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class VoidPortal : ModProjectile
	{
		internal int connectedWhoAmI = -1;

		protected int teleported = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Void Portal");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(80, 200);
			projectile.hostile = true;
			projectile.timeLeft = 60;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.velocity.Y = (float)Math.Sin(projectile.ai[0]++ * 0.005f) * 0.02f;

			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				Projectile proj = Main.projectile[i];

				bool validProj = i != projectile.whoAmI && proj.active && (proj.friendly || proj.type == ModContent.ProjectileType<VoidProjectile>());
				if (validProj)
				{
					if (projectile.DistanceSQ(proj.Center) < 80 * 80 && !proj.GetGlobalProjectile<VoidGlobalProjectile>().hasTeleported)
					{
						TeleportProjectile(i);
						teleported = 10;
					}
				}
			}
		}

		public void TeleportProjectile(int ind)
		{
			Projectile teleported = Main.projectile[ind];
			Projectile other = Main.projectile[connectedWhoAmI];
			var otherPortal = other.modProjectile as VoidPortal;

			otherPortal.teleported = 10; //Set the tp timer
			teleported.Center = other.Center; //Teleport the projectile
			teleported.GetGlobalProjectile<VoidGlobalProjectile>().hasTeleported = true; //The projectile has teleported...
			teleported.GetGlobalProjectile<VoidGlobalProjectile>().teleportWhoAmI = connectedWhoAmI; //...to this portal
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;
	}
}
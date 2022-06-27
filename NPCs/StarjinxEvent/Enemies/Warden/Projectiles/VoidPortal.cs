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
			Projectile.Size = new Vector2(80, 200);
			Projectile.hostile = true;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.velocity.Y = (float)Math.Sin(Projectile.ai[0]++ * 0.005f) * 0.02f;

			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				Projectile proj = Main.projectile[i];

				bool validProj = i != Projectile.whoAmI && proj.active && (proj.friendly || proj.type == ModContent.ProjectileType<VoidProjectile>());
				if (validProj)
				{
					if (Projectile.DistanceSQ(proj.Center) < 80 * 80 && !proj.GetGlobalProjectile<VoidGlobalProjectile>().hasTeleported)
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
			var otherPortal = other.ModProjectile as VoidPortal;

			otherPortal.teleported = 10; //Set the tp timer
			teleported.Center = other.Center; //Teleport the projectile
			teleported.GetGlobalProjectile<VoidGlobalProjectile>().hasTeleported = true; //The projectile has teleported...
			teleported.GetGlobalProjectile<VoidGlobalProjectile>().teleportWhoAmI = connectedWhoAmI; //...to this portal
			teleported.GetGlobalProjectile<VoidGlobalProjectile>().OnTeleport(teleported); //And run teleport code.
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;
	}
}
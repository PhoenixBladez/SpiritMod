using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory.MoonlightSack;
using Terraria;
using Terraria.ModLoader;

namespace WhirlingWorlds.Items.Accessory.Moonlight_Sack
{
	public class Moonlight_Sack_Player : ModPlayer
	{
		public bool isEquipped;
		public int projectileTimer = 0;

		public override void ResetEffects() => isEquipped = false;

		public override void PostUpdate()
		{
			if (isEquipped)
			{
				projectileTimer++;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (player.DistanceSQ(projectile.Center) <= 300 * 300 && projectile.owner == player.whoAmI && projectile.active && projectile.minion && projectile.type != ModContent.ProjectileType<Moonlight_Sack_Lightning>())
					{		
						if (projectileTimer % 5 == 0)
						{
							Vector2 vel = Vector2.Normalize(projectile.Center - projectile.Center) * 8f;
							int p = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, ModContent.ProjectileType<Moonlight_Sack_Lightning>(), (int)(12*player.minionDamage), 1f, player.whoAmI, 0.0f, 0.0f);
							Main.projectile[p].ai[0] = projectile.whoAmI;
						}
					}
				}
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace WhirlingWorlds.Items.Accessory.Moonlight_Sack
{
	public class Moonlight_Sack_Player : ModPlayer
	{
		public bool isEquipped;
		public int projectileTimer = 0;
		public override void ResetEffects()
		{
			isEquipped = false;
		}
		
		public override void PostUpdate()
		{
			if (isEquipped)
			{
				projectileTimer++;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile projectile = Main.projectile[i];
					if ((double)Vector2.Distance(player.Center, projectile.Center) <= (double)300f && projectile.owner == player.whoAmI && projectile.active && projectile.minion && projectile.type != mod.ProjectileType("Moonlight_Sack_Lightning"))
					{		
						if (projectileTimer % 5 == 0)
						{
							int pickedProjectile = mod.ProjectileType("Moonlight_Sack_Lightning");
							Vector2 vector2_1 = new Vector2((float) player.Center.X, (float) player.Center.Y);
							Vector2 vector2_2 = Vector2.Normalize(vector2_1 - projectile.Center) * 8f;
							int p = Projectile.NewProjectile(player.Center.X, player.Center.Y, vector2_2.X, vector2_2.Y, pickedProjectile, (int)(12*player.minionDamage), 1f, player.whoAmI, 0.0f, 0.0f);
							Main.projectile[p].ai[0] = projectile.whoAmI;
						}
					}
				}
			}
		}
	}
}

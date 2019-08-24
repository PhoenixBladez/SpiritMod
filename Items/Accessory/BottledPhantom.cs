using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class BottledPhantom : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Phantom");
			Tooltip.SetDefault("A spectral entity guides you");
		}


		private int proj2;
		public override void SetDefaults()
		{
            item.width = 18;
            item.height = 18;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 8;

			item.accessory = true;
			item.defense = 0;
		}
   
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddIngredient(null, "SpiritBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
		public override void UpdateAccessory(Player player, bool hideVisual)
		{   
		Projectile newProj2 = Main.projectile[proj2];
			player.GetModPlayer<MyPlayer>(mod).Phantom = true;
			if (newProj2.type == mod.ProjectileType("PhantomMinion"))
			{
			}
			else {
				proj2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("PhantomMinion"), 66, 1, player.whoAmI);
				newProj2 = Main.projectile[proj2];
			}
			if (newProj2.active == false)
			{
				proj2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("PhantomMinion"), 66, 1, player.whoAmI);
				newProj2 = Main.projectile[proj2];
			}
		}
	}
}

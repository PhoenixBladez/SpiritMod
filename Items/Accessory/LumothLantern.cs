using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Balloon)]
    public class LumothLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminous Lantern");
            Tooltip.SetDefault("Provides bright light\nWorks in the vanity slot, but less effectively\n'Adventure into the deepest caverns with a trusty lightsource!'");

        }


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 38;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                Lighting.AddLight(player.position, 1.25f, 1.2f, 1.2f);
            }
        }
		public override void UpdateVanity(Player player, EquipType type)
		  {
                Lighting.AddLight(player.position, .35f, .35f, .35f);
          }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Brightbulb"), 1);
			recipe.AddIngredient(ItemID.SilverBar, 5);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(mod.ItemType("Brightbulb"), 1);
			recipe1.AddIngredient(ItemID.TungstenBar, 5);
			recipe1.AddIngredient(ItemID.Wood, 20);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
    }
}

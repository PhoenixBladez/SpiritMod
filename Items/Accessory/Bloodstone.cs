using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class Bloodstone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstone");
            Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nIncreases life regeneration slightly");

        }


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = 3;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.lifeRegen += 3;
            player.GetModPlayer<MyPlayer>(mod).Ward = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodWard", 1);
            recipe.AddIngredient(null, "VitalityStone", 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

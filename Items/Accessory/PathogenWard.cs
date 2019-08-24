using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class PathogenWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathogen Ward");
            Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nIncreases life regeneration slightly\nProvides immunity to the 'Poisoned' buff\nIncreases maximum life by 10 at the cost of 1 defense");

        }


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.rare = 4;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.lifeRegen += 3;
            player.GetModPlayer<MyPlayer>(mod).Ward = true;
            player.statLifeMax2 += 10;
            player.statDefense -= 1;
            player.buffImmune[BuffID.Poisoned] = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
          //  recipe.AddIngredient(null, "BloodWard", 1);
            recipe.AddIngredient(null, "Bloodstone", 1);
            recipe.AddIngredient(null, "PMicrobe", 1);
            recipe.AddIngredient(ItemID.Bezoar, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

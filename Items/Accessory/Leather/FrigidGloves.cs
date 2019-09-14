using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class FrigidGloves : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Wraps");
			Tooltip.SetDefault("Critical strikes may inflict Frostborn\nIncreases weapon speed by 3% for every nearby enemy\nThis effect stacks up to four times");
		}
        public override void SetDefaults()
        {
            item.width = 26;
			item.height = 34;
            item.rare = 1;
            item.value = 1200;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetSpiritPlayer().frigidGloves = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LeatherGlove"), 1);
            recipe.AddIngredient(null, "FrigidFragment", 6);
            recipe.AddRecipeGroup("EvilMaterial1", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

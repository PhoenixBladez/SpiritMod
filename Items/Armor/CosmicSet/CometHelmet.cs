using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CosmicSet
{
    [AutoloadEquip(EquipType.Head)]
    public class CometHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Helm");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Item.sellPrice(0, 3, 50, 0);
            item.rare = ItemRarityID.Red;

            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex, 3);
			recipe.AddIngredient(ItemID.FragmentNebula, 3);
			recipe.AddIngredient(ItemID.FragmentSolar, 3);
			recipe.AddIngredient(ItemID.FragmentStardust, 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
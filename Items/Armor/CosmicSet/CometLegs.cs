using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CosmicSet
{
    [AutoloadEquip(EquipType.Legs)]
    public class CometLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Greaves");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 25, 0);
            item.rare = ItemRarityID.Red;

            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex, 4);
			recipe.AddIngredient(ItemID.FragmentNebula, 4);
			recipe.AddIngredient(ItemID.FragmentSolar, 4);
			recipe.AddIngredient(ItemID.FragmentStardust, 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

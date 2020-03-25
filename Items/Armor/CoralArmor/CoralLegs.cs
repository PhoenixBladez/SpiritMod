using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CoralArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CoralLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coral Greaves");
		}
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.rare = 1;
            item.value = Terraria.Item.sellPrice(0, 0, 9, 0);
            item.defense = 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 12);
            recipe.AddIngredient(ItemID.Seashell, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

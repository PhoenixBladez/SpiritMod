using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SpiritWood
{
    [AutoloadEquip(EquipType.Legs)]
    public class SpiritWoodLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Greaves");

        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 0;
            item.rare = 0;
            item.defense = 3;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritWoodItem", 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

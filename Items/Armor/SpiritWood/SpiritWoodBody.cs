using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SpiritWood
{
    [AutoloadEquip(EquipType.Body)]
    public class SpiritWoodBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Chestplate");

        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 0;
            item.rare = 0;
            item.defense = 5;
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

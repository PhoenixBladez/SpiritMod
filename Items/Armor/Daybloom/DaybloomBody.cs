using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Daybloom
{
    [AutoloadEquip(EquipType.Body)]
    public class DaybloomBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybloom Garb");
			Tooltip.SetDefault("Increases magic damage by 4%");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 0000;
            item.rare = 0;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Daybloom, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

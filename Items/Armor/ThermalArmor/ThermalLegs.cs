using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ThermalArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ThermalLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Greaves");
			Tooltip.SetDefault("Increases movement speed by 7% and melee speed by 5%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.defense = 14;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += .07f;
            player.meleeSpeed += .05f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ThermiteBar", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
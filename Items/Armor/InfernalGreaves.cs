using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class InfernalGreaves : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pain Monger's Greaves");
			Tooltip.SetDefault("Increases magic critical chance by 7% and reduces mana consumption by 10%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.rare = 5;
            item.value = 42000;

            item.defense = 9;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 7;
            player.manaCost -= 0.10f;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "InfernalAppendage", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
}

using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class SpiritBodyArmor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Body Armor");
            Tooltip.SetDefault("Increases melee damage and critical strike chance by 10%, as well as reducing damage taken by 10%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = 50000;
            item.rare = 5;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {

            player.meleeDamage += 0.10f;
            player.meleeCrit += 10;

            player.endurance = .10f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBar", 20);
            recipe.AddIngredient(null, "SoulShred", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
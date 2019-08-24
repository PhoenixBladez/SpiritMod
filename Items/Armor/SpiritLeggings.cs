using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SpiritLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Leggings");
            Tooltip.SetDefault("10% increased melee speed");

        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.value = 30000;
            item.rare = 5;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.10f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBar", 12);
            recipe.AddIngredient(null, "SoulShred", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
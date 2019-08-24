using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.LeatherArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class LeatherBoots : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Boots");
			Tooltip.SetDefault("Increases ranged damage by 2% and increases movement speed by 3%");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 100;
            item.rare = 1;
            item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.03F;
            player.rangedDamage += 0.02F;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "OldLeather", 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

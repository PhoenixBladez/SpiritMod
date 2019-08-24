using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GoreArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class IchorPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Platemail");
			Tooltip.SetDefault("Increases melee damage by 7% and melee speed by 6%");
		}

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 70, 0);
            item.rare = 4;

            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.07f;

            player.meleeSpeed += 0.06f; ;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FleshClump", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

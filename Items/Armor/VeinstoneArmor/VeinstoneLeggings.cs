using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.VeinstoneArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class VeinstoneLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Veinstone Leggings");
			Tooltip.SetDefault("Increases invincibility time and critical strike chance by 6%");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;

            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.longInvince = true;

            player.magicCrit += 6;
            player.meleeCrit += 6;
            player.rangedCrit += 6;
            player.thrownCrit += 6;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Veinstone", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

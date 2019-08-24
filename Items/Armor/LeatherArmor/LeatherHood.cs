using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.LeatherArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class LeatherHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Hood");
			Tooltip.SetDefault("Increases ranged damage by 3%");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 12;
            item.value = 100;
            item.rare = 1;

            item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("LeatherPlate") && legs.type == mod.ItemType("LeatherBoots");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases ranged damage by 3%";
            player.rangedDamage += 0.03F;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.02F;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "OldLeather", 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class RunicHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Hood");
			Tooltip.SetDefault("Increases magic damage by 15% and movement speed by 5%");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = 70000;
            item.rare = 5;
            item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RunicPlate") && legs.type == mod.ItemType("RunicGreaves");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Leave behind dangerous explosive runes";
            player.GetModPlayer<MyPlayer>(mod).runicSet = true;
		}

        public override void UpdateEquip(Player player)
        {
            player.magicDamage = 1.12f;
            player.moveSpeed += 1.05f;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Rune", 8);
            recipe.AddIngredient(null, "SoulShred", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
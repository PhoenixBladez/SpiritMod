using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ShadowGauntlet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Gauntlet");
			Tooltip.SetDefault("Melee attacks have a chance to inflict Shadowflame\nIncreases melee damage and melee speed by 10%\nIncreases melee knockback");
		}
        public override void SetDefaults()
        {
            item.width = 16;
			item.height = 16;
            item.rare = 8;
            item.value = 150000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).shadowGauntlet = true;
            player.meleeSpeed += 0.1f;
            player.meleeDamage += 0.1f;
            player.kbGlove = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FireGauntlet);
            recipe.AddIngredient(mod.ItemType("DuskStone"), 18);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WindArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class WindArmorBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wind God's Chestplate");
			Tooltip.SetDefault("Increases summon damage by 15%\nIncreases maximum amount of minions by 1");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.defense = 13;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.15f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WorshipCrystal", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
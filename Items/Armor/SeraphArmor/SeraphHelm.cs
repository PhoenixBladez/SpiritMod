using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class SeraphHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph Helmet");
			 Tooltip.SetDefault("Increases minion damage by 15% \nIncreases your maximum number of minions");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 10;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SeraphArmor") && legs.type == mod.ItemType("SeraphLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "Minions will sometimes shoot astral flares at enemies.";

            player.GetModPlayer<MyPlayer>(mod).astralSet = true;
        }
        public override void UpdateEquip(Player player)
        {
			player.minionDamage += 0.15f;
			player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
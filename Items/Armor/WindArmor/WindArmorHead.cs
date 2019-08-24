using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WindArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class WindArmorHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wind God's Headdress");
			Tooltip.SetDefault("Increases summon damage by 12% and movement speed by 10%\nIncreases maximum amount of minions by 2");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.defense = 15;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("WindArmorBody") && legs.type == mod.ItemType("WindArmorLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "Movement speed increases drastically as health wanes\nMinion attacks may release a fountain of souls from hit foes";
            player.GetModPlayer<MyPlayer>(mod).windSet = true;
            float runBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 5f;
            player.maxRunSpeed += (int)runBoost;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.12f;
            player.maxRunSpeed += 0.10f;
            player.maxMinions += 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WorshipCrystal", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
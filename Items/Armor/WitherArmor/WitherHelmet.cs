using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitherArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class WitherHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither Visor");
			Tooltip.SetDefault("Increases damage and critical strike chance by 10%");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.defense = 17;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("WitherPlate") && legs.type == mod.ItemType("WitherLeggings");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "A mass of wither energy follows you, attacking nearby enemies";
            player.GetModPlayer<MyPlayer>(mod).witherSet = true;
        }
        public override void UpdateEquip(Player player)
        {


			            player.magicDamage += 0.1f;
            player.meleeDamage += 0.10f;
            player.thrownDamage += 0.10f;
            player.rangedDamage += 0.10f;
			player.minionDamage += 0.10f;
            player.maxRunSpeed += 0.1f;
			
            player.magicCrit += 10;
            player.meleeCrit += 10;
            player.rangedCrit += 10;
            player.thrownCrit += 10;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightmareFuel", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
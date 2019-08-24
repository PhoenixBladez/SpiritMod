using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.QuicksilverArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class QuicksilverHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Mask");
			Tooltip.SetDefault("Increases ranged damage by 10%\nIncreases critical strike chance by 6%\nIncreases throwing velocity by 10%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.defense = 19;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("QuicksilverBody") && legs.type == mod.ItemType("QuicksilverLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "Increases damage by 12%\nPressing the 'Armor Bonus' hotkey will cause your cursor to release multiple damaging quicksilver droplets\nIf these droplets hit foes, they will regenerate some of the player's life\n30 second cooldown";
            player.minionDamage += 0.11f;
            player.thrownDamage += 0.11f;
            player.meleeDamage += 0.11f;
            player.rangedDamage += 0.11f;
            player.magicDamage += 0.11f;
            player.GetModPlayer<MyPlayer>(mod).quickSilverSet = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownCrit += 6;
            player.rangedCrit += 6;
            player.meleeCrit += 6;
            player.magicCrit += 6;
            player.rangedDamage += 0.1f;
            player.thrownVelocity += .1f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 11);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
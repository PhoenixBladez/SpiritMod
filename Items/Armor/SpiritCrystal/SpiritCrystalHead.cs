using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SpiritCrystal
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritCrystalHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Crystal Guard");
			Tooltip.SetDefault("Increases maximum number of minions by 1\nIncreases minion damage by 6%");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.defense = 9;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SpiritCrystalBody") && legs.type == mod.ItemType("SpiritCrystalLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {            
            player.setBonus = "Minion attacks may inflict 'Soul Burn' to hit foes\nSummons a stationary hedron to shoot bolts at foes";
            player.GetModPlayer<MyPlayer>(mod).crystalSet = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.06f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritCrystal", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
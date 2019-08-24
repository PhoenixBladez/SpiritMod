using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class DepthHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Walker's Helmet");
            Tooltip.SetDefault("Increases melee critical strike chance by 10% and minion damage by 10%\nIncreases maximum number of minions by 1");

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 5;
            item.defense = 9;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 10;
            player.minionDamage += 0.1f;
            player.maxMinions += 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DepthChest") && legs.type == mod.ItemType("DepthGreaves");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "Press the 'Armor Bonus' to release multiple mechanical shark minions that home onto enemies \n30 second cooldown";
            player.GetModPlayer<MyPlayer>(mod).depthSet = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
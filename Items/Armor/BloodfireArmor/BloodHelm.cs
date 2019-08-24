using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodfireArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodfire Mask");
            Tooltip.SetDefault("Increases magic damage and critical strike chance by 6%\nIncreases maximum mana by 30");

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 11000;
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.magicDamage += .06f;
            player.magicCrit += 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BloodPlate") && legs.type == mod.ItemType("BloodGreaves");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "Magic attacks may inflict 'Blood Corruption' \n Magic attacks may rarely steal life";
            player.GetModPlayer<MyPlayer>(mod).bloodfireSet = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
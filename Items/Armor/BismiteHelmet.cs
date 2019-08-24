using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class BismiteHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Helmet");
            Tooltip.SetDefault("Increases critical strike chance by 2%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 1;
            item.defense = 4;
        }
         public override void UpdateEquip(Player player)
        {
            player.magicCrit += 2;
            player.meleeCrit += 2;
            player.thrownCrit += 2;
            player.rangedCrit += 2;
        }
			public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "+5% Critical chance"; 
            player.magicCrit += 5;
            player.meleeCrit += 5;
            player.thrownCrit += 5;
            player.rangedCrit += 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BismiteChestplate") && legs.type == mod.ItemType("BismiteLeggings");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

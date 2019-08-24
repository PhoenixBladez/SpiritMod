using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SilkHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silk Hood");
			Tooltip.SetDefault("Increases minion damage by 4%");
		}


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = 2000;
            item.rare = 1;
            item.defense = 2;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.04f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SilkRobe") && legs.type == mod.ItemType("SilkLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
  
            player.setBonus = "Increases minion damage and knockback by 5%";
            player.minionKB += 0.05f;
            player.minionDamage += 0.05f;

        }
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddRecipeGroup("GoldBars");
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
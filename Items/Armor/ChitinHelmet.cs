using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChitinHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Faceguard");
            Tooltip.SetDefault("Increases max number of minions by 1\nIncreases minion damage and knockback by 5%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 18000;
            item.rare = 2;
            item.defense = 4;
        }
         public override void UpdateEquip(Player player)
        {
			 player.maxMinions += 1;
			player.minionDamage +=0.05f;
            player.minionKB += 0.05f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ChitinChestplate") && legs.type == mod.ItemType("ChitinLeggings");
        }
		 public override void UpdateArmorSet(Player player)
        {
           player.setBonus = "Increases minion damage by 7%";
            player.minionDamage += 0.07f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Chitin", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SeraphLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph Greaves");
			 Tooltip.SetDefault("Increases minion damage by 8% \nIncreases movement speed by 17%\nIncreases your maximum number of minions \nReduces mana cost by 17%");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
			player.minionDamage += 0.10f;
			player.moveSpeed += 0.17f;
			player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

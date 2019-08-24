using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class SeraphArmor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph Breastplate");
            Tooltip.SetDefault("Increases minion damage by 10% \nIncreases your maximum number of minions \nReduces mana cost by 17%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 60000;
            item.rare = 5;
            item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
			player.manaCost -= .17f;
			player.minionDamage += 0.10f;
			player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

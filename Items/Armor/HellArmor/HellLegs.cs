using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HellArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class  HellLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolent Greaves");
			Tooltip.SetDefault("Reduces ammo consumption by 25%\nIncreases movement speed by 10%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 6;
            item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.ammoCost75 = true;
            player.moveSpeed += 0.1f;

        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryEssence", 18);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
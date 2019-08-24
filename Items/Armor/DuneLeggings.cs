using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DuneLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Leggings");
			Tooltip.SetDefault("Increases throwing damage and crit chance by 15% and reduces throwing consumption by 33%");
		}


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 56000;
            item.rare = 6;
            item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage+= 0.15f;
			            player.thrownCrit+= 15;
            player.thrownCost33 = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence", 18);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
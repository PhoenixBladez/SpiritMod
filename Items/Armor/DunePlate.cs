using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DunePlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Plate");
			Tooltip.SetDefault("Increases throwing velocity by 15% and throwing damage by 18%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 66000;
            item.rare = 6;
            item.defense = 17;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += .18f;
            player.thrownVelocity += 0.15f;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence", 20);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
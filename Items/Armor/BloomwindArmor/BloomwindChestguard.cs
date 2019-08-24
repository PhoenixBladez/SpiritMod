using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloomwindArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BloomwindChestguard : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomwind Chestguard");
			Tooltip.SetDefault("Increases maximum minions by 1 and increases minion damage by 15%");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = 60000;
            item.rare = 6;

            item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
            player.minionDamage += 0.15F;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PrimevalEssence", 16);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
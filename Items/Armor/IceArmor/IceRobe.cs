using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IceArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class  IceRobe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Robes");
			Tooltip.SetDefault("Increases magic damage by 10% and magic critical strike chance by 5%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 86000;
            item.rare = 6;
            item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.1f;
            player.magicCrit += 5;

        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IcyEssence", 18);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
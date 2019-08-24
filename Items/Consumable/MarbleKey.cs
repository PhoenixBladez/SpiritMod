using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class MarbleKey : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Key");
            Tooltip.SetDefault("'Charged with the might of ancient civilizations'");

        }


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = 0;
            item.maxStack = 99;
            item.value = 100;
            item.useStyle = 4;
            item.useTime = item.useAnimation = 19;

            item.noMelee = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 8);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddIngredient(ItemID.SoulofLight, 4);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

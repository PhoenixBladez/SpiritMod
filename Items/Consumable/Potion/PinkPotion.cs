using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class PinkPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jump Potion");
			Tooltip.SetDefault("Greatly increases jump height");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 5;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("PinkPotionBuff");
            item.buffTime = 7200;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PinkGel, 1);
            recipe.AddIngredient(ItemID.Gel, 2); 
            recipe.AddIngredient(ItemID.Daybloom, 1); 
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class RunePotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runescribe Potion");
			Tooltip.SetDefault("Magic attacks may cause enemies to erupt into runes\nIncreases magic damage by 5%");
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

            item.buffType = mod.BuffType("RunePotionBuff");
            item.buffTime = 10800;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SoulBloom", 1);
            recipe.AddIngredient(ItemID.Fireblossom, 1);
            recipe.AddIngredient(null, "Rune", 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

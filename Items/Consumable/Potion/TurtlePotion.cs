using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class TurtlePotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steadfast Potion");
			Tooltip.SetDefault("Increases defense as health wanes\nReduces damage taken by 5%");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 7;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("TurtlePotionBuff");
            item.buffTime = 14400;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SoulBloom", 1);
            recipe.AddIngredient(ItemID.Shiverthorn, 1);
            recipe.AddIngredient(ItemID.TurtleShell, 1);
            recipe.AddIngredient(ItemID.IronOre, 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class StarPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starburn Potion");
			Tooltip.SetDefault("Critical hits may cause stars to fall from the sky\nIncreases ranged damage and critical strike chance by 4% when moving");
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

            item.buffType = mod.BuffType("StarPotionBuff");
            item.buffTime = 10800;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SoulBloom", 1);
            recipe.AddIngredient(ItemID.Feather, 2);
            recipe.AddIngredient(null, "StarPiece", 1);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

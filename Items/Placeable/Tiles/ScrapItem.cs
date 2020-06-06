using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
    public class ScrapItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Salvaged Scrap Block");
        }


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ScrapTile");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SpaceJunkItem"), 2);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this, 30);
            recipe.AddRecipe();
        }
    }
}
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tiles.Furniture;
namespace SpiritMod.Items.Placeable.Furniture
{
    public class FishingPainting : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Moonlight Deluge");
            Tooltip.SetDefault("'P. Otat'");
        }

        public override void SetDefaults() {
            item.width = 36;
            item.height = 28;
            item.value = item.value = Terraria.Item.buyPrice(0, 1, 40, 10);
            item.rare = 0;

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<FishingPaintingTile>();
        }
        
    }
}
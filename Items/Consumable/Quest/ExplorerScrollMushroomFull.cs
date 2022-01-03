
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ExplorerScrollMushroomFull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Completed Surveyor's Scroll");
		}


		public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;
            item.width = 30;
			item.height = 20;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<Tiles.Furniture.Paintings.MushroomMap>();
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "A nearby Glowing Mushroom Biome has been charted!");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}


using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ExplorerScrollGraniteFull : ModItem
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
			item.value = Item.sellPrice(0, 2, 0, 0);
            item.createTile = ModContent.TileType<Tiles.Furniture.Paintings.GraniteMap>();
            item.rare = -11;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "A nearby Granite Cavern has been charted!");
			line1.overrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}

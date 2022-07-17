using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ExplorerScrollHiveFull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Completed Surveyor's Scroll");
		}

		public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 30;
			Item.height = 20;
            Item.createTile = ModContent.TileType<Tiles.Furniture.Paintings.HiveMap>();
			Item.value = Item.buyPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line1 = new TooltipLine(Mod, "FavoriteDesc", "A giant Hive has been charted!");
			line1.OverrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}

using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Ore");
			Tooltip.SetDefault("'Spirit-infused metal'");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 12;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.maxStack = 999;
			item.rare = ItemRarityID.Pink;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<SpiritOreTile>();
		}
	}
}

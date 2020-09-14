using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class FloranOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Ore");
			Tooltip.SetDefault("'A strange mixture of plant cells and metal'");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.rare = ItemRarityID.Green;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<FloranOreTile>();
		}
	}
}

using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
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
			Item.width = 16;
			Item.height = 16;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.value = 100;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<FloranOreTile>();
		}
	}
}

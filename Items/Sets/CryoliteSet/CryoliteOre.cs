using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoliteOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Ore");
			Tooltip.SetDefault("'Veins of metal intertwined with ice'");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.Orange;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<CryoliteOreTile>();
		}
	}
}

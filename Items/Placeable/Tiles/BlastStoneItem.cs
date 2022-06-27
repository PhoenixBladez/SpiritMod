using Terraria.ID;
using Terraria.ModLoader;
using BlastStoneTile = SpiritMod.Tiles.Block.BlastStone;
using SpiritMod.Tiles.Block.Ambient;
namespace SpiritMod.Items.Placeable.Tiles
{
	public class BlastStoneItem : AmbientStoneItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blast Stone");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<BlastStoneTile>();
		}
	}
}
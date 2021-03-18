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
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<BlastStoneTile>();
		}
	}
}
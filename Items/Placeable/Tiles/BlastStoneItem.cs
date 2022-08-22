using SpiritMod.Tiles.Block.Ambient;
using BlastStoneTile = SpiritMod.Tiles.Block.BlastStone;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class BlastStoneItem : AmbientStoneItem<BlastStoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blast Stone");
	}
}
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Relics
{
	public class VinewrathRelic : BaseRelic
	{
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Placeable.Relics.VinewrathRelicItem>());
	}
}

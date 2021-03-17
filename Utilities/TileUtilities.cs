using Terraria;
using Terraria.ObjectData;

namespace SpiritMod
{
	public static class TileUtilities
	{
		public static void BlockActuators(int i, int j)
		{
			Tile tile = Main.tile[i, j]; //get the tile and its data
			TileObjectData data = TileObjectData.GetTileData(tile);
			int frameX = tile.frameX / data.CoordinateWidth;
			int frameY = tile.frameY / (data.CoordinateFullHeight / data.Height);
			if (frameY == (data.Height - 1)) { //check if this tile is the lowest frame of the tile
				for (int x = 0 - frameX; x < data.Width - frameX; x++) { //iterate through all the tiles beneath this tile, starting from the leftmost and ending at the rightmost
					Main.tile[i + x, j + 1].inActive(false); //prevent tiles from being actuated
				}
			}
		}
	}
}

using SpiritMod.Tiles.Block;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class BiomeTileCounts : ModSystem
	{
		public int briarCount;
		public int spiritCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			briarCount = tileCounts[ModContent.TileType<BriarGrass>()];
			spiritCount = tileCounts[ModContent.TileType<Spiritsand>()] + tileCounts[ModContent.TileType<SpiritStone>()] + tileCounts[ModContent.TileType<SpiritDirt>()] + tileCounts[ModContent.TileType<SpiritGrass>()] + tileCounts[ModContent.TileType<SpiritIce>()];
		}
	}
}
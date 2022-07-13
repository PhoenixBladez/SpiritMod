using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Biomes
{
	internal class BiomeTileCounts : ModSystem
	{
		public int briarCount;
		public int spiritCount;
		public int asteroidCount;
		public bool inSynthwave;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			briarCount = tileCounts[ModContent.TileType<BriarGrass>()];
			spiritCount = tileCounts[ModContent.TileType<Spiritsand>()] + tileCounts[ModContent.TileType<SpiritStone>()] + tileCounts[ModContent.TileType<SpiritDirt>()] + tileCounts[ModContent.TileType<SpiritGrass>()] + tileCounts[ModContent.TileType<SpiritIce>()];
			asteroidCount = tileCounts[ModContent.TileType<Asteroid>()];
			inSynthwave = tileCounts[ModContent.TileType<SynthwaveHeadActive>()] > 0;
		}
	}
}
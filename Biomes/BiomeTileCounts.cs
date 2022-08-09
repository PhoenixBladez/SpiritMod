using SpiritMod.Items.Sets.StarplateDrops;
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

		public static bool InSpirit => ModContent.GetInstance<BiomeTileCounts>().spiritCount > 80;
		public static bool InBriar => ModContent.GetInstance<BiomeTileCounts>().briarCount > 80;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			briarCount = tileCounts[ModContent.TileType<BriarGrass>()];
			spiritCount = tileCounts[ModContent.TileType<Spiritsand>()] + tileCounts[ModContent.TileType<SpiritStone>()] + tileCounts[ModContent.TileType<SpiritDirt>()] + tileCounts[ModContent.TileType<SpiritGrass>()] + tileCounts[ModContent.TileType<SpiritIce>()];
			asteroidCount = tileCounts[ModContent.TileType<Asteroid>()] + tileCounts[ModContent.TileType<BigAsteroid>()] + tileCounts[ModContent.TileType<SpaceJunkTile>()] + tileCounts[ModContent.TileType<Glowstone>()];
			inSynthwave = tileCounts[ModContent.TileType<SynthwaveHeadActive>()] > 0;
		}
	}
}
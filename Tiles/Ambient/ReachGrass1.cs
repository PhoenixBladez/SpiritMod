using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class ReachGrass1 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();

			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
			TileID.Sets.SwaysInWindBasic[Type] = true;

			name.SetDefault("Spiky Grass");
			AddMapEntry(new Color(200, 200, 200), name);
			AdjTiles = new int[] { TileID.Lamps };
		}
	}
}
using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture.Reach;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Reach
{
	public class ReachPlatform : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.Platforms[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(179, 146, 107));
			dustType = 0;
			drop = ModContent.ItemType<ReachPlatformTile>();
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Platforms };
		}

		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}
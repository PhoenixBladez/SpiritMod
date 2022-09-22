using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture.GraniteSpikes;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.SurfaceIce
{
	public class TundraBerries2x2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);

			DustType = DustID.BorealWood;
			HitSound = SoundID.Dig;

			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		//public override void KillMultiTile(int i, int j, int frameX, int frameY)
		//{
		//	if (Main.rand.NextBool(3))
		//		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Sets.FrigidSet.FrigidFragment>());
		//}
	}

	public class TundraBerries1x2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.RandomStyleRange = 2;
			TileObjectData.newTile.StyleHorizontal = true;

			TileObjectData.addTile(Type);

			DustType = DustID.BorealWood;
			HitSound = SoundID.Dig;

			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		//public override void KillMultiTile(int i, int j, int frameX, int frameY)
		//{
		//	if (Main.rand.NextBool(3))
		//		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Sets.FrigidSet.FrigidFragment>());
		//}
	}
}
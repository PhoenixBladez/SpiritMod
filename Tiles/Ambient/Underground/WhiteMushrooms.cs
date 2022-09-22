using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Tiles.Block;

namespace SpiritMod.Tiles.Ambient.Underground
{
	public class WhiteMushroom2x2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.RandomStyleRange = 2;
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Grass, TileID.Dirt, TileID.Mud, TileID.Stone, TileID.ClayBlock, TileID.ArgonMoss, TileID.BlueMoss, TileID.BrownMoss, 
				TileID.GreenMoss, TileID.KryptonMoss, TileID.LavaMoss, TileID.PurpleMoss, TileID.RedMoss, TileID.XenonMoss, ModContent.TileType<Stargrass>() }; 
			TileObjectData.addTile(Type);

			DustType = DustID.RedMoss;

			AddMapEntry(new Color(170, 166, 140));
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;
		//public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, Mod.Find<ModItem>("Shrine3").Type);
	}

	public class WhiteMushroom2x3 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Grass, TileID.Dirt, TileID.Mud, TileID.Stone, TileID.ClayBlock, TileID.ArgonMoss, TileID.BlueMoss, TileID.BrownMoss,
				TileID.GreenMoss, TileID.KryptonMoss, TileID.LavaMoss, TileID.PurpleMoss, TileID.RedMoss, TileID.XenonMoss, ModContent.TileType<Stargrass>() };
			TileObjectData.addTile(Type);

			DustType = DustID.Stone;

			AddMapEntry(new Color(170, 166, 140));
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;
		//public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, Mod.Find<ModItem>("Shrine3").Type);
	}
}
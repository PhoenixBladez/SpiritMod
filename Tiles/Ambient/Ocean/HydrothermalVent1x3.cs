﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class HydrothermalVent1x3 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand };
			TileObjectData.newTile.RandomStyleRange = 1;
			TileObjectData.addTile(Type);

			disableSmartCursor = true;
			dustType = DustID.Stone;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hydrothermal Vent");
			AddMapEntry(new Color(64, 54, 66), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			Tile t = Framing.GetTileSafely(i, j);

			if (t.frameY == 0 && t.frameX == 0)
				HydrothermalVent1x2.SpawnSmoke(new Vector2(i - 0.75f, j) * 16);
		}
	}
}
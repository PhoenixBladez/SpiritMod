using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SpiritMod.Tiles.Furniture
{
	public class GemsPickaxeRuby : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lodged Pickaxe");
			this.AddMapEntry(Color.Red, name);
		}
		public override void SetDrawPositions (int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .122f;
			g = .048f;
			b = .063f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (Main.rand.Next(2) == 0)
            {
			    Terraria.Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<GemPickaxe>());
            }
			for (int k = 0; k < 3; k++)
			{
				Terraria.Item.NewItem(i * 16, j * 16, 64, 32, ItemID.Ruby);
			}
		}
	}
}

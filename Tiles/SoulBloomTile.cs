using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;

namespace SpiritMod.Tiles
{
	public class SoulBloomTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			//Main.tileAlch[Type] = true;
			Main.tileNoFail[Type] = true;
			//Main.tileLavaDeath[Type] = true;
			dustType = 187;
			//disableSmartCursor = true;
			//AddMapEntry(new Color(13, 88, 130), "Banner");
			//TileObjectData.newTile.Width = 1;
			//TileObjectData.newTile.Height = 1;
			//TileObjectData.newTile.Origin = Point16.Zero;
			//TileObjectData.newTile.UsesCustomCanPlace = true;
			//TileObjectData.newTile.CoordinateHeights = new int[]
			//{
			//	20
			//};
			//TileObjectData.newTile.CoordinateWidth = 16;
			//TileObjectData.newTile.CoordinatePadding = 2;
			//TileObjectData.newTile.DrawYOffset = -1;
			//TileObjectData.newTile.StyleHorizontal = true;
			//TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			//TileObjectData.newTile.LavaDeath = true;
			//TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			//TileObjectData.addBaseTile(out TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{

				mod.TileType("SpiritGrass")
			};
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				78, //ClayPot
				TileID.PlanterBox
			};
			TileObjectData.addTile(Type);
			//drop = mod.ItemType()
		}

		//public override bool CanPlace(int i, int j)
		//{
		//	return base.CanPlace(i, j);
		//}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override bool Drop(int i, int j)
		{
			int stage = Main.tile[i, j].frameX / 18;
			if (stage == 1)
			{
				Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("SoulBloom"));
			}
			if (stage == 2)
			{
				Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("SoulBloom"));
				Item.NewItem(i * 16, j * 16, 0, 0, mod.ItemType<Items.Placeable.SoulSeeds>());
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (Main.tile[i, j].frameX == 0)
			{
				Main.tile[i, j].frameX += 18;
			}
			else if (Main.tile[i, j].frameX == 18)
			{
				Main.tile[i, j].frameX += 18;
			}
		}

		//public override void RightClick(int i, int j)
		//{
		//	base.RightClick(i, j);
		//}
	}
}

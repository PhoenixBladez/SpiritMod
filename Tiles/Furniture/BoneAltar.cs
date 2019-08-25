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
	public class BoneAltar : ModTile
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
			dustType = 6;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Strange Altar");
			this.AddMapEntry(Colors.RarityAmber, name);
		}
		public override void SetDrawPositions (int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .46f;
			g = .32f;
			b = .1f;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/BoneAltar_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				if (Main.rand.Next(20) ==0)
				{
				int d  = Dust.NewDust(new Vector2(i*16 , j * 16 -10), 0, 16, 6, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division
				}
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (!NPC.AnyNPCs(mod.NPCType("ForestWraith")))
			{
				Main.NewText("You have disturbed the ancient Nature Spirits!", 0, 170, 60);
				NPC.NewNPC((int)i * 16, (int)j * 16 - 60, mod.NPCType("ForestWraith"), 0, 2, 1, 0, 0, Main.myPlayer);
				NPC.NewNPC((int)i * 16, (int)j * 16, mod.NPCType("Woody"), 0, 2, 1, 0, 0, Main.myPlayer);
				NPC.NewNPC((int)i * 16, (int)j * 16, mod.NPCType("Woody"), 0, 2, 1, 0, 0, Main.myPlayer);
			}
		}
	}
}

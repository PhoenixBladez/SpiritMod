﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class ShadowflameStone : ModTile
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
			name.SetDefault("Staff in the Stone");
			AddMapEntry(Colors.RarityPurple, name);
			dustType = DustID.ShadowbeamStaff;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .51f;
			g = .25f;
			b = .72f;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<ShadowflameStoneStaff>());
			Main.PlaySound(SoundID.Zombie, i * 16, j * 16, 7);
			NPC.NewNPC(i * 16 + Main.rand.Next(-60, 60), j * 16, 29, 0, 2, 1, 0, 0, Main.myPlayer);
			NPC.NewNPC(i * 16 + Main.rand.Next(-60, 60), j * 16, 29, 0, 2, 1, 0, 0, Main.myPlayer);
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Furniture/ShadowflameStone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Tile t = Main.tile[i, j];
			if (t.frameX % 54 == 0 && t.frameY == 0) {
				Main.spriteBatch.Draw(Main.extraTexture[60], new Vector2(i * 16 - (int)Main.screenPosition.X - 44, j * 16 - (int)Main.screenPosition.Y - 32) + zero, null, new Color(169, 3, 252, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}

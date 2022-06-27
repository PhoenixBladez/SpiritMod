using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using SpiritMod.Items.Material;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Dusts;

using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Tiles.Ambient
{
	public class MarbleObelisk : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(75, 139, 166));
			DustType = DustID.Stone;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.LunarMonolith };
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Items.Placeable.Furniture.MarbleObeliskItem>());
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			if (Main.tile[i, j].TileFrameY >= 56) {
                if (Main.rand.Next(100) == 1)
                {
                    int glyphnum = Main.rand.Next(10);
                    DustHelper.DrawDustImage(new Vector2(i * 16 + Main.rand.Next(-25, 25), j * 16 + Main.rand.Next(-25, 0)), ModContent.DustType<MarbleDust>(), 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
                }
            }
		}
		public override bool RightClick(int i, int j) {
			SoundEngine.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

        public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType<Items.Placeable.Furniture.MarbleObeliskItem>();
		}

        public override void HitWire(int i, int j) {
			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
			for (int l = x; l < x + 2; l++) {
				for (int m = y; m < y + 3; m++) {
					if (Main.tile[l, m] == null) {
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type) {
						if (Main.tile[l, m].TileFrameY < 56) {
							Main.tile[l, m].TileFrameY += 56;
						}
						else {
							Main.tile[l, m].TileFrameY -= 56;
						}
					}
				}
			}
			if (Wiring.running) {
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
		}
	}
}
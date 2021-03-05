using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class SynthwaveHead : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 18 };
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(75, 139, 166));
			disableSmartCursor = true;
			adjTiles = new int[]{ TileID.LunarMonolith };
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("VoidMonolith"));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.tile[i, j].frameY >= 74)
			{
				MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
				modPlayer.ZoneSynthwave = true;
			}
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			//if (tile.frameY == 18 || (tile.frameY == 36 && (tile.frameX == 18 || tile.frameX == 72)))
			{
				Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

				Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SynthwaveHead_Glow");
				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

				spriteBatch.Draw(glow, new Vector2(i * 16, j * 16 + 2) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), colour);
			} 
	    	if (Main.tile[i, j].frameY >= 74)
			{       
                if (Main.rand.Next(50)==0)
                {
                    int index3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16) - 20), 16, 16, 226, 0.0f, 0f, 150, new Color(), 0.5f);
                    Main.dust[index3].fadeIn = 0.75f;
                    Main.dust[index3].velocity = new Vector2((float)Main.rand.Next(-2,2), (float)Main.rand.Next(-2,-1));
                    Main.dust[index3].noLight = true;
                    Main.dust[index3].noGravity = true;
                }
                if (Main.rand.Next(50)==0)
                {
                    int index3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16) - 20), 16, 16, 272, 0.0f, 0f, 150, new Color(), 0.5f);
                    Main.dust[index3].fadeIn = 0.75f;
                    Main.dust[index3].velocity = new Vector2((float)Main.rand.Next(-2,2), (float)Main.rand.Next(-2,-1));
                    Main.dust[index3].noGravity = true;
                }
            }
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void RightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("VoidMonolith");
		}

		public override void HitWire(int i, int j)
		{
			int x = i - (Main.tile[i, j].frameX / 18) % 3;
			int y = j - (Main.tile[i, j].frameY / 18) % 4;
			for (int l = x; l < x + 3; l++)
			{
				for (int m = y; m < y + 4; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						if (Main.tile[l, m].frameY < 74)
						{
							Main.tile[l, m].frameY += 74;
						}
						else
						{
							Main.tile[l, m].frameY -= 74;
						}
					}
				}
			}
			if (Wiring.running)
			{
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
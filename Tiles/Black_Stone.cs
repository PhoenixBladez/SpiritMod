using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles
{
	public class Black_Stone : ModTile
	{
		public bool check;
		public int colorNumber = 510;
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true;
			dustType = 180;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blackrock");
			drop = mod.ItemType("Black_Stone_Item");
			AddMapEntry(new Color(133, 206, 181), name);
			soundStyle = 21;
		}

		public override bool KillSound(int i, int j)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(21, 0));
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.1f + (Convert.ToSingle(colorNumber) * 0.001f);
			b = 0.3f  + (Convert.ToSingle(colorNumber) * 0.001f);

		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (colorNumber == 510)
			{
				check = true;
			}
			else if (colorNumber == 200)
			{
				check = false;
			}

			if (check)
			{
				colorNumber--;
			}
			else if (!check)
			{
				colorNumber++;
			}
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Texture2D texture;
			if (Main.canDrawColorTile(i, j))
			{
				texture = Main.tileAltTexture[Type, (int)tile.color()];
			}
			else
			{
				texture = Main.tileTexture[Type];
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			int animate = 0;
			if (tile.frameY >= 56)
			{
				animate = Main.tileFrame[Type] * animationFrameHeight;
			}
			Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/Black_Stone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), new Color(colorNumber / 2, colorNumber / 2, colorNumber / 2), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}

	internal class Black_Stone_Item : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blackrock");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.value = 1000;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 0;
			item.createTile = mod.TileType("Black_Stone");
		}
	}
}
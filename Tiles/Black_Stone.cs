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
			g = 0.1f + (510 * 0.001f);
			b = 0.3f  + (510 * 0.001f);

		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			if (tile.slope() == 0 && !tile.halfBrick()) 
			{
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Black_Stone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
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
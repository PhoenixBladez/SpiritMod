using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles
{
	public class Black_Stone : ModTile
	{
		public bool check;
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true;
			DustType = DustID.DungeonSpirit;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Blackrock");
			ItemDrop = Mod.Find<ModItem>("Black_Stone_Item").Type;
			AddMapEntry(new Color(133, 206, 181), name);
			soundStyle = 21;
		}

		public override bool KillSound(int i, int j)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(21, 0));
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock) 
			{
				Main.spriteBatch.Draw(Mod.GetTexture("Tiles/Black_Stone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	internal class Black_Stone_Item : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blackrock");

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.value = 1000;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.White;
			Item.createTile = Mod.Find<ModTile>("Black_Stone").Type;
		}
	}
}
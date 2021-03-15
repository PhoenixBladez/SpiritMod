using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Tiles.Block.Ambient
{
    public class MagmastoneItem : AmbientStoneItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magmastone");
		}
		public override void SetDefaults()
		{		
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<Magmastone_Tile>();
        }
    }
    public class Magmastone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			drop = ModContent.ItemType<MagmastoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Magmastone");
			dustType = 6;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.222f;
			g = .133f;
			b = .073f;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			if (tile.slope() == 0 && !tile.halfBrick()) {
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/Magmastone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }
    public class SmolderingRockItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Smoldering Rock");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<SmolderingRock_Tile>();
        }
    }
    public class SmolderingRock_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			drop = ModContent.ItemType<SmolderingRockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Magmastone");
			dustType = 6;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.222f;
			g = .133f;
			b = .073f;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			int height = tile.frameY == 36 ? 18 : 16;
			if (tile.slope() == 0 && !tile.halfBrick()) {
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/SmolderingRock_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }

    public class MottledStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mottled Stone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<MottledStone_Tile>();
        }
    }
    public class MottledStone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(60, 60, 60));
			drop = ModContent.ItemType<MottledStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mottled Stone");
			dustType = 54;
        }
    }
    public class OldStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Stone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<OldStone_Tile>();
        }
    }
    public class OldStone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(74, 60, 50));
			drop = ModContent.ItemType<OldStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Old Stone");
			dustType = 8;
        }
    }
    public class RuinstoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruinstone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<Ruinstone_Tile>();
        }
    }
    public class Ruinstone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 76, 48));
			drop = ModContent.ItemType<RuinstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ruined Stone");
			dustType = 38;
        }
    }
    public class VinestoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinestone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<Vinestone_Tile>();
        }
    }
    public class Vinestone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(69, 74, 60));
			drop = ModContent.ItemType<VinestoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vinestone");
			dustType = 38;
        }
    }
    public class WornStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worn Stone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<WornStone_Tile>();
        }
    }
    public class WornStone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<WornStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Worn Stone");
			dustType = 1;
        }
    }    
	public class CorruptPustuleItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Pustule");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<CorruptPustule_Tile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.RottenChunk, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 50);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(ItemID.StoneBlock, 50);
			recipe1.AddRecipe();
		}
    }
    public class CorruptPustule_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			drop = ModContent.ItemType<CorruptPustuleItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Pustule");
			dustType = 17;
        }
    }
    public class StalactiteStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stalactite Stone");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
            item.createTile = ModContent.TileType<StalactiteStone_Tile>();
        }
    }
    public class StalactiteStone_Tile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<StalactiteStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Stalactite Stone");
			dustType = 1;
        }
    }
}
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
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
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
			r = 0.222f * 1.5f;
			g = .133f * 1.5f;
			b = .073f * 1.5f;
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
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
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
			r = 0.222f * 1.5f;
			g = .133f* 1.5f;
			b = .073f* 1.5f;
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
    public class CinderstoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cinderstone");
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
            item.createTile = ModContent.TileType<CinderstoneTile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
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
    public class CinderstoneTile : ModTile
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
			drop = ModContent.ItemType<CinderstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cinderstone");
			dustType = 6;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.222f * 1.5f;
			g = .133f* 1.5f;
			b = .073f* 1.5f;
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
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/Cinderstone_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
    public class AzureGemBlockItem : AzureGemItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Block");
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
            item.createTile = ModContent.TileType<AzureBlock_Tile>();
        }
    }
    public class AzureBlock_Tile : ModTile
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
			drop = ModContent.ItemType<AzureGemBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Azure Block");
			dustType = 187;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.052f * 1.5f;
			g = .128f * 1.5f;
			b = .235f * 1.5f;
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
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/AzureBlock_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }
    public class ObsidianBlockItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Stone");
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
            item.createTile = ModContent.TileType<ObsidianBlockTile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.Obsidian, 1);
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
    public class ObsidianBlockTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(60, 60, 60));
			drop = ModContent.ItemType<ObsidianBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Obsidian Stone");
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
 	public class OutlandStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outland Stone");
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
            item.createTile = ModContent.TileType<OutlandStoneTile>();
        }
    }
    public class OutlandStoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(74, 60, 50));
			drop = ModContent.ItemType<OutlandStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Outland Stone");
			dustType = 54;
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
	public class IvyStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ivy Stone");
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
            item.createTile = ModContent.TileType<IvyStoneTile>();
        }
    }
    public class IvyStoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<IvyStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ivy Stone");
			dustType = 1;
        }
    }    
	public class CorruptPustuleItem : AmbientCorruptItem
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
        	Main.tileLighted[Type] = true;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.074f * 1.5f;
			g = .143f * 1.5f;
			b = .040f * 1.5f;
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
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/CorruptPustule_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }
	public class DarkFoliageItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Foliage");
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
            item.createTile = ModContent.TileType<DarkFoliageTile>();
        }
    }
    public class DarkFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(80, 55, 82));
			drop = ModContent.ItemType<DarkFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Dark Foliage");
			dustType = 17;
        }
    }
	public class CorruptOvergrowthItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Overgrowth");
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
            item.createTile = ModContent.TileType<CorruptOvergrowthTile>();
        }
    }
    public class CorruptOvergrowthTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			drop = ModContent.ItemType<CorruptOvergrowthItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Overgrowth");
			dustType = 17;
        }
    }
	public class CorruptTendrilItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Tendril");
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
            item.createTile = ModContent.TileType<CorruptTendrilTile>();
        }
    }
    public class CorruptTendrilTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			drop = ModContent.ItemType<CorruptTendrilItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Tendril");
			dustType = 17;
        }
    }
	public class CorruptMassItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupt Mass");
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
            item.createTile = ModContent.TileType<CorruptMassTile>();
        }
    }
    public class CorruptMassTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			drop = ModContent.ItemType<CorruptMassItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Mass");
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
	public class CragstoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cragstone");
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
            item.createTile = ModContent.TileType<CragstoneTile>();
        }
    }
    public class CragstoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<CragstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cragstone");
			dustType = 1;
        }
    }
	public class FracturedStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fractured Stone");
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
            item.createTile = ModContent.TileType<FracturedStoneTile>();
        }
    }
    public class FracturedStoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			drop = ModContent.ItemType<FracturedStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fractured Stone");
			dustType = 1;
        }
    }
	public class LeafyDirtItem : AmbientLeafItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leafy Dirt");
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
            item.createTile = ModContent.TileType<LeafyDirtTile>();
        }
    }
    public class LeafyDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<LeafyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Leafy Dirt");
			dustType = 0;
        }
    }
	public class ForestFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forest Foliage");
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
            item.createTile = ModContent.TileType<ForestFoliageTile>();
        }
    }
    public class ForestFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(57, 128, 78));
			drop = ModContent.ItemType<ForestFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Forest Foliage");
			dustType = 2;
        }
    }
	public class FloweryFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flowery Foliage");
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
            item.createTile = ModContent.TileType<FloweryFoliageTile>();
        }
    }
    public class FloweryFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(57, 128, 78));
			drop = ModContent.ItemType<FloweryFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Flowery Foliage");
			dustType = 2;
        }
    }
	public class JungleFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Foliage");
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
            item.createTile = ModContent.TileType<JungleFoliageTile>();
        }
    }
    public class JungleFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(66, 122, 49));
			drop = ModContent.ItemType<JungleFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jungle Foliage");
			dustType = 18;
        }
    }
	public class CrumblingDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumbling Dirt");
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
            item.createTile = ModContent.TileType<CrumblingDirtTile>();
        }
    }
    public class CrumblingDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<CrumblingDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crumbling Dirt");
			dustType = 0;
        }
    }

	public class CrackedDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cracked Dirt");
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
            item.createTile = ModContent.TileType<CrackedDirtTile>();
        }
    }
    public class CrackedDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<CrackedDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cracked Dirt");
			dustType = 0;
        }
    }
	public class RoughDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rough Dirt");
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
            item.createTile = ModContent.TileType<RoughDirtTile>();
        }
    }
    public class RoughDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<RoughDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Rough Dirt");
			dustType = 0;
        }
    }
	public class RockyDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rocky Dirt");
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
            item.createTile = ModContent.TileType<RockyDirtTile>();
        }
    }
    public class RockyDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<RockyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Rocky Dirt");
			dustType = 0;
        }
    }
	public class LayeredDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Layered Dirt");
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
            item.createTile = ModContent.TileType<LayeredDirtTile>();
        }
    }
    public class LayeredDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<LayeredDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Layered Dirt");
			dustType = 0;
        }
    }
	public class CaveDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cave Dirt");
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
            item.createTile = ModContent.TileType<CaveDirtTile>();
        }
    }
    public class CaveDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<CaveDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cave Dirt");
			dustType = 0;
        }
    }
	public class WavyDirtItem : AmbientDirtItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wavy Dirt");
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
            item.createTile = ModContent.TileType<WavyDirtTile>();
        }
    }
    public class WavyDirtTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			drop = ModContent.ItemType<WavyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wavy Dirt");
			dustType = 0;
        }
    }

	public class CrimsonPustuleItem : AmbientCrimsonItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Pustule");

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
            item.createTile = ModContent.TileType<CrimsonPustuleBlockTile>();
        }
    }

    public class CrimsonPustuleBlockTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			drop = ModContent.ItemType<CrimsonPustuleItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Pustule");
			dustType = 5;
        }
    }

	public class CrimsonScabItem : AmbientCrimsonItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Scab");
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
            item.createTile = ModContent.TileType<CrimsonScabTile>();
        }
    }

    public class CrimsonScabTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			drop = ModContent.ItemType<CrimsonScabItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Scab");
			dustType = 5;
        }
    }

	public class BloodyFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bloody Foliage");

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
            item.createTile = ModContent.TileType<BloodyFoliageTile>();
        }
    }

    public class BloodyFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(92, 36, 49));
			drop = ModContent.ItemType<BloodyFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bloody Foliage");
			dustType = 5;
        }
    }

	public class CrimsonBlisterItem : AmbientCrimsonItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Blister");
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
            item.createTile = ModContent.TileType<CrimsonBlisterTile>();
        }
    }

    public class CrimsonBlisterTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			drop = ModContent.ItemType<CrimsonBlisterItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Blister");
			dustType = 5;
			Main.tileLighted[Type] = true;
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.212f;
			g = .146f;
			b = .066f;
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
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/Block/Ambient/CrimsonBlister_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
    }

	public class HallowPrismstoneItem : AmbientHallowItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Prismstone");
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
            item.createTile = ModContent.TileType<HallowPrismstoneTile>();
        }
    }
    public class HallowPrismstoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			drop = ModContent.ItemType<HallowPrismstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Prismstone");
			dustType = 69;
        	Main.tileLighted[Type] = true;
        }
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.152f * 1.5f;
			g = .039f * 1.5f;
			b = .168f * 1.5f;
		}
	}
	public class HallowCavernstoneItem : AmbientHallowItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Cavernstone");
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
            item.createTile = ModContent.TileType<HallowCavernstoneTile>();
        }
    }
    public class HallowCavernstoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			drop = ModContent.ItemType<HallowCavernstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Cavernstone");
			dustType = 51;
        }
	}
	public class HallowFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Foliage");
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
            item.createTile = ModContent.TileType<HallowFoliageTile>();
        }
    }
    public class HallowFoliageTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(39, 132, 168));
			drop = ModContent.ItemType<HallowFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Foliage");
			dustType = 93;
        }
	}
	public class HallowShardstoneItem : AmbientHallowItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Shardstone");
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
            item.createTile = ModContent.TileType<HallowShardstoneTile>();
        }
    }
    public class HallowShardstoneTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			drop = ModContent.ItemType<HallowShardstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Shardstone");
			dustType = 69;
        }
	}
	public class HallowCrystallineItem : AmbientHallowItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Crystalline Stone");
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
            item.createTile = ModContent.TileType<HallowCrystallineTile>();
        }
    }
    public class HallowCrystallineTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			drop = ModContent.ItemType<HallowCrystallineItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Crystalline Stone");
			dustType = 69;
        }
	}
	public class HiveBlockAltItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hive Hexblock");
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
            item.createTile = ModContent.TileType<HiveBlockAltTile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(1124, 1);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(1124, 1);
			recipe1.AddRecipe();
		}
    }
    public class HiveBlockAltTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			drop = ModContent.ItemType<HiveBlockAltItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hive Hexblock");
			dustType = 7;
        }
	}
	public class RuneBlockItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Rune Block");
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
            item.createTile = ModContent.TileType<RuneBlockTile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(2271, 4);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(2271, 1);
			recipe1.AddRecipe();
		}
    }
    public class RuneBlockTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(99, 45, 117));
			drop = ModContent.ItemType<RuneBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Arcane Rune Block");
			dustType = 24;
        }
	}
	public class KrampusHornBlockItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Krampus Horn Block");
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
            item.createTile = ModContent.TileType<KrampusHornBlockTile>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(1955, 4);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Tiles.Furniture.ForagerTableTile>());
			recipe1.SetResult(1955, 1);
			recipe1.AddRecipe();
		}
    }
    public class KrampusHornBlockTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(200, 200, 200));
			drop = ModContent.ItemType<KrampusHornBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Krampus Horn Block");
			dustType = 32;
        }
	}
}
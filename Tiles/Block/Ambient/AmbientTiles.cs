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
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magmastone");

		public override void SetDefaults()
		{		
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<Magmastone_Tile>();
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
    }
    public class Magmastone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<MagmastoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Magmastone");
			DustType = DustID.Fire;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/Magmastone_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
    }
    public class SmolderingRockItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Smoldering Rock");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<SmolderingRock_Tile>();
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
    }
    public class SmolderingRock_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<SmolderingRockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Magmastone");
			DustType = DustID.Fire;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/SmolderingRock_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
    }
    public class CinderstoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cinderstone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CinderstoneTile>();
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}		
    }
    public class CinderstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<CinderstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cinderstone");
			DustType = DustID.Fire;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock) 
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/Cinderstone_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
    }
    public class MottledStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mottled Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<MottledStone_Tile>();
        }
    }

    public class MottledStone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(60, 60, 60));
			ItemDrop = ModContent.ItemType<MottledStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mottled Stone");
			DustType = DustID.Wraith;
        }
    }

    public class AzureGemBlockItem : AzureGemItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Azure Block");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<AzureBlock_Tile>();
        }
    }

    public class AzureBlock_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<AzureGemBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Azure Block");
			DustType = DustID.Flare_Blue;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/AzureBlock_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
    }

    public class ObsidianBlockItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Obsidian Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<ObsidianBlockTile>();
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}		
    }
    public class ObsidianBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(60, 60, 60));
			ItemDrop = ModContent.ItemType<ObsidianBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Obsidian Stone");
			DustType = DustID.Wraith;
        }
    }
    public class OldStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<OldStone_Tile>();
        }
    }
    public class OldStone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(74, 60, 50));
			ItemDrop = ModContent.ItemType<OldStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Old Stone");
			DustType = DustID.Iron;
        }
    }
 	public class OutlandStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Outland Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<OutlandStoneTile>();
        }
    }
    public class OutlandStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(74, 60, 50));
			ItemDrop = ModContent.ItemType<OutlandStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Outland Stone");
			DustType = DustID.Wraith;
        }
    }

    public class RuinstoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ruinstone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<Ruinstone_Tile>();
        }
    }

    public class Ruinstone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 76, 48));
			ItemDrop = ModContent.ItemType<RuinstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ruined Stone");
			DustType = DustID.Mud;
        }
    }

    public class VinestoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vinestone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<Vinestone_Tile>();
        }
    }

    public class Vinestone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(69, 74, 60));
			ItemDrop = ModContent.ItemType<VinestoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vinestone");
			DustType = DustID.Mud;
        }
    }

    public class WornStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Worn Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<WornStone_Tile>();
        }
    }

    public class WornStone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<WornStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Worn Stone");
			DustType = DustID.Stone;
        }
    }    

	public class IvyStoneItem : AmbientStoneItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ivy Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<IvyStoneTile>();
        }
    }
    public class IvyStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<IvyStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ivy Stone");
			DustType = DustID.Stone;
        }
    }    

	public class CorruptPustuleItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Pustule");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CorruptPustule_Tile>();
        }
    }

    public class CorruptPustule_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptPustuleItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Pustule");
			DustType = DustID.Corruption_Gravity;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock) {
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/CorruptPustule_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<DarkFoliageTile>();
        }
    }
    public class DarkFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<DarkFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Dark Foliage");
			DustType = DustID.Corruption_Gravity;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CorruptOvergrowthTile>();
        }
    }
    public class CorruptOvergrowthTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptOvergrowthItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Overgrowth");
			DustType = DustID.Corruption_Gravity;
        }
    }
	public class CorruptTendrilItem : AmbientCorruptItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Tendril");
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CorruptTendrilTile>();
        }
    }
    public class CorruptTendrilTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptTendrilItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Tendril");
			DustType = DustID.Corruption_Gravity;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CorruptMassTile>();
        }
    }
    public class CorruptMassTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptMassItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrupt Mass");
			DustType = DustID.Corruption_Gravity;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<StalactiteStone_Tile>();
        }
    }
    public class StalactiteStone_Tile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<StalactiteStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Stalactite Stone");
			DustType = DustID.Stone;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CragstoneTile>();
        }
    }
    public class CragstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<CragstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cragstone");
			DustType = DustID.Stone;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<FracturedStoneTile>();
        }
    }
    public class FracturedStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<FracturedStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Fractured Stone");
			DustType = DustID.Stone;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<LeafyDirtTile>();
        }
    }
    public class LeafyDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<LeafyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Leafy Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<ForestFoliageTile>();
        }
    }
    public class ForestFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(57, 128, 78));
			ItemDrop = ModContent.ItemType<ForestFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Forest Foliage");
			DustType = DustID.Grass;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<FloweryFoliageTile>();
        }
    }
    public class FloweryFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(57, 128, 78));
			ItemDrop = ModContent.ItemType<FloweryFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Flowery Foliage");
			DustType = DustID.Grass;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<JungleFoliageTile>();
        }
    }
    public class JungleFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(66, 122, 49));
			ItemDrop = ModContent.ItemType<JungleFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jungle Foliage");
			DustType = DustID.Vile;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CrumblingDirtTile>();
        }
    }
    public class CrumblingDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CrumblingDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crumbling Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CrackedDirtTile>();
        }
    }
    public class CrackedDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CrackedDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cracked Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<RoughDirtTile>();
        }
    }
    public class RoughDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<RoughDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Rough Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<RockyDirtTile>();
        }
    }
    public class RockyDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<RockyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Rocky Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<LayeredDirtTile>();
        }
    }
    public class LayeredDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<LayeredDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Layered Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CaveDirtTile>();
        }
    }
    public class CaveDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CaveDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cave Dirt");
			DustType = DustID.Dirt;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<WavyDirtTile>();
        }
    }
    public class WavyDirtTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<WavyDirtItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wavy Dirt");
			DustType = DustID.Dirt;
        }
    }

	public class CrimsonPustuleItem : AmbientCrimsonItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Pustule");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CrimsonPustuleBlockTile>();
        }
    }

    public class CrimsonPustuleBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonPustuleItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Pustule");
			DustType = DustID.Blood;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CrimsonScabTile>();
        }
    }

    public class CrimsonScabTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonScabItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Scab");
			DustType = DustID.Blood;
        }
    }

	public class BloodyFoliageItem : AmbientLeafItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bloody Foliage");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<BloodyFoliageTile>();
        }
    }

    public class BloodyFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<BloodyFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bloody Foliage");
			DustType = DustID.Blood;
        }
    }

	public class CrimsonBlisterItem : AmbientCrimsonItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Blister");
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<CrimsonBlisterTile>();
        }
    }

    public class CrimsonBlisterTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonBlisterItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crimson Blister");
			DustType = DustID.Blood;
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
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock) {
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/CrimsonBlister_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HallowPrismstoneTile>();
        }
    }
    public class HallowPrismstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowPrismstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Prismstone");
			DustType = DustID.PinkCrystalShard;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HallowCavernstoneTile>();
        }
    }
    public class HallowCavernstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowCavernstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Cavernstone");
			DustType = DustID.SnowBlock;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HallowFoliageTile>();
        }
    }
    public class HallowFoliageTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			soundType = SoundID.Grass;
            AddMapEntry(new Color(39, 132, 168));
			ItemDrop = ModContent.ItemType<HallowFoliageItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Foliage");
			DustType = DustID.Moss_Green;
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
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HallowShardstoneTile>();
        }
    }
    public class HallowShardstoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowShardstoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Shardstone");
			DustType = DustID.PinkCrystalShard;
        }
	}
	public class HallowCrystallineItem : AmbientHallowItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Crystalline Stone");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HallowCrystallineTile>();
        }
    }

    public class HallowCrystallineTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowCrystallineItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hallowed Crystalline Stone");
			DustType = DustID.PinkCrystalShard;
        }
	}

	public class HiveBlockAltItem : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hive Hexblock");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<HiveBlockAltTile>();
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Hive, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.Hive, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
    }
    public class HiveBlockAltTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HiveBlockAltItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hive Hexblock");
			DustType = 7;
        }
	}
	public class RuneBlockItem : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Arcane Rune Block");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<RuneBlockTile>();
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.ArcaneRuneWall, 4);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.ArcaneRuneWall, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
    }

    public class RuneBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(99, 45, 117));
			ItemDrop = ModContent.ItemType<RuneBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Arcane Rune Block");
			DustType = DustID.CorruptionThorns;
        }
	}

	public class KrampusHornBlockItem : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Krampus Horn Block");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
            Item.createTile = ModContent.TileType<KrampusHornBlockTile>();
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.KrampusHornWallpaper, 4);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Mod.CreateRecipe(ItemID.KrampusHornWallpaper, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
    }

    public class KrampusHornBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(200, 200, 200));
			ItemDrop = ModContent.ItemType<KrampusHornBlockItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Krampus Horn Block");
			DustType = DustID.Sand;
        }
	}
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Tiles.Block.Ambient
{
	public class MagmastoneItem : AmbientStoneItem<Magmastone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magmastone");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public class Magmastone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			ItemDrop = ModContent.ItemType<MagmastoneItem>();
			DustType = DustID.Torch;
			AddMapEntry(new Color(79, 55, 59));
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
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/Magmastone_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}

	public class SmolderingRockItem : AmbientStoneItem<SmolderingRock_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Smoldering Rock");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public class SmolderingRock_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<SmolderingRockItem>();
			DustType = DustID.Torch;
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
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/SmolderingRock_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}

	public class CinderstoneItem : AmbientStoneItem<CinderstoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cinderstone");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.AshBlock, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}
	public class CinderstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<CinderstoneItem>();
			DustType = DustID.Torch;
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
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/Cinderstone_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}

	public class MottledStoneItem : AmbientStoneItem<MottledStone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Mottled Stone");
	}

	public class MottledStone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(60, 60, 60));
			ItemDrop = ModContent.ItemType<MottledStoneItem>();
			DustType = DustID.Wraith;
		}
	}

	public class AzureGemBlockItem : AzureGemItem<AzureBlock_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Azure Block");
	}

	public class AzureBlock_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(79, 55, 59));
			ItemDrop = ModContent.ItemType<AzureGemBlockItem>();
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
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/AzureBlock_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}

	public class ObsidianBlockItem : AmbientStoneItem<ObsidianBlockTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Obsidian Stone");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.Obsidian, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.StoneBlock, 50);
			recipe1.AddIngredient(this, 50);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}
	public class ObsidianBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(60, 60, 60));
			ItemDrop = ModContent.ItemType<ObsidianBlockItem>();
			DustType = DustID.Wraith;
		}
	}

	public class OldStoneItem : AmbientStoneItem<OldStone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Old Stone");
	}

	public class OldStone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(74, 60, 50));
			ItemDrop = ModContent.ItemType<OldStoneItem>();
			DustType = DustID.Iron;
		}
	}

	public class OutlandStoneItem : AmbientStoneItem<OutlandStoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Outland Stone");
	}

	public class OutlandStoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(74, 60, 50));
			ItemDrop = ModContent.ItemType<OutlandStoneItem>();
			DustType = DustID.Wraith;
		}
	}

	public class RuinstoneItem : AmbientStoneItem<Ruinstone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ruinstone");
	}

	public class Ruinstone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			Main.tileBlendAll[Type] = true;
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(115, 76, 48));
			ItemDrop = ModContent.ItemType<RuinstoneItem>();
			DustType = DustID.Mud;
		}
	}

	public class VinestoneItem : AmbientStoneItem<Vinestone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vinestone");
	}

	public class Vinestone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(69, 74, 60));
			ItemDrop = ModContent.ItemType<VinestoneItem>();
			DustType = DustID.Mud;
		}
	}

	public class WornStoneItem : AmbientStoneItem<WornStone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Worn Stone");
	}

	public class WornStone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<WornStoneItem>();
			DustType = DustID.Stone;
		}
	}

	public class IvyStoneItem : AmbientStoneItem<IvyStoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ivy Stone");
	}

	public class IvyStoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<IvyStoneItem>();
			DustType = DustID.Stone;
		}
	}

	public class CorruptPustuleItem : AmbientCorruptItem<CorruptPustule_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Pustule");
	}

	public class CorruptPustule_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptPustuleItem>();
			DustType = DustID.CorruptPlants;
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
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
			{
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/CorruptPustule_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public class DarkFoliageItem : AmbientCorruptItem<DarkFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Dark Foliage");
	}

	public class DarkFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<DarkFoliageItem>();
			DustType = DustID.CorruptPlants;
		}
	}

	public class CorruptOvergrowthItem : AmbientCorruptItem<CorruptOvergrowthTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Overgrowth");
	}

	public class CorruptOvergrowthTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptOvergrowthItem>();
			DustType = DustID.CorruptPlants;
		}
	}

	public class CorruptTendrilItem : AmbientCorruptItem<CorruptTendrilTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Tendril");
	}

	public class CorruptTendrilTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptTendrilItem>();
			DustType = DustID.CorruptPlants;
		}
	}

	public class CorruptMassItem : AmbientCorruptItem<CorruptMassTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Corrupt Mass");
	}

	public class CorruptMassTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(80, 55, 82));
			ItemDrop = ModContent.ItemType<CorruptMassItem>();
			DustType = DustID.CorruptPlants;
		}
	}

	public class StalactiteStoneItem : AmbientStoneItem<StalactiteStone_Tile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Stalactite Stone");
	}

	public class StalactiteStone_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<StalactiteStoneItem>();
			DustType = DustID.Stone;
		}
	}

	public class CragstoneItem : AmbientStoneItem<CragstoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cragstone");
	}

	public class CragstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<CragstoneItem>();
			DustType = DustID.Stone;
		}
	}

	public class FracturedStoneItem : AmbientStoneItem<FracturedStoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Fractured Stone");
	}

	public class FracturedStoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<FracturedStoneItem>();
			DustType = DustID.Stone;
		}
	}

	public class LeafyDirtItem : AmbientLeafItem<LeafyDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Leafy Dirt");
	}

	public class LeafyDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<LeafyDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class ForestFoliageItem : AmbientLeafItem<ForestFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Forest Foliage");
	}

	public class ForestFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(57, 128, 78));
			ItemDrop = ModContent.ItemType<ForestFoliageItem>();
			DustType = DustID.Grass;
		}
	}
	public class FloweryFoliageItem : AmbientLeafItem<FloweryFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Flowery Foliage");
	}

	public class FloweryFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(57, 128, 78));
			ItemDrop = ModContent.ItemType<FloweryFoliageItem>();
			DustType = DustID.Grass;
		}
	}

	public class JungleFoliageItem : AmbientLeafItem<JungleFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Jungle Foliage");
	}

	public class JungleFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(66, 122, 49));
			ItemDrop = ModContent.ItemType<JungleFoliageItem>();
			DustType = DustID.CorruptGibs;
		}
	}

	public class CrumblingDirtItem : AmbientDirtItem<CrumblingDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crumbling Dirt");
	}

	public class CrumblingDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CrumblingDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class CrackedDirtItem : AmbientDirtItem<CrackedDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cracked Dirt");
	}

	public class CrackedDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CrackedDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class RoughDirtItem : AmbientDirtItem<RoughDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rough Dirt");
	}

	public class RoughDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<RoughDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class RockyDirtItem : AmbientDirtItem<RockyDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rocky Dirt");
	}

	public class RockyDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<RockyDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class LayeredDirtItem : AmbientDirtItem<LayeredDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Layered Dirt");
	}

	public class LayeredDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<LayeredDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class CaveDirtItem : AmbientDirtItem<CaveDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cave Dirt");
	}

	public class CaveDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<CaveDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class WavyDirtItem : AmbientDirtItem<WavyDirtTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Wavy Dirt");
	}

	public class WavyDirtTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(115, 87, 62));
			ItemDrop = ModContent.ItemType<WavyDirtItem>();
			DustType = DustID.Dirt;
		}
	}

	public class CrimsonPustuleItem : AmbientCrimsonItem<CrimsonPustuleBlockTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Pustule");
	}

	public class CrimsonPustuleBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonPustuleItem>();
			DustType = DustID.Blood;
		}
	}

	public class CrimsonScabItem : AmbientCrimsonItem<CrimsonScabTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Scab");
	}

	public class CrimsonScabTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonScabItem>();
			DustType = DustID.Blood;
		}
	}

	public class BloodyFoliageItem : AmbientLeafItem<BloodyFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bloody Foliage");
	}

	public class BloodyFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<BloodyFoliageItem>();
			DustType = DustID.Blood;
		}
	}

	public class CrimsonBlisterItem : AmbientCrimsonItem<CrimsonBlisterTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crimson Blister");
	}

	public class CrimsonBlisterTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			AddMapEntry(new Color(92, 36, 49));
			ItemDrop = ModContent.ItemType<CrimsonBlisterItem>();
			DustType = DustID.Blood;
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
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

			int height = tile.TileFrameY == 36 ? 18 : 16;
			if (tile.Slope == 0 && !tile.IsHalfBlock)
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Block/Ambient/CrimsonBlister_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 60), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}

	public class HallowPrismstoneItem : AmbientHallowItem<HallowPrismstoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Prismstone");
	}

	public class HallowPrismstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type, true);
			AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowPrismstoneItem>();
			DustType = DustID.PinkCrystalShard;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.152f * 1.5f;
			g = .039f * 1.5f;
			b = .168f * 1.5f;
		}
	}

	public class HallowCavernstoneItem : AmbientHallowItem<HallowCavernstoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Cavernstone");
	}

	public class HallowCavernstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowCavernstoneItem>();
			DustType = DustID.SnowBlock;
		}
	}

	public class HallowFoliageItem : AmbientLeafItem<HallowFoliageTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Foliage");
	}

	public class HallowFoliageTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(39, 132, 168));
			ItemDrop = ModContent.ItemType<HallowFoliageItem>();
			DustType = DustID.GreenMoss;
		}
	}

	public class HallowShardstoneItem : AmbientHallowItem<HallowShardstoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Shardstone");
	}

	public class HallowShardstoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowShardstoneItem>();
			DustType = DustID.PinkCrystalShard;
		}
	}

	public class HallowCrystallineItem : AmbientHallowItem<HallowCrystallineTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hallowed Crystalline Stone");
	}

	public class HallowCrystallineTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HallowCrystallineItem>();
			DustType = DustID.PinkCrystalShard;
		}
	}

	public class HiveBlockAltItem : AmbientBlockItem<HiveBlockAltTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Hive Hexblock");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Hive, 1);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.Hive, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public class HiveBlockAltTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(94, 40, 117));
			ItemDrop = ModContent.ItemType<HiveBlockAltItem>();
			DustType = 7;
		}
	}

	public class RuneBlockItem : AmbientBlockItem<RuneBlockTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Arcane Rune Block");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.ArcaneRuneWall, 4);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.ArcaneRuneWall, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public class RuneBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(99, 45, 117));
			ItemDrop = ModContent.ItemType<RuneBlockItem>();
			DustType = DustID.CorruptionThorns;
		}
	}

	public class KrampusHornBlockItem : AmbientBlockItem<KrampusHornBlockTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Krampus Horn Block");

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.KrampusHornWallpaper, 4);
			recipe.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe.Register();

			Recipe recipe1 = Recipe.Create(ItemID.KrampusHornWallpaper, 1);
			recipe1.AddIngredient(this, 1);
			recipe1.AddTile(ModContent.TileType<Furniture.ForagerTableTile>());
			recipe1.Register();
		}
	}

	public class KrampusHornBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);
			AddMapEntry(new Color(200, 200, 200));
			ItemDrop = ModContent.ItemType<KrampusHornBlockItem>();
			DustType = DustID.Sand;
		}
	}

	public class WeatheredStoneItem : AmbientStoneItem<WeatheredStoneTile>
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Weathered Stone");
	}

	public class WeatheredStoneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);

			Main.tileMerge[Type][TileID.Grass] = true;
			Main.tileMerge[TileID.Grass][Type] = true;

			HitSound = SoundID.Tink;
			AddMapEntry(new Color(100, 100, 100));
			ItemDrop = ModContent.ItemType<WeatheredStoneItem>();
			DustType = DustID.Stone;
		}
	}
}
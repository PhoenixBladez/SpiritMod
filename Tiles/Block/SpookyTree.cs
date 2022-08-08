using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.Halloween.Biome;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Tiles.Block
{
	public class SpookyTree : ModTree
	{
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults() => GrowsOnTileId = new int[] { ModContent.TileType<HalloweenGrass>() };
		public override int CreateDust() => 1;
		public override int DropWood() => ItemID.SpookyWood;
		public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight) { }

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<SpookySapling>();
		}

		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpookyTree", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpookyTree_Tops", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpookyTree_Branches", AssetRequestMode.ImmediateLoad);

		public override bool Shake(int x, int y, ref bool createLeaves)
		{
			WeightedRandom<SpookyTreeShakeEffect> options = new WeightedRandom<SpookyTreeShakeEffect>();
			options.Add(SpookyTreeShakeEffect.None, 1f);
			options.Add(SpookyTreeShakeEffect.Acorn, 0.8f);
			options.Add(SpookyTreeShakeEffect.NPC, 0.3f);
			options.Add(SpookyTreeShakeEffect.Gore, 0.5f);
			options.Add(SpookyTreeShakeEffect.Fruit, 0.6f);

			SpookyTreeShakeEffect effect = options;
			if (effect == SpookyTreeShakeEffect.Acorn)
			{
				Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16 + offset, ItemID.Acorn, Main.rand.Next(1, 3));
			}
			else if (effect == SpookyTreeShakeEffect.NPC)
			{
				WeightedRandom<int> npcType = new WeightedRandom<int>();
				npcType.Add(NPCID.Raven, 1);
				npcType.Add(NPCID.Splinterling, 0.01f);

				Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
				Vector2 pos = new Vector2(x * 16, y * 16) + offset;
				NPC.NewNPC(WorldGen.GetItemSource_FromTreeShake(x, y), (int)pos.X, (int)pos.Y, npcType);
			}
			else if (effect == SpookyTreeShakeEffect.Gore)
			{
				WeightedRandom<int> goreType = new WeightedRandom<int>();
				goreType.Add(473, 1); //Small Mourning Wood gore
				goreType.Add(471, 0.6f); //Large Mourning Wood gore

				Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
				Gore.NewGore(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16 + offset, Vector2.Zero, goreType);
			}
			else if (effect == SpookyTreeShakeEffect.Fruit)
			{
				WeightedRandom<int> getRepeats = new WeightedRandom<int>();
				getRepeats.Add(1, 1f);
				getRepeats.Add(2, 0.2f);
				getRepeats.Add(4, 0.1f);
				getRepeats.Add(8, 0.01f);

				int repeats = getRepeats;
				for (int i = 0; i < repeats; ++i)
				{
					Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
					Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16 + offset, Main.rand.NextBool() ? ModContent.ItemType<TreeGourd>() : ModContent.ItemType<CaramelApple>(), 1);
				}
			}

			createLeaves = effect != SpookyTreeShakeEffect.None;
			return false;
		}
	}

	public enum SpookyTreeShakeEffect
	{
		None = 0,
		Acorn,
		NPC,
		Gore,
		Fruit
	}
}
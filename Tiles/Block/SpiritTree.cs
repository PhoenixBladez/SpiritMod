using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.ByBiome.Spirit.Consumables;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.NPCs.Spirit;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Tiles.Block
{
	public class SpiritTree : ModTree
	{
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults() => GrowsOnTileId = new int[] { ModContent.TileType<SpiritGrass>() };
		public override int CreateDust() => 1;
		public override int DropWood() => ModContent.ItemType<SpiritWoodItem>();

		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritTree", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritTree_Tops", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/SpiritTree_Branches", AssetRequestMode.ImmediateLoad);

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<SpiritSapling>();
		}

		public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
		{
			topTextureFrameWidth = 114;
			topTextureFrameHeight = 96;
		}

		public override bool Shake(int x, int y, ref bool createLeaves)
		{
			WeightedRandom<SpiritTreeShakeEffect> options = new WeightedRandom<SpiritTreeShakeEffect>();
			options.Add(SpiritTreeShakeEffect.None, 0.8f);
			options.Add(SpiritTreeShakeEffect.Acorn, 0.8f);
			options.Add(SpiritTreeShakeEffect.Critter, 0.6f);
			options.Add(SpiritTreeShakeEffect.Fruit, 0.4f);

			SpiritTreeShakeEffect effect = options;
			if (effect == SpiritTreeShakeEffect.Acorn)
			{
				Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16 + offset, ItemID.Acorn, Main.rand.Next(1, 3));
			}
			else if (effect == SpiritTreeShakeEffect.Critter)
			{
				int repeats = Main.rand.Next(1, 4);

				for (int i = 0; i < repeats; ++i)
				{
					Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
					Vector2 pos = new Vector2(x * 16, y * 16) + offset;
					int npc = NPC.NewNPC(WorldGen.GetItemSource_FromTreeShake(x, y), (int)pos.X, (int)pos.Y, ModContent.NPCType<SoulOrb>());
					Main.npc[npc].velocity = new Vector2(Main.rand.NextFloat(2, 5), 0).RotatedByRandom(MathHelper.TwoPi);
				}
			}
			else if (effect == SpiritTreeShakeEffect.Fruit)
			{
				WeightedRandom<int> getRepeats = new WeightedRandom<int>();
				getRepeats.Add(1, 1f);
				getRepeats.Add(2, 0.5f);
				getRepeats.Add(3, 0.33f);
				getRepeats.Add(6, 0.01f);

				int repeats = getRepeats;
				for (int i = 0; i < repeats; ++i)
				{
					Vector2 offset = this.GetRandomTreePosition(Main.tile[x, y]);
					Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16 + offset, Main.rand.NextBool() ? ModContent.ItemType<LuminBerry>() : ModContent.ItemType<Glowpear>(), 1);
				}
			}

			//createLeaves = effect != SpiritTreeShakeEffect.None;
			return false;
		}
	}

	public enum SpiritTreeShakeEffect
	{
		None = 0,
		Acorn,
		Critter,
		Gore,
		Fruit,
	}
}
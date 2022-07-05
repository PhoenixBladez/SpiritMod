using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Gores;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.NPCs.Reach;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Tiles.Block
{
	public class ReachTree : ModTree
	{
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings 
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults() => GrowsOnTileId = new int[] { ModContent.TileType<BriarGrass>() };
		public override int CreateDust() => 22;
		public override int TreeLeaf() => SpiritMod.Instance.Find<ModGore>("GreenLeaf").Type;
		public override int DropWood() => ModContent.ItemType<AncientBark>();
		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/ReachTree");
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/ReachTree_Tops");
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/ReachTree_Branches");

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<ReachSapling>();
		}

		public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
		{
			topTextureFrameWidth = 142;
			topTextureFrameHeight = 114;
			xoffset = 62;
			floorY = 2;
		}


		public override bool Shake(int x, int y, ref bool createLeaves)
		{
			WeightedRandom<ReachTreeShakeEffect> options = new WeightedRandom<ReachTreeShakeEffect>();
			options.Add(ReachTreeShakeEffect.None, 1f);
			options.Add(ReachTreeShakeEffect.Acorn, 0.8f);
			options.Add(ReachTreeShakeEffect.Critter, 0.3f);
			options.Add(ReachTreeShakeEffect.Gore, 0.5f);

			ReachTreeShakeEffect effect = options;
			if (effect == ReachTreeShakeEffect.Acorn)
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, ItemID.Acorn, Main.rand.Next(1, 3));
			else if (effect == ReachTreeShakeEffect.Critter)
			{
				WeightedRandom<int> npcType = new WeightedRandom<int>();
				npcType.Add(ModContent.NPCType<Briarmoth>(), 1);
				npcType.Add(ModContent.NPCType<Blubby>(), 1.5f);
				npcType.Add(ModContent.NPCType<Briarmoth>(), 0.8f);

				int repeats = Main.rand.Next(1, 4);

				for (int i = 0; i < repeats; ++i)
					NPC.NewNPC(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, npcType);
			}
			else if (effect == ReachTreeShakeEffect.Gore)
			{
				WeightedRandom<int> goreType = new WeightedRandom<int>();
				goreType.Add(SpiritMod.Instance.Find<ModGore>("Reach1").Type, 1);
				goreType.Add(SpiritMod.Instance.Find<ModGore>("Reach2").Type, 3f);
				goreType.Add(SpiritMod.Instance.Find<ModGore>("BlossomHound2").Type, 0.8f);

				Gore.NewGore(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, Vector2.Zero, goreType);
			}

			createLeaves = effect != ReachTreeShakeEffect.None;
			return false;
		}
	}

	public enum ReachTreeShakeEffect
	{
		None = 0,
		Acorn,
		Critter,
		Gore,
	}
}
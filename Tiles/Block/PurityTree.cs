using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.ByBiome.Briar.Consumables;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.NPCs.Reach;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Tiles.Block
{
	/// <summary>This class serves to clone the vanilla Purity tree in order to use it in more places.</summary>
	public class PurityTree : ModTree
	{
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults() => GrowsOnTileId = new int[] { ModContent.TileType<Stargrass>() };
		public override int CreateDust() => DustID.WoodFurniture;
		public override int TreeLeaf() => GoreID.TreeLeaf_Normal;
		public override int DropWood() => ItemID.Wood;
		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/PurityTree");
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/PurityTree_Tops");
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/PurityTree_Branches");

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return TileID.Saplings;
		}

		public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
		{
			//topTextureFrameWidth = 142;
			//topTextureFrameHeight = 114;
		}
	}
}
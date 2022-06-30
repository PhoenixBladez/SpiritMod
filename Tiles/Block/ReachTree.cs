using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

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

		public override void SetStaticDefaults() => GrowsOnTileId = new int[] { ModContent.TileType<ReachGrassTile>() };
		public override int CreateDust() => 22;
		public override int DropWood() => ModContent.ItemType<AncientBark>();
		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("Tiles/Block/ReachTree");
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("Tiles/Block/ReachTree_Tops");
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("Tiles/Block/ReachTree_Branches");

		public override void SetTreeFoliageSettings(Tile tile, int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
		{
			topTextureFrameWidth = 114;
			topTextureFrameHeight = 116;
			xoffset = 64;
			floorY = 2;
		}
	}
}
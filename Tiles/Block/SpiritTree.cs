using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

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

		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("Tiles/Block/SpiritTree", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("Tiles/Block/SpiritTree_Tops", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("Tiles/Block/SpiritTree_Branches", AssetRequestMode.ImmediateLoad);

		public override void SetTreeFoliageSettings(Tile tile, int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
		{
			topTextureFrameWidth = 114;
			topTextureFrameHeight = 96;
		}
	}
}
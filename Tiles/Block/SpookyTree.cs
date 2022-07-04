using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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

		public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>("Tiles/Block/SpookyTree", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>("Tiles/Block/SpookyTree_Tops", AssetRequestMode.ImmediateLoad);
		public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>("Tiles/Block/SpookyTree_Branches", AssetRequestMode.ImmediateLoad);

	}
}
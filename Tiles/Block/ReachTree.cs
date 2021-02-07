using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class ReachTree : ModTree
	{
		private Mod mod {
			get {
				return ModLoader.GetMod("SpiritMod");
			}
		}

		public override int CreateDust()
		{
			return 22;
		}

		public override int DropWood()
		{
			return ModContent.ItemType<AncientBark>();
		}

		public override Texture2D GetTexture()
		{
			return mod.GetTexture("Tiles/Block/ReachTree");
		}

		public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            frameWidth = 144;
            frameHeight = 116;
            xOffsetLeft = 62;
            yOffset = 2;
            return mod.GetTexture("Tiles/Block/ReachTree_Tops");
		}

		public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
		{
			return mod.GetTexture("Tiles/Block/ReachTree_Branches");
		}
	}
}
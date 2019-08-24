using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class ReachTree : ModTree
	{
		private Mod mod
		{
			get
			{
				return ModLoader.GetMod("SpiritMod");
			}
		}

		public override int CreateDust()
		{
			return 1;
		}

		public override int DropWood()
		{
			return mod.ItemType("AncientBark");
		}

		public override Texture2D GetTexture()
		{
			return mod.GetTexture("Tiles/Block/ReachTree");
		}

		public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
		{
			return mod.GetTexture("Tiles/Block/ReachTree_Tops");
		}

		public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
		{
			return mod.GetTexture("Tiles/Block/ReachTree_Branches");
		}
	}
}
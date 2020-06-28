
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class GoblinStandardTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Goblin Battle Standard");
			dustType = 1;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, 362);
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, 362);
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, 362);
		}
	}
}
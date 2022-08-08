using SpiritMod.Tiles.Ambient.Forest;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Vanilla
{
	internal class CloudRandomUpdate
	{
		public static void OnTick(int i, int j, int type)
		{
			if (Main.tile[i, j + 1].HasTile)
				return;

			float chance = 0.2f;
			if (type == TileID.RainCloud)
				chance = 0.25f;
			else if (type == TileID.SnowCloud)
				chance = 0.05f;

			int y = j + 1;

			while (!Main.tile[i, y].HasTile)
				y++;

			Tile ground = Main.tile[i, y];
			if (Main.rand.NextFloat() < chance && (ground.TileType == TileID.Grass || ground.TileType == TileID.HallowedGrass) && !ground.TopSlope)
			{
				WorldGen.PlaceTile(i, y - 1, ModContent.TileType<Cloudstalk>(), true, false);
				NetMessage.SendObjectPlacment(-1, i, y - 1, ModContent.TileType<Cloudstalk>(), 0, 0, -1, -1);
			}
		}
	}
}

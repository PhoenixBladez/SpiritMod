using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class ReachGrassTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new ReachTree());
			Main.tileMerge[Type][ModContent.TileType<ReachGrassTile>()] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
			AddMapEntry(new Color(104, 156, 70));
			drop = ModContent.ItemType<ReachGrass>();
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if(!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false)) {
				return false;
			}
			toBePlaced.random = random;
			if(TileObject.Place(toBePlaced) && !mute) {
				WorldGen.SquareTileFrame(x, y, true);
				//   Main.PlaySound(0, x * 16, y * 16, 1, 1f, 0f);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if(!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(4) == 0) {
				switch(Main.rand.Next(7)) {
					case 0:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA1>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA1>(), 0, 0, -1, -1);
						break;
					case 1:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA2>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA2>(), 0, 0, -1, -1);
						break;
					case 2:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA3>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA3>(), 0, 0, -1, -1);
						break;
					case 3:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA4>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA4>(), 0, 0, -1, -1);
						break;
					case 4:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA5>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA5>(), 0, 0, -1, -1);
						break;
					case 5:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA6>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA6>(), 0, 0, -1, -1);
						break;

					default:
						PlaceObject(i, j - 1, ModContent.TileType<ReachGrassA7>());
						NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<ReachGrassA7>(), 0, 0, -1, -1);
						break;
				}

			}
			{
				if(!Framing.GetTileSafely(i, j + 1).active() && Main.rand.Next(4) == 0) {
					PlaceObject(i, j + 1, ModContent.TileType<Vine1>());
					NetMessage.SendObjectPlacment(-1, i, j + 1, ModContent.TileType<Vine1>(), 0, 0, -1, -1);
				}
				if(!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i, j + 2).active() && Main.rand.Next(16) == 0) {
					PlaceObject(i, j + 1, ModContent.TileType<Vine2>());
					NetMessage.SendObjectPlacment(-1, i, j + 1, ModContent.TileType<Vine2>(), 0, 0, -1, -1);
				}
			}
		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<ReachSapling>();
		}

	}
}


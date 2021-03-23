using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Tiles.Ambient;

namespace SpiritMod.Tiles.Block
{
	public class HalloweenGrass : ModTile
	{

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new SpookyTree());
			Main.tileMerge[Type][ModContent.TileType<HalloweenGrass>()] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
			AddMapEntry(new Color(252, 161, 3));
			drop = ModContent.ItemType<Items.Placeable.Tiles.HalloweenGrass>();
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if (!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false)) {
				return false;
			}
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute) {
				WorldGen.SquareTileFrame(x, y, true);
				//   Main.PlaySound(SoundID.Dig, x * 16, y * 16, 1, 1f, 0f);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(20) == 0) {
				int style = Main.rand.Next(23);
				if (PlaceObject(i, j - 1, ModContent.TileType<SpookyFoliage>(), false, style))
					NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<SpookyFoliage>(), style, 0, -1, -1);
			}
			//else if (Main.rand.Next(100) == 0)
			//{
			//	if (Framing.GetTileSafely(i-1, j).type == _type &&
			//		!Framing.GetTileSafely(i-1, j-1).active()&&
			//		!Framing.GetTileSafely(i, j-1).active()&&
			//		!Framing.GetTileSafely(i-1, j-2).active()&&
			//		!Framing.GetTileSafely(i, j-2).active())
			//	{

			//	}
			//}
		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<SpookySapling>();
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.028f;
			g = 0.153f;
			b = 0.081f;
		}
	}
}


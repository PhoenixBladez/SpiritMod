using SpiritMod.Tiles.Ambient.Kelp;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Sets.FloatingItems.Driftwood;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Ambient.Ocean
{
	public class OceanGlobalTile : GlobalTile
	{
		public override void RandomUpdate(int i, int j, int type)
		{
			int[] sands = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand, ModContent.TileType<Spiritsand>() }; //All valid sands
			int[] woods = new int[] { TileID.WoodBlock, TileID.BorealWood, TileID.Ebonwood, TileID.DynastyWood, TileID.RichMahogany, TileID.PalmWood, TileID.Shadewood, TileID.WoodenBeam,
				ModContent.TileType<SpiritWood>(), ModContent.TileType<LivingBriarWood>(), ModContent.TileType<DriftwoodTile>(), TileID.Pearlwood };

			bool inOcean = (i < Main.maxTilesX / 16 || i > (Main.maxTilesX / 16) * 15) && j < (int)Main.worldSurface; //Might need adjustment; don't know if this will be exclusively in the ocean

			if (sands.Contains(type) && inOcean && !Framing.GetTileSafely(i, j - 1).active() && !Framing.GetTileSafely(i, j).topSlope()) //woo
			{
				if (Framing.GetTileSafely(i, j - 1).liquid > 200) //water stuff
				{
					if (Main.rand.NextBool(25))
						WorldGen.PlaceTile(i, j - 1, ModContent.TileType<OceanKelp>()); //Kelp spawning

					bool openSpace = !Framing.GetTileSafely(i, j - 2).active();
					if (openSpace && Main.rand.NextBool(40)) //1x2 kelp
						WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Kelp1x2>());

					openSpace = !Framing.GetTileSafely(i + 1, j - 1).active() && !Framing.GetTileSafely(i + 1, j - 2).active() && !Framing.GetTileSafely(i, j - 2).active();
					if (openSpace && Framing.GetTileSafely(i + 1, j).active() && Main.tileSolid[Framing.GetTileSafely(i + 1, j).type] && Framing.GetTileSafely(i + 1, j).topSlope() && Main.rand.NextBool(80)) //2x2 kelp
						WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Kelp2x2>());

					openSpace = !Framing.GetTileSafely(i + 1, j - 1).active() && !Framing.GetTileSafely(i + 1, j - 2).active() && !Framing.GetTileSafely(i, j - 2).active() && !Framing.GetTileSafely(i + 1, j - 3).active() && !Framing.GetTileSafely(i, j - 3).active();
					if (openSpace && Framing.GetTileSafely(i + 1, j).active() && Main.tileSolid[Framing.GetTileSafely(i + 1, j).type] && Framing.GetTileSafely(i + 1, j).topSlope() && Main.rand.NextBool(90)) //2x3 kelp
						WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Kelp2x3>());
				}
				else
				{
					if (Main.rand.NextBool(6))
						WorldGen.PlaceTile(i, j - 1, ModContent.TileType<Seagrass>(), true, true, -1, Main.rand.Next(16));
				}

				for (int k = i - 1; k < i + 2; ++k)
				{
					for (int l = j - 1; l < j + 2; ++l)
					{
						if (k == i && l == j) continue; //Dont check myself

						Tile cur = Framing.GetTileSafely(k, l);
						if (!cur.active() && woods.Contains(cur.type) && cur.liquid > 155 && cur.liquidType() == Tile.Liquid_Water && Main.rand.NextBool(6))
							WorldGen.PlaceTile(k, l, ModContent.TileType<Mussel>());
					}
				}
			}
		}
	}
}

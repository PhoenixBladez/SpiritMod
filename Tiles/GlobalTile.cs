using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.NPCs.OceanSlime;
using SpiritMod.Items.Consumable.Food;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Mechanics.Fathomless_Chest;
using SpiritMod.Tiles.Ambient.Kelp;

namespace SpiritMod.Tiles
{
	public class GTile : GlobalTile
	{
		public int tremorItem = 0;

		static ushort IceType1 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceWheezerHostile>();
		static ushort IceType2 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceWheezerPassive>();
		static ushort IceType3 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceVikingHostile>();
		static ushort IceType4 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceVikingPassive>();
		static ushort IceType5 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceBatHostile>();
		static ushort IceType6 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceBatPassive>();
		static ushort IceType7 = (ushort)ModContent.TileType<Ambient.IceSculpture.Hostile.IceFlinxHostile>();
		static ushort IceType8 = (ushort)ModContent.TileType<Ambient.IceSculpture.IceFlinxPassive>();

		readonly int[] indestructibletiles = new int[] { ModContent.TileType<StarBeacon>(), ModContent.TileType<AvianEgg>(), ModContent.TileType<Fathomless_Chest>(), ModContent.TileType<BloodBlossom>() };
		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type)
				return false;

			else if (type != IceType1 && tileAbove.type == IceType1) {
				return false;
			}
			else if (type != IceType2 && tileAbove.type == IceType2) {
				return false;
			}
			else if (type != IceType3 && tileAbove.type == IceType3) {
				return false;
			}
			else if (type != IceType4 && tileAbove.type == IceType4) {
				return false;
			}
			else if (type != IceType5 && tileAbove.type == IceType5) {
				return false;
			}
			else if (type != IceType6 && tileAbove.type == IceType6) {
				return false;
			}
			else if (type != IceType7 && tileAbove.type == IceType7) {
				return false;
			}
			else if (type != IceType8 && tileAbove.type == IceType8) {
				return false;
			}
			return base.CanKillTile(i, j, type, ref blockDamaged);
		}
		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type && TileID.Sets.Falling[type]) {
				//add something here to make the frame of the tile reset properly
				return false;
			}

			return base.TileFrame(i, j, type, ref resetFrame, ref noBreak);
		}

		public override bool PreHitWire(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type) {
				Main.tile[i, j].inActive(false);
			}
			return true;
		}

		public virtual bool Slope(int i, int j, int type)
        {
            Tile tileAbove = Framing.GetTileSafely(i, j - 1);
            Tile tileRight = Framing.GetTileSafely(i + 1, j - 1);
            Tile tileLeft = Framing.GetTileSafely(i - 1, j - 1);
            ushort eggType = (ushort)ModContent.TileType<Ambient.AvianEgg>();
			ushort flowerType = (ushort)ModContent.TileType<BloodBlossom>();
			if (type == flowerType || tileAbove.type == flowerType) {
				return false;
			}
			else if (type == eggType || tileAbove.type == eggType) {
				return false;
			}
			else if (type != IceType1 && tileAbove.type == IceType1) {
				return false;
			}
			else if (type != IceType2 && tileAbove.type == IceType2) {
				return false;
			}
			else if (type != IceType3 && tileAbove.type == IceType3) {
				return false;
			}
			else if (type != IceType4 && tileAbove.type == IceType4) {
				return false;
			}
			else if (type != IceType5 && tileAbove.type == IceType5) {
				return false;
			}
			else if (type != IceType6 && tileAbove.type == IceType6) {
				return false;
			}
			else if (type != IceType7 && tileAbove.type == IceType7) {
				return false;
			}
			else if (type != IceType8 && tileAbove.type == IceType8) {
				return false;
			}
            return base.Slope(i, j, type);
        }
        public override bool CanExplode(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type)
				return false;

			if (type != IceType1 && tileAbove.type == IceType1) {
				return false;
			}
			else if (type != IceType2 && tileAbove.type == IceType2) {
				return false;
			}
			else if (type != IceType3 && tileAbove.type == IceType3) {
				return false;
			}
			else if (type != IceType4 && tileAbove.type == IceType4) {
				return false;
			}
			else if (type != IceType5 && tileAbove.type == IceType5) {
				return false;
			}
			else if (type != IceType6 && tileAbove.type == IceType6) {
				return false;
			}
			else if (type != IceType7 && tileAbove.type == IceType7) {
				return false;
			}
			else if (type != IceType8 && tileAbove.type == IceType8) {
				return false;
			}
			return base.CanExplode(i, j, type);
		}

		int[] TileArray212 = { TileID.Dirt, TileID.Plants, TileID.SmallPiles, TileID.LargePiles, TileID.LargePiles2, TileID.MushroomPlants, TileID.Pots };

		public override void RandomUpdate(int i, int j, int type)
		{
			int[] sands = new int[] { TileID.Sand, TileID.Crimsand, TileID.Ebonsand }; //All valid sands

			bool inOcean = (i < Main.maxTilesX / 16 || i > (Main.maxTilesX / 16) * 15) && j < (int)Main.worldSurface; //Might need adjustment; don't know if this will be exclusively in the ocean

			bool inLavaLayer = j > (int)Main.rockLayer && j < (int)Main.maxTilesY - 250; 
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);	

			if (sands.Contains(type) && inOcean && !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j - 1).liquid > 155 && !Framing.GetTileSafely(i, j).topSlope()) //woo
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


			if (type == TileID.Pearlstone && inLavaLayer)
			{
				if (WorldGen.genRand.NextBool(20) && !tileBelow.active() && !tileBelow.lava()) {
					if (!tile.bottomSlope()) {
						tileBelow.type = (ushort)ModContent.TileType<Tiles.Ambient.HangingChimes.HangingChimes>();
						tileBelow.active(true);
						WorldGen.SquareTileFrame(i, j + 1, true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
						}
					}
				}
			}
			if (type == TileID.CorruptGrass || type == TileID.Ebonstone) {
				if (MyWorld.CorruptHazards < 20) {
					if ((TileArray212.Contains(Framing.GetTileSafely(i, j - 1).type) && TileArray212.Contains(Framing.GetTileSafely(i, j - 2).type) && TileArray212.Contains(Framing.GetTileSafely(i, j - 3).type)) && (j > (int)Main.worldSurface - 100 && j < (int)Main.rockLayer - 20)) {
						if (Main.rand.Next(450) == 0) {
							WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Corpsebloom>());
							NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<Corpsebloom>(), 0, 0, -1, -1);
						}
						if (Main.rand.Next(450) == 0) {
							WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Corpsebloom1>());
							NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<Corpsebloom1>(), 0, 0, -1, -1);
						}
						if (Main.rand.Next(450) == 0) {
							WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Corpsebloom2>());
							NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<Corpsebloom2>(), 0, 0, -1, -1);
						}
					}
				}
			}
		}
		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
            if (!Main.dedServ)
            {
                Player player = Main.LocalPlayer;
                MyPlayer modPlayer = player.GetSpiritPlayer();
                if (type == 1 || type == 25 || type == 117 || type == 203 || type == 57)
                {
                    if (Main.rand.Next(50) == 1 && modPlayer.gemPickaxe)
                    {
                        tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181 });
                        if (Main.hardMode)
                        {
                            tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181, 364, 365, 366, 1104, 1105, 1106 });
                        }
                        Item.NewItem(i * 16, j * 16, 64, 48, tremorItem, Main.rand.Next(1, 3));
                    }
                }
                if (player.GetSpiritPlayer().wayfarerSet && type == 28)
                {
                    player.AddBuff(ModContent.BuffType<Buffs.Armor.ExplorerPot>(), 360);
                }
                if (player.GetSpiritPlayer().wayfarerSet && Main.tileSpelunker[type] && Main.tileSolid[type])
                {
                    player.AddBuff(ModContent.BuffType<Buffs.Armor.ExplorerMine>(), 600);
                }

                if (player.HasItem(ModContent.ItemType<Spineshot>()))
                {
                    if (type == 3 || type == 24 || type == 61 || type == 71 || type == 110 || type == 201)
                    {
                        Item.NewItem(i * 16, j * 16, 64, 48, ItemID.Seed, Main.rand.Next(1, 3));
                    }
                }
				if (player.cordage && type == ModContent.TileType<Tiles.Ambient.Briar.BriarVines>())
				{
					Item.NewItem(i * 16, j * 16, 64, 48, ItemID.VineRope);
				}
				if (player.inventory[player.selectedItem].type == ItemID.Sickle && (type == ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage>() || type == ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage1>()))
				{
                    {
                        Item.NewItem(i * 16, j * 16, 64, 48, ItemID.Hay);
                    }				
				}
            }
		}
		public override void FloorVisuals(int type, Player player)
		{
			foreach (var effect in player.GetSpiritPlayer().effects)
				effect.TileFloorVisuals(type, player);

			if (type == TileID.Sand && player.GetSpiritPlayer().scarabCharm) {
				player.jumpSpeedBoost += .15f;
			}
		}
		public override bool Drop(int i, int j, int type)
        {
            if (!Main.dedServ)
            {
                Player player = Main.LocalPlayer;
                MyPlayer modPlayer = player.GetSpiritPlayer();
                if (type == TileID.PalmTree && Main.rand.Next(3) == 0 && player.ZoneBeach)
                {
                    if (Main.rand.Next(2) == 1)
                    {
                        Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<Coconut>(), Main.rand.Next(5, 8));
                    }
                    if (NPC.CountNPCS(ModContent.NPCType<OceanSlime>()) < 1)
                    {
                        NPC.NewNPC(i * 16, (j - 10) * 16, ModContent.NPCType<OceanSlime>(), 0, 0.0f, -8.5f, 0.0f, 0.0f, (int)byte.MaxValue);
                    }
                }

                if (type == 72)
                {
                    Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<GlowRoot>(), Main.rand.Next(0, 2));
                }
                if (type == TileID.Trees && Main.rand.Next(25) == 0 && player.ZoneSnow)
                {
                    if (Main.rand.Next(4) == 1)
                    {
                        Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<IceBerries>(), Main.rand.Next(1, 3));
                    }
                }
            }
			return base.Drop(i, j, type);
		}
	}
}
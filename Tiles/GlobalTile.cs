using SpiritMod.Items.Material;
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
using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.IceSculpture.Hostile;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.Ocean;

namespace SpiritMod.Tiles
{
	public class GTile : GlobalTile
	{
		static readonly int[] IceSculptures = new int[] { ModContent.TileType<IceWheezerHostile>(), ModContent.TileType<IceVikingHostile>(), ModContent.TileType<IceFlinxHostile>(), ModContent.TileType<IceBatHostile>(),
			ModContent.TileType<IceWheezerPassive>(), ModContent.TileType<IceVikingPassive>(), ModContent.TileType<IceBatPassive>(), ModContent.TileType<IceFlinxPassive>() };

		readonly int[] indestructibletiles = new int[] { ModContent.TileType<StarBeacon>(), ModContent.TileType<AvianEgg>(), ModContent.TileType<Fathomless_Chest>(), ModContent.TileType<BloodBlossom>() };

		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type)
				return false;
			else if (IceSculptures.Contains(type) || IceSculptures.Contains(tileAbove.type))
				return false;

			return true;
		}

		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type && TileID.Sets.Falling[type])
			{
				//add something here to make the frame of the tile reset properly
				return false;
			}
			return true;
		}

		public override bool PreHitWire(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type)
				Main.tile[i, j].inActive(false);
			return true;
		}

		public override bool Slope(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);
			ushort flowerType = (ushort)ModContent.TileType<BloodBlossom>();
			if (type == flowerType || tileAbove.type == flowerType)
				return false;
			else if (IceSculptures.Contains(type) || IceSculptures.Contains(tileAbove.type))
				return false;
			return true;
		}

		public override bool CanExplode(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (indestructibletiles.Contains(tileAbove.type) && type != tileAbove.type)
				return false;
			if (IceSculptures.Contains(type) || IceSculptures.Contains(tileAbove.type))
				return false;
			return base.CanExplode(i, j, type);
		}

		readonly int[] DirtAndDecor = { TileID.Dirt, TileID.Plants, TileID.SmallPiles, TileID.LargePiles, TileID.LargePiles2, TileID.MushroomPlants, TileID.Pots };

		public override void RandomUpdate(int i, int j, int type)
		{
			bool inLavaLayer = j > (int)Main.rockLayer && j < Main.maxTilesY - 250;
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);

			if (type == TileID.Pearlstone && inLavaLayer)
			{
				if (WorldGen.genRand.NextBool(20) && !tileBelow.active() && !tileBelow.lava())
				{
					if (!tile.bottomSlope())
					{
						tileBelow.type = (ushort)ModContent.TileType<Ambient.HangingChimes.HangingChimes>();
						tileBelow.active(true);
						WorldGen.SquareTileFrame(i, j + 1, true);
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
					}
				}
			}

			if (type == TileID.CorruptGrass || type == TileID.Ebonstone)
			{
				if (MyWorld.CorruptHazards < 20)
				{
					if (DirtAndDecor.Contains(Framing.GetTileSafely(i, j - 1).type) && DirtAndDecor.Contains(Framing.GetTileSafely(i, j - 2).type) && DirtAndDecor.Contains(Framing.GetTileSafely(i, j - 3).type) && (j > (int)Main.worldSurface - 100 && j < (int)Main.rockLayer - 20))
					{
						if (Main.rand.Next(450) == 0)
						{
							WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Corpsebloom>());
							NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<Corpsebloom>(), 0, 0, -1, -1);
						}
						if (Main.rand.Next(450) == 0)
						{
							WorldGen.PlaceObject(i, j - 1, ModContent.TileType<Corpsebloom1>());
							NetMessage.SendObjectPlacment(-1, i, j - 1, ModContent.TileType<Corpsebloom1>(), 0, 0, -1, -1);
						}
						if (Main.rand.Next(450) == 0)
						{
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

				if (type == TileID.Stone || type == 25 || type == 117 || type == 203 || type == 57)
				{
					if (Main.rand.Next(25) == 1 && modPlayer.gemPickaxe && !fail)
					{
						int tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181 });
						if (Main.hardMode)
							tremorItem = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702, 999, 182, 178, 179, 177, 180, 181, 364, 365, 366, 1104, 1105, 1106 });

						Main.PlaySound(SoundLoader.customSoundType, new Vector2(i * 16, j * 16), mod.GetSoundSlot(SoundType.Custom, "Sounds/PositiveOutcome"));
						Item.NewItem(i * 16, j * 16, 64, 48, tremorItem, Main.rand.Next(1, 3));
					}
				}

				if (player.GetSpiritPlayer().wayfarerSet && type == 28)
					player.AddBuff(ModContent.BuffType<Buffs.Armor.ExplorerPot>(), 360);
				if (player.GetSpiritPlayer().wayfarerSet && Main.tileSpelunker[type] && Main.tileSolid[type])
					player.AddBuff(ModContent.BuffType<Buffs.Armor.ExplorerMine>(), 600);

				if (player.cordage && type == ModContent.TileType<Tiles.Ambient.Briar.BriarVines>())
					Item.NewItem(i * 16, j * 16, 64, 48, ItemID.VineRope);
				if (Main.rand.NextBool(40) && type == ModContent.TileType<Tiles.Ambient.HangingChimes.HangingChimes>())
					Item.NewItem(i * 16, j * 16, 64, 48, ItemID.CrystalShard);
				if (player.inventory[player.selectedItem].type == ItemID.Sickle && (type == ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage>() || type == ModContent.TileType<Tiles.Ambient.Briar.BriarFoliage1>()))
					Item.NewItem(i * 16, j * 16, 64, 48, ItemID.Hay);
			}
		}

		public override void FloorVisuals(int type, Player player)
		{
			foreach (var effect in player.GetSpiritPlayer().effects)
				effect.TileFloorVisuals(type, player);

			if (type == TileID.Sand && player.GetSpiritPlayer().scarabCharm)
				player.jumpSpeedBoost += .15f;
		}

		public override bool Drop(int i, int j, int type)
		{
			if (!Main.dedServ)
			{
				Player player = Main.LocalPlayer;
				if (type == TileID.PalmTree && Main.rand.Next(3) == 0 && player.ZoneBeach)
				{
					if (Main.rand.Next(2) == 1)
						Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<Coconut>(), Main.rand.Next(5, 8));
					if (NPC.CountNPCS(ModContent.NPCType<OceanSlime>()) < 1)
						NPC.NewNPC(i * 16, (j - 10) * 16, ModContent.NPCType<OceanSlime>(), 0, 0.0f, -8.5f, 0.0f, 0.0f, (int)byte.MaxValue);
				}

				if (type == 72)
					Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<GlowRoot>(), Main.rand.Next(0, 2));
				if (type == TileID.Trees && Main.rand.Next(25) == 0 && player.ZoneSnow && Main.rand.Next(4) == 1)
					Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<IceBerries>(), Main.rand.Next(1, 3));
			}
			return true;
		}
	}
}
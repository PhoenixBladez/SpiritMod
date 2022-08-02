using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.ByBiome.Forest.Placeable.Decorative;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.Forest
{
	public enum PlantStage : byte
	{
		Planted,
		Growing,
		Grown
	}

	public class Cloudstalk : ModTile
	{
		private const int FrameWidth = 18; // A constant for readability and to kick out those magic numbers

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.IgnoredInHouseScore[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Example Herb");
			AddMapEntry(new Color(128, 128, 128), name);

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Grass, TileID.HallowedGrass };
			TileObjectData.newTile.AnchorAlternateTiles = new int[] { TileID.ClayPot, TileID.PlanterBox };
			TileObjectData.addTile(Type);

			HitSound = SoundID.Grass;
			DustType = DustID.Cloud;
		}

		public override bool CanPlace(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			if (tile.HasTile)
			{
				int tileType = tile.TileType;
				if (tileType == Type)
				{
					PlantStage stage = GetStage(i, j);
					return stage == PlantStage.Grown;
				}
				else
				{
					if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
					{
						bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
						bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
						bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX / 18);

						if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
						{
							WorldGen.KillTile(i, j);
							if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
							return true;
						}
					}

					return false;
				}
			}

			return true;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 0)
				spriteEffects = SpriteEffects.FlipHorizontally;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = -2;

		public override bool Drop(int i, int j)
		{
			PlantStage stage = GetStage(i, j);

			if (stage == PlantStage.Planted)
				return false;

			Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
			Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

			int herbItemStack = 0;
			int seedItemStack = 0;

			if (nearestPlayer.active && nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth) // Increased yields with Staff of Regrowth, even when not fully grown
			{
				if (stage == PlantStage.Grown)
					(herbItemStack, seedItemStack) = (2, Main.rand.Next(2, 5));
				else if (stage == PlantStage.Growing)
					seedItemStack = Main.rand.Next(1, 3);
			}
			else if (stage == PlantStage.Grown)
				(herbItemStack, seedItemStack) = (1, Main.rand.Next(1, 4));
			else if (stage == PlantStage.Growing)
				seedItemStack = 1;

			var source = new EntitySource_TileBreak(i, j);

			if (herbItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<CloudstalkItem>(), herbItemStack);
			if (seedItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<CloudstalkSeed>(), seedItemStack);
			return false;
		}

		public override bool IsTileSpelunkable(int i, int j) => GetStage(i, j) == PlantStage.Grown;

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			PlantStage stage = GetStage(i, j);

			// Only grow to the next stage if there is a next stage. We don't want our tile turning pink!
			if (stage != PlantStage.Grown)
			{
				// Increase the x frame to change the stage
				tile.TileFrameX += FrameWidth;

				// If in multiplayer, sync the frame change
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendTileSquare(-1, i, j, 1);
			}
		}

		// A helper method to quickly get the current stage of the herb (assuming the tile at the coordinates is our herb)
		private static PlantStage GetStage(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			return (PlantStage)(tile.TileFrameX / FrameWidth);
		}
	}
}
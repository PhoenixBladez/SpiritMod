using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor.BotanistSet;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
	public class SoulBloomTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			DustType = DustID.Flare_Blue;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				ModContent.TileType<Block.SpiritGrass>()
			};
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				TileID.ClayPot,
				TileID.PlanterBox
			};
			TileObjectData.addTile(Type);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

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
				herbItemStack = 1;

			if (nearestPlayer.GetModPlayer<BotanistPlayer>().active && stage != PlantStage.Planted)
			{
				seedItemStack += 2;
				herbItemStack++;
			}

			var source = new EntitySource_TileBreak(i, j);

			if (herbItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<SoulSeeds>(), herbItemStack);
			if (seedItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<SoulBloom>(), seedItemStack);
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (Main.tile[i, j].TileFrameX == 0) {
				Main.tile[i, j].TileFrameX += 18;
			}
			else if (Main.tile[i, j].TileFrameX == 18) {
				Main.tile[i, j].TileFrameX += 18;
			}
		}

		// A helper method to quickly get the current stage of the herb (assuming the tile at the coordinates is our herb)
		private static PlantStage GetStage(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			return (PlantStage)(tile.TileFrameX / 18);
		}
	}
}

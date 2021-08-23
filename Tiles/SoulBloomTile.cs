using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Tiles
{
	public class SoulBloomTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			dustType = DustID.Flare_Blue;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				TileType<Block.SpiritGrass>()
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
			int stage = Main.tile[i, j].frameX / 18;
			if (stage == 1) {
				Item.NewItem(i * 16, j * 16, 64, 32, ItemType<SoulBloom>());
			}
			if (stage == 2) {
				Item.NewItem(i * 16, j * 16, 64, 32, ItemType<SoulBloom>());
				Item.NewItem(i * 16, j * 16, 0, 0, ItemType<Items.Placeable.SoulSeeds>());
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (Main.tile[i, j].frameX == 0) {
				Main.tile[i, j].frameX += 18;
			}
			else if (Main.tile[i, j].frameX == 18) {
				Main.tile[i, j].frameX += 18;
			}
		}
	}
}

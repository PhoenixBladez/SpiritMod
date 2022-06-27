using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture
{
	public class CandyBowl : ModTile
	{
		public override void SetStaticDefaults()
		{
			AnimationFrameHeight = 36;
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Halloween.CandyBowl>());
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (Main.dayTime || Main.gameMenu) {
				frame = 0;
				return;
			}
			MyPlayer player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
			frame = player.candyInBowl > 0 ? 1 : 0;
		}

		public override void MouseOverFar(int i, int j)
		{
			if (Main.dayTime)
				return;

			Player player = Main.player[Main.myPlayer];
			if (player.GetModPlayer<MyPlayer>().candyInBowl <= 0)
				return;

			player.noThrow = 2;
			//player.showItemIcon = true;
			player.cursorItemIconText = "Take a piece of candy";
		}

		public override bool RightClick(int i, int j)
		{
			if (Main.dayTime)
				return false;

			Player player = Main.player[Main.myPlayer];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (modPlayer.candyInBowl <= 0)
				return false;

			ItemUtils.DropCandy(player);
			modPlayer.candyInBowl--;
			return true;
		}
	}
}
using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Reach;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Block
{
	public class RuneStone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 15;
			dustType = 107;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			animationFrameHeight = 108;
			AddMapEntry(new Color(200, 200, 200));


		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<ForestWraith>())) {
				Main.NewText("You have disturbed the ancient Nature Spirits!", 0, 170, 60);
				NPC.NewNPC(i * 16, j * 16, ModContent.NPCType<ForestWraith>(), 0, 2, 1, 0, 0, Main.myPlayer);
			}
		}
	}
}
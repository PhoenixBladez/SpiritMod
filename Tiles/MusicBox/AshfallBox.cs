using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Tiles.MusicBox
{
	internal class AshfallBox : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(200, 200, 200), name);
            DustType = -1;
        }

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ItemType<Items.Placeable.MusicBox.AshfallBox>());
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ItemType<Items.Placeable.MusicBox.AshfallBox>();
		}
	}
}

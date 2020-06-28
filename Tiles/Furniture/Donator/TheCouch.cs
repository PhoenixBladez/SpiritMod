using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Donator
{
	public class TheCouch : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.Direction = Terraria.Enums.TileObjectDirection.None;
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("The Couch");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
			bed = true;
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if(closer) {
				Player player = Main.player[Main.myPlayer];
				player.AddBuff(ModContent.BuffType<CouchPotato>(), 60, true);
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.DonatorItems.TheCouch>());
		}

	}
}
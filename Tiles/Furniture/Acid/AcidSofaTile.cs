using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidSofaTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			name.SetDefault("Corrosive Sofa");
			AddMapEntry(new Color(100, 122, 111), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = -1;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Terraria.Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidSofa>());
		}
	}
}
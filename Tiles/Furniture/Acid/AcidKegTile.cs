using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidKegTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Corrosive Keg");
			AddMapEntry(new Color(100, 122, 111), name);
			AdjTiles = new int[] { TileID.Kegs };
			DustType = -1;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(SoundID.NPCHit4);
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidKeg>());
		}
	}
}
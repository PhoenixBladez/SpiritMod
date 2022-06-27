using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	public class OlympiumToken_Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.addTile(Type);
			ItemDrop = ModContent.ItemType<Items.Sets.OlympiumSet.OlympiumToken>();
			AdjTiles = new int[] { TileID.MetalBars };
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Olympium Token");
			AddMapEntry(new Color(200, 200, 200), name);
			DustType = -1;
			soundType = SoundID.Coins;
		}
	}
}
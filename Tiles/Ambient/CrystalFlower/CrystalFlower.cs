using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.AccessoriesMisc.CrystalFlower;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.CrystalFlower
{
	public class CrystalFlower : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.AnchorAlternateTiles = new int[] { TileID.Cactus };
			TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.AlternateTile, 1, 0);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crystal Flower");
			AddMapEntry(new Color(9, 170, 219), name);

			mineResist = 1.2f;
			drop = ModContent.ItemType<CrystalFlowerItem>();
		}
	}
}

using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.IceSculpture
{
	[TileTag(TileTags.Indestructible)]
	public class IceBatPassive : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frozen Ice Bats");
			dustType = DustID.SnowBlock;
			AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;
		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => NPC.downedBoss3;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
			Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<CreepingIce>(), Main.rand.Next(6, 13));
		}
	}
}
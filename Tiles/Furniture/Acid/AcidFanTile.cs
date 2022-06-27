using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
namespace SpiritMod.Tiles.Furniture.Acid
{
	public class AcidFanTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			AnimationFrameHeight = 36;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType -= 1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Rusty Fan");
			AddMapEntry(new Color(100, 122, 111), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 6) //replace 10 with duration of frame in ticks
			{
				frameCounter = 0;
				frame++;
				frame %= 2;
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Placeable.Furniture.Acid.AcidFan>());
		}
	}
}
using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.SpaceCrystals
{
	public class PurpleShardBig : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile(Type);
			DustType = -3;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Giant Crystal");
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.7f / 4;
			g = 0.05f / 4;
			b = 0.9f / 4;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<RockCandy>());
			Player player = Main.LocalPlayer;
			SoundEngine.PlaySound(Terraria.ID.SoundID.Item27);
		}
	}
}
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Glyphs
{
	public class BloodGlyph : ModItem
	{
		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public override void LoadData(TagCompound tag)
		{
			Item.SetDefaults(ModContent.ItemType<SanguineGlyph>());
		}
	}
}
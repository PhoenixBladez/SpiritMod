using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Glyphs
{
	public class PoisonGlyph : ModItem
	{
		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public override void LoadData(TagCompound tag)
		{
			Item.SetDefaults(ModContent.ItemType<UnholyGlyph>());
		}
	}
}
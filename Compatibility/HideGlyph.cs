using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Glyphs
{
	public class HideGlyph : ModItem
	{
		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public override void Load(TagCompound tag)
		{
			item.SetDefaults(VeilGlyph._type);
		}
	}
}
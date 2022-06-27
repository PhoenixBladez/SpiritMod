using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class Glyph : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blank Glyph");
			Tooltip.SetDefault("'The Enchanter could probably use this'");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = 0;
			Item.rare = -11;
			Item.maxStack = 999;
		}
	}
}
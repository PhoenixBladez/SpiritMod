/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class HauntedGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Haunt;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override string Effect => "Haunting";
		public override string Addendum =>
			"Attacks cause fear inducing bats to sweep across the screen\n"+
			"You deal 8% additional damage to feared enemies";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Glyph");
			Tooltip.SetDefault("Wielding the weapon grants you Collapsing Void, which reduces damage taken by 5%\nLanding a critical hit on foes may grant you up to two additional stacks of collapsing void, which reduces damage taken by up to 15%\nHitting foes when having more than one stack of Collapsing Void may generate Void Stars");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = 7;

			item.maxStack = 999;
		}
	}
}*/
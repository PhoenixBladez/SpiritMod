using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class EfficiencyGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Efficiency;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x1759e8 };
		public override string ItemType => "tool";
		public override string Effect => "Efficiency";
		public override string Addendum =>
			"Tool speed and range are increased";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Efficiency Glyph");
			Tooltip.SetDefault(
				"Can only be applied to tools\n"+
				"Speed +30% and Range +2");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;

			item.maxStack = 999;
		}


		public override bool CanApply(Item item)
		{
			return item.pick > 0 || item.axe > 0 || item.hammer > 0;
		}
	}
}
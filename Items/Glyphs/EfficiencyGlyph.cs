using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Glyphs
{
	public class EfficiencyGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
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
				"Can only be applied to tools\n" +
				"+30% speed and +2 range");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;

			Item.maxStack = 999;
		}


		public override bool CanApply(Item item)
		{
			return item.pick > 0 || item.axe > 0 || item.hammer > 0;
		}
	}
}
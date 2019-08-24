using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public sealed class NullGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.None;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x9f9593 };
		public override string Effect => "Invalid Effect";
		public override string Addendum =>
			"Glyph not implemented!";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Null Glyph");
			Tooltip.SetDefault("Can be used to wipe a glyph off an item");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = 1;

			item.maxStack = 999;
		}


		public override bool CanApply(Item item)
		{
			return item.GetGlobalItem<GItem>().Glyph != GlyphType.None;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = tooltips.FindIndex(x => x.Name == "Tooltip0");
			if (index < 0)
				return;

			Player player = Main.player[Main.myPlayer];
			TooltipLine line;
			if (CanRightClick())
			{
				Item held = player.HeldItem;
				GlyphBase glyph = FromType(held.GetGlobalItem<GItem>().Glyph);
				Color color = glyph.Color;
				color *= Main.mouseTextColor / 255f;
				Color itemColor = held.RarityColor(Main.mouseTextColor / 255f);
				line = new TooltipLine(mod, "GlyphHint", "Right-click to wipe the [c/"+
					string.Format("{0:X2}{1:X2}{2:X2}:", color.R, color.G, color.B)+
					glyph.item.Name +"] of [i:"+ player.HeldItem.type +"] [c/"+
					string.Format("{0:X2}{1:X2}{2:X2}:", itemColor.R, itemColor.G, itemColor.B)+
					held.Name +"]");
			}
			else
				line = new TooltipLine(mod, "GlyphHint", "Hold the item which' glyph you want to remove and right-click this glyph");
			line.overrideColor = new Color(120, 190, 120);
			tooltips.Insert(index + 1, line);
		}
	}
}
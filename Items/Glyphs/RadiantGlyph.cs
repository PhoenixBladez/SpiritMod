using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class RadiantGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Radiant;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x28cacc };
		public override string Effect => "Radiance";
		public override string Addendum =>
			"Not attacking builds stacks of Divine Strike\n"+
			"The next hit is empowered by 11% per stack";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Glyph");
			Tooltip.SetDefault(
				"+4% crit chance\n"+
				"Not attacking builds stacks of Divine Strike\n"+
				"The next hit is empowered by 11% per stack");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 4;

			item.maxStack = 999;
		}


		public static void DivineStrike(Player player, ref int damage)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			modPlayer.divineCounter = 0;
			int index = player.FindBuffIndex(Buffs.Glyph.DivineStrike._type);
			if (index < 0)
				return;

			damage += (int)(.11f * modPlayer.divineStacks * damage);
			player.DelBuff(index);
			modPlayer.divineStacks = 1;
		}
	}
}
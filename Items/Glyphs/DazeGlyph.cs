using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Glyphs
{
	public class DazeGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Daze;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xce5aba };
		public override string Effect => "Dazed Dance";
		public override string Addendum =>
			"All attacks inflict confusion\n" +
			"Confused enemies take extra damage\n" +
			"Getting hurt may confuse you";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daze Glyph");
			Tooltip.SetDefault(
				"All attacks inflict confusion\n" +
				"Confused enemies take extra damage\n" +
				"Getting hurt may confuse the player");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;

			Item.maxStack = 999;
		}


		public static void Daze(NPC target, ref int damage)
		{
			if (target.FindBuffIndex(BuffID.Confused) > -1) {
				//Main.NewText("Daze");
				damage += 30;
			}

			if (Main.rand.Next(9) == 1)
				target.AddBuff(BuffID.Confused, 240);
		}

		public static void Daze(Player target, ref int damage)
		{
			if (target.FindBuffIndex(BuffID.Confused) > -1)
				damage += 30;

			if (Main.rand.Next(9) == 1)
				target.AddBuff(BuffID.Confused, 240, false);
		}
	}
}
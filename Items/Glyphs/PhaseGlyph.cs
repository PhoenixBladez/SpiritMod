using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Glyphs
{
	public class PhaseGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Phase;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xeb6bd8 };
		public override string ItemType => "item";
		public override string Effect => "Phase Flux";
		public override string Addendum =>
			"Weapon damage increases, the faster you move\n" +
			"Every 12 seconds you gain a stack of Temporal Shift\n" +
			"These stacks allow you to dash and gain a short burst of speed";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Glyph");
			Tooltip.SetDefault(
				"+7% Crit chance\n" +
				"Weapon damage increases, the faster you move\n" +
				"Every 12 seconds you gain a stack of Temporal Shift\n" +
				"These stacks allow you to dash and gain a short burst of speed");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightPurple;

			Item.maxStack = 999;
		}

		public override bool CanApply(Item item)
		{
			return item.IsWeapon() || item.useStyle > 0 && item.mountType < 0 && item.shoot <= ProjectileID.None;
		}


		public static void PhaseEffects(Player player, ref int damage, bool crit)
		{
			float scale = 1f;
			if (crit)
				scale += 0.07f;

			damage = (int)(damage * scale);
		}
	}
}
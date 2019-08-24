using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class BlazeGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Blaze;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x4d7fff };
		public override string Effect => "Flare Frenzy";
		public override string Addendum =>
			"Attacking enemies may grant Burning Rage\n"+
			"Burning Rage increases attack speed and damage but sets you ablaze";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaze Glyph");
			Tooltip.SetDefault(
				"+100% velocity and +3% damage\n"+
				"Attacking enemies may grant Burning Rage\n"+
				"Burning Rage increases attack speed and damage but sets you ablaze");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 5;

			item.maxStack = 999;
		}

		public static void Rage(Player player, NPC target)
		{
			if (target.CanLeech())
				Rage(player);
		}

		public static void Rage(Player player)
		{
			if (Main.rand.NextDouble() < .075)
				player.AddBuff(Buffs.Glyph.BurningRage._type, 300);
		}
	}
}
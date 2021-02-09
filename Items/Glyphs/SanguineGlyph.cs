using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class SanguineGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Sanguine;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x373eb9 };
		public override string Effect => "Sanguine Strike";
		public override string Addendum =>
			"+2 life regeneration per second\n" +
			"Attacks inflict Crimson Bleed\n" +
			"Attacking bleeding enemies leeches some life";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Glyph");
			Tooltip.SetDefault(
				"+2 life regeneration per second\n" +
				"Attacks inflict Crimson Bleed\n" +
				"Attacking bleeding enemies leeches some life");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;

			item.maxStack = 999;
		}


		public static void BloodCorruption(Player player, NPC target, int damage)
		{
			if (!target.CanLeech())
				return;

			if (target.FindBuffIndex(SpiritMod.instance.BuffType("SanguineBleed")) > -1
				&& Main.rand.NextDouble() < 0.3) {
				Leech(player);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(SpiritMod.instance.BuffType("SanguineBleed"), 600);
		}

		public static void BloodCorruption(Player player, Player target, int damage)
		{
			if (target.FindBuffIndex(SpiritMod.instance.BuffType("SanguineBleed")) > -1
				&& Main.rand.NextDouble() < 0.2) {
				Leech(player);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(SpiritMod.instance.BuffType("SanguineBleed"), 600, false);
		}

		private static void Leech(Player player)
		{
			player.AddBuff(SpiritMod.instance.BuffType("SanguineRegen"), Main.rand.Next(2, 4) * 60);
			DustHelper.DrawDiamond(player.Center, ModContent.DustType<Dusts.Blood>(), 3);
			Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 3, 0.75f, -0.5f);
		}
	}
}
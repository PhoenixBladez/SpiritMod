using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;

			Item.maxStack = 999;
		}


		public static void BloodCorruption(Player player, NPC target, int damage)
		{
			if (!target.CanLeech())
				return;

			if (target.FindBuffIndex(SpiritMod.Instance.Find<ModBuff>("SanguineBleed").Type) > -1
				&& Main.rand.NextBool(8)) {
				Leech(player);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(SpiritMod.Instance.Find<ModBuff>("SanguineBleed").Type, 600);
		}

		public static void BloodCorruption(Player player, Player target, int damage)
		{
			if (target.FindBuffIndex(SpiritMod.Instance.Find<ModBuff>("SanguineBleed").Type) > -1
				&& Main.rand.NextBool(8)) {
				Leech(player);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(SpiritMod.Instance.Find<ModBuff>("SanguineBleed").Type, 600, false);
		}

		private static void Leech(Player player)
		{
			player.AddBuff(SpiritMod.Instance.Find<ModBuff>("SanguineRegen").Type, Main.rand.Next(2, 4) * 60);
			DustHelper.DrawDiamond(player.Center, ModContent.DustType<Dusts.Blood>(), 3);
			SoundEngine.PlaySound(SoundID.Item3, player.Center);
		}
	}
}
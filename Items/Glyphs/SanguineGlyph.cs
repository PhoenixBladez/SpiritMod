using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class SanguineGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Sanguine;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x373eb9 };
		public override string Effect => "Sanguine Strike";
		public override string Addendum =>
			"+2 life regeneration per second\n"+
			"Attacks inflict Crimson Bleed\n"+
			"Attacking bleeding enemies leeches some life";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Glyph");
			Tooltip.SetDefault(
				"+2 life regeneration per second\n"+
				"Attacks inflict Crimson Bleed\n"+
				"Attacking bleeding enemies leeches some life");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;

			item.maxStack = 999;
		}


		public static void BloodCorruption(Player player, NPC target, int damage)
		{
			if (!target.CanLeech())
				return;

			if (target.FindBuffIndex(Buffs.Glyph.SanguineBleed._type) > -1
				&& Main.rand.NextDouble() < 0.2)
			{
				Leech(player, target, damage);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(Buffs.Glyph.SanguineBleed._type, 600);
		}

		public static void BloodCorruption(Player player, Player target, int damage)
		{
			if (target.FindBuffIndex(Buffs.Glyph.SanguineBleed._type) > -1
				&& Main.rand.NextDouble() < 0.2)
			{
				Leech(player, target, damage);
			}

			if (Main.rand.Next(5) == 0)
				target.AddBuff(Buffs.Glyph.SanguineBleed._type, 600, false);
		}

		private static void Leech(Player player, Entity target, int damage)
		{
			int leech = (int)(damage * 0.1);
			if (leech == 0)
				return;
			if (player.lifeSteal <= 0f)
				return;
			
			player.lifeSteal -= leech;
			Projectile.NewProjectile(target.position, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, leech);
		}
	}
}
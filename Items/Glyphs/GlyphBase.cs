using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public abstract class GlyphBase : ModItem
	{
		public const float GLOW_BIAS = 0.225f;

		private static GlyphBase[] _lookup;
		public static GlyphBase FromType(GlyphType type) => _lookup[(byte)type];


		public abstract GlyphType Glyph { get; }
		public abstract Texture2D Overlay { get; }
		public virtual Color Color => Color.White;
		public virtual string ItemType => "weapon";
		public abstract string Effect { get; }
		public abstract string Addendum { get; }

		public virtual bool CanApply(Item item)
		{
			return item.IsWeapon();
		}

		public sealed override bool CanRightClick()
		{
			Item item = Main.LocalPlayer.HeldItem;
			return !item.IsAir && item.GetGlobalItem<GItem>().Glyph != Glyph &&
				item.maxStack == 1 && CanApply(item);
		}

		public override void RightClick(Player player)
		{
			Item item = EnchantmentTarget(player);
			item.GetGlobalItem<GItem>().SetGlyph(item, Glyph);
			SoundEngine.PlaySound(SoundLoader.customSoundType, player.Center, Mod.GetSoundSlot(SoundType.Custom, "Sounds/GlyphAttach"));
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = tooltips.FindIndex(x => x.Name == "Tooltip0");
			if (index < 0)
				return;

			Player player = Main.player[Main.myPlayer];
			TooltipLine line;

			Color color = Color;
			color *= Main.mouseTextColor / 255f;
			line = new TooltipLine(Mod, "GlyphTooltip",
				"The enchanted " + ItemType + " will gain: [c/" +
				string.Format("{0:X2}{1:X2}{2:X2}:", color.R, color.G, color.B) + Effect + "]");
			line.OverrideColor = new Color(120, 190, 120);
			tooltips.Insert(index, line);

			if (Item.shopCustomPrice.HasValue) {
				line = new TooltipLine(Mod, "GlyphHint",
					"Can only be applied to " + ItemType + "s");
			}
			else if (CanRightClick()) {
				Item held = player.HeldItem;
				Color itemColor = held.RarityColor(Main.mouseTextColor / 255f);
				line = new TooltipLine(Mod, "GlyphHint", "Right-click to enchant [i:" + held.type + "] [c/" +
					string.Format("{0:X2}{1:X2}{2:X2}:", itemColor.R, itemColor.G, itemColor.B) +
					held.Name + "]");
			}
			else
				line = new TooltipLine(Mod, "GlyphHint", "Hold the " + ItemType
					+ " you want to enchant and right-click this glyph");
			line.OverrideColor = new Color(120, 190, 120);
			tooltips.Insert(index, line);
		}

		public Item EnchantmentTarget(Player player)
		{
			if (player.selectedItem == 58)
				return Main.mouseItem;
			else
				return player.HeldItem;
		}


		internal static void InitializeGlyphLookup()
		{
			_lookup = new GlyphBase[(byte)GlyphType.Count];

			Type glyphBase = typeof(GlyphBase);
			foreach (Type type in SpiritMod.Instance.Code.GetTypes()) {
				if (type.IsAbstract)
					continue;
				if (!type.IsSubclassOf(glyphBase))
					continue;

				Item item = new Item();
				item.SetDefaults(SpiritMod.Instance.Find<ModItem>(type.Name).Type, true);
				GlyphBase glyph = (GlyphBase)item.ModItem;
				_lookup[(byte)glyph.Glyph] = glyph;
			}

			GlyphBase zero = _lookup[0];
			for (int i = 1; i < _lookup.Length; i++) {
				if (_lookup[i] == null)
					_lookup[i] = zero;
			}
		}

		internal static void UninitGlyphLookup()
		{
			_lookup = null;
		}
	}
}
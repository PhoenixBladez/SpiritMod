using SpiritMod.Items;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.GlobalClasses.Players;

namespace SpiritMod.Utilities
{
	/// <summary>
	/// Class containing various extension methods.
	/// </summary>
	public static class ExtensionMethods
	{
		public static string IsNullOrWhitespaceFallback(this string str, string fallback) =>
			string.IsNullOrWhiteSpace(str)
				? fallback
				: str;

		public static string IsNullOrEmptyFallback(this string str, string fallback) =>
			string.IsNullOrEmpty(str)
				? fallback
				: str;

		public static bool ChestplateEquipped(this Player player, int type) => player.armor[1].type == type;
		public static bool ChestplateEquipped<T>(this Player player) where T : ModItem => player.ChestplateEquipped(ModContent.ItemType<T>());

		public static bool HasAccessory(this Player player, Item item) => item.modItem != null && item.modItem is AccessoryItem acc && player.GetModPlayer<MiscAccessoryPlayer>().accessory[acc.AccName];
		public static bool HasAccessory(this Player player, ModItem item) => item is AccessoryItem acc && player.GetModPlayer<MiscAccessoryPlayer>().accessory[acc.AccName];
		public static bool HasAccessory<TItem>(this Player player) where TItem : AccessoryItem => player.GetModPlayer<MiscAccessoryPlayer>().accessory[ModContent.GetInstance<TItem>().AccName];

		public static int ItemTimer(this Player player, ModItem item, int slot = -1)
		{
			if (item != null && item is ITimerItem tItem)
				return player.GetModPlayer<MiscAccessoryPlayer>().timers[tItem.GetType().Name + (slot == -1 ? "" : slot.ToString())];
			throw new Exception("Item timer not found or invalid.");
		}

		public static int ItemTimer(this Player player, Item item, int slot = -1) => ItemTimer(player, item.modItem, slot);
		public static int ItemTimer<T>(this Player player, int slot = -1) where T : ModItem, ITimerItem => player.GetModPlayer<MiscAccessoryPlayer>().timers[ModContent.GetInstance<T>().GetType().Name + (slot == -1 ? "" : slot.ToString())];

		public static int SetItemTimer<T>(this Player player, int value, int slot = -1) where T : ModItem, ITimerItem => player.GetModPlayer<MiscAccessoryPlayer>().timers[ModContent.GetInstance<T>().GetType().Name + (slot == -1 ? "" : slot.ToString())] = value;
	}
}

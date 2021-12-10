using Terraria;
using Terraria.ModLoader;

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

		public static bool AccessoryEquipped(this Player player, int type)
		{
			for (int k = 3; k <= 7 + player.extraAccessorySlots; k++)
				if (player.armor[k].type == type) return true;
			return false;
		}
		public static bool AccessoryEquipped(this Player player, Item item) => player.AccessoryEquipped(item.type);
		public static bool AccessoryEquipped<T>(this Player player) where T : ModItem => player.AccessoryEquipped(ModContent.ItemType<T>());
	}
}

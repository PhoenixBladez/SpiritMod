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
	}
}

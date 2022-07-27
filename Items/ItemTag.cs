using System;

namespace SpiritMod.Items
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ItemTagAttribute : Attribute
	{
		public ItemTags[] Tags = Array.Empty<ItemTags>();

		public ItemTagAttribute(params ItemTags[] tags)
		{
			if (tags.Length == 0)
				throw new ArgumentException("ItemTagAttribute contains no tags.");

			Tags = tags;
		}
	}

	public enum ItemTags
	{
		Explosive,
		Unloaded,
	}
}

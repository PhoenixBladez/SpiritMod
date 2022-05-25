using System;

namespace SpiritMod.Items
{
	public class ItemTagAttribute : Attribute
	{
		public ItemTags[] Tags = { };

		public ItemTagAttribute(params ItemTags[] tags)
		{
			Tags = tags;
		}
	}

	public enum ItemTags
	{
		Explosive,
	}
}

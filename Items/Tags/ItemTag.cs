using System;

namespace SpiritMod.Items.Tags
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

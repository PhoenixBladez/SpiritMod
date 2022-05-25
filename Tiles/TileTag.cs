using System;

namespace SpiritMod.Tiles
{
	public class TileTagAttribute : Attribute
	{
		public TileTags[] Tags = { };

		public TileTagAttribute(params TileTags[] tags)
		{
			Tags = tags;
		}
	}

	public enum TileTags
	{
		Indestructible,
	}
}
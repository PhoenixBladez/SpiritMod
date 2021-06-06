﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem
{
	public abstract class BasePortrait
	{
		/// <summary>The Texture associated with this BasePortrait.</summary>
		public readonly Texture2D Texture;
		/// <summary>The frame size of this BasePortrait. Adjust this to make it draw properly if you have a larger or smaller portrait than usual. Defaults to (108, 108).</summary>
		public readonly Point BaseSize;
		/// <summary>The NPCID associated with this BasePortrait.</summary>
		public abstract int ID { get; }

		/// <summary>Loads the texture of the portrait by default & sets the size to size. Defaults to (108, 108) by default.</summary>
		protected BasePortrait(Point? size = null)
		{
			Texture = ModContent.GetTexture($"SpiritMod/UI/Portraits/{GetType().Name}");
			BaseSize = size ?? new Point(108, 108);
		}

		public virtual Rectangle GetFrame(string speech) => new Rectangle(0, 0, 108, 108);
	}
}
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod
{
	public interface Glowing
	{
		Texture2D Glowmask(out float bias);
	}
}

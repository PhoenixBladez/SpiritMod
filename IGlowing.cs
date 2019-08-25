using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod
{
    public interface IGlowing
	{
		Texture2D Glowmask(out float bias);
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

namespace SpiritMod
{
	/// <summary>
	/// Creates a vertical item animation using a rectangular offset.
	/// Allows using animations with pixel buffers to avoid bleeding, etc.
	/// Just put the location and size of the first frame in as the offset.
	/// </summary>
	public class DrawAnimationVerticalRect : DrawAnimationVertical
	{
		public Rectangle offset;

		public DrawAnimationVerticalRect(int ticksperframe, int frameCount, Rectangle offset) : base(ticksperframe, frameCount)
		{
			this.offset = offset;
		}

		public override Rectangle GetFrame(Texture2D texture)
		{
			var frame = texture.Frame(1, FrameCount, 0, Frame);
			frame.X += offset.X;
			frame.Y += offset.Y;
			frame.Width = offset.Width;
			frame.Height = offset.Height;
			return frame;
		}
	}
}


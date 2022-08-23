using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Utilities.AnimationTester
{
	public class Animation
	{
		public AnimationActivation activation = AnimationActivation.None;
		public Texture2D Texture;
		public int frameSpeed = 0;
		public int timeLeft = 0;
		public int maxFrames = 0;

		public float FPS => 60f / frameSpeed;

		public Animation(Texture2D tex)
		{
			Texture = tex;
		}
	}
}

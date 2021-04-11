using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.Skies
{
	public class VaporwaveSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;
		private int AtlasIndex = -1;

		public override void Update(GameTime gameTime)
		{			
			if (isActive && intensity < 1f) {
				intensity += 0.02f;
			}
			else if (!isActive && intensity > 0f) {
				intensity -= 0.0035f;
			}
		}
		private float GetIntensity()
		{
			return 1f - Utils.SmoothStep(1000f, 6000f, 200f);
		}
		public override Color OnTileColor(Color inColor)
		{
			return new Color(Vector4.Lerp(new Vector4(0.68f, 0.29f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}



		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(29,0,45) * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			isActive = false;
		}

		public override void Reset()
		{
			isActive = false;
		}

		public override bool IsActive()
		{
			return isActive || intensity > 0f;
		}
	}
}
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
				intensity -= 0.008f;
			}
		}

        public override Color OnTileColor(Color inColor)
        {
            float amt = intensity * .02f;
            return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
        }



		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
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
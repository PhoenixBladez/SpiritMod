using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.Skies
{
	public class MeteoriteSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;

		public override void Update(GameTime gameTime)
		{			
			if (isActive && intensity < 1f) {
				intensity += 0.02f;
			}
			else if (!isActive && intensity > 0f) {
				intensity -= 0.02f;
			}
		}
		private float GetIntensity()
		{
			return 1f - Utils.SmoothStep(1000f, 6000f, 200f);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity * .4f);
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
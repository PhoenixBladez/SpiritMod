using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace SpiritMod
{
    public class BlueMoonSky : CustomSky
	{
        private float Intensity => 1f;

        public override Color OnTileColor(Color inColor)
		{
			float intensity = Intensity;
			return new Color(Vector4.Lerp(new Vector4(0f, 0.3f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				float intensity = Intensity;
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

        public override bool IsActive()
        {
            return MyWorld.BlueMoon;
        }

        public override void Update(GameTime gameTime)
        {
            // Required abstract member
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            // Required abstract member
        }

        public override void Deactivate(params object[] args)
		{
            // Required abstract member
        }

        public override void Reset()
		{
            // Required abstract member
        }
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using SpiritMod;

namespace SpiritMod
{
	public class BlueMoonSky : CustomSky
	{
		private float intensity = 0f;
		private int BlueMoonIndex = -1;

		public override void Update(GameTime gameTime)
		{

		}

		private float GetIntensity()
		{
			return 1f;
		}

		public override Color OnTileColor(Color inColor)
		{
			float intensity = this.GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(0f, 0.3f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		private bool UpdateBlueMoonIndex()
		{
			if (MyWorld.BlueMoon)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				float intensity = this.GetIntensity();
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			//isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			//	isActive = false;
		}

		public override void Reset()
		{
			//	isActive = false;
		}

		public override bool IsActive()
		{
			return MyWorld.BlueMoon;
		}
	}
}
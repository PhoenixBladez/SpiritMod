using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace SpiritMod.Skies
{
	public class BlueAlgaeSky : CustomSky
	{
		private UnifiedRandom _random = new UnifiedRandom();

		private Texture2D _bgTexture;


		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			this._bgTexture = TextureManager.Load("Images/Misc/StardustSky/Background");
		}

		public override void Update(GameTime gameTime)
		{
			if (this._isActive) {
				this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
			}
			else {
				this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				spriteBatch.Draw(this._bgTexture, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 900.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), new Color(0, 225, 255, 240) * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * this._fadeOpacity));
				Vector2 value = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
				Vector2 value2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
			}

		}

		public override float GetCloudAlpha()
		{
			return (1f - this._fadeOpacity) * 0.3f + 0.7f;
		}
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
		}
		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		public override void Reset()
		{
			this._isActive = false;
		}

		public override bool IsActive()
		{
			if (!this._isActive) {
				return this._fadeOpacity > 0.001f;
			}
			return true;
		}
	}
}

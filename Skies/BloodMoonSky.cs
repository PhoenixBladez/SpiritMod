using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SpiritMod.Skies
{
	public class BloodMoonSky : CustomSky
	{
		private Texture2D _bgTexture;

		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad() => _bgTexture = ModContent.Request<Texture2D>("Terraria/Images/Misc/NebulaSky/Background", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		public override void Update(GameTime gameTime)
		{
			if (_isActive) 
				_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);

			else 
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) 
				spriteBatch.Draw(_bgTexture, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16 - Main.screenPosition.Y - 900) * 0.1f) - (Main.screenHeight / 3)), Main.screenWidth, Main.screenHeight), new Color(255, 0, 120) * 0.5f * Math.Min(1f, (Main.screenPosition.Y - 800) / 1000 * _fadeOpacity));
		}

		public override float GetCloudAlpha() => (1f - _fadeOpacity) * 0.3f + 0.7f;
		public override void Activate(Vector2 position, params object[] args) => _isActive = true;
		public override void Deactivate(params object[] args) => _isActive = false;

		public override void Reset() => _isActive = false;

		public override bool IsActive()
		{
			if (!_isActive) 
				return _fadeOpacity > 0.001f;

			return true;
		}
	}
}

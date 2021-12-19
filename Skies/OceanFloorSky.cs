using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.StarjinxEvent;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace SpiritMod.Skies
{
	public class OceanFloorSky : CustomSky
	{
		private UnifiedRandom _random = new UnifiedRandom();

		private bool _isActive;

		private float _fadeOpacity;

		public override float GetCloudAlpha()
		{
			return 1f - _fadeOpacity;
		}
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (_isActive && _fadeOpacity < 1f)
			{
				_fadeOpacity += 0.05f;
			}
			else if (!_isActive && _fadeOpacity > 0f)
			{
				_fadeOpacity -= 0.005f;
			}
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

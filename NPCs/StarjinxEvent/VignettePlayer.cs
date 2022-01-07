using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used to handle if a vignette filter is active for the player, and information related to the application of the shader.</summary>
	class VignettePlayer : ModPlayer
	{
		private bool _lastTickVignetteActive;
		private bool _vignetteActive;
		private Vector2 _targetPosition;
		private float _opacity;
		private float _radius;
		private float _fadeDistance;
		private Color _color;

		public override void ResetEffects()
		{
			_lastTickVignetteActive = _vignetteActive;
			_vignetteActive = false;
		}

		public void SetVignette(float radius, float colorFadeDistance, float opacity) => SetVignette(radius, colorFadeDistance, opacity, Color.Black, Main.screenPosition);

		/// <summary>
		/// Sets a vignette effect for the player with the given radius, distance to fade from the default color to the used color, opacity of the effect, color, and center position.
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="colorFadeDistance"></param>
		/// <param name="opacity"></param>
		/// <param name="color"></param>
		/// <param name="targetPosition"></param>
		public void SetVignette(float radius, float colorFadeDistance, float opacity, Color color, Vector2 targetPosition)
		{
			_radius = radius;
			_targetPosition = targetPosition;
			_fadeDistance = colorFadeDistance;
			_color = color;
			_opacity = opacity;
			_vignetteActive = true;
		}

		public override void UpdateBiomeVisuals()
		{
			SpiritMod.vignetteShader.UseColor(_color);
			SpiritMod.vignetteShader.UseIntensity(_opacity);
			SpiritMod.vignetteEffect.Parameters["Radius"].SetValue(_radius);
			SpiritMod.vignetteEffect.Parameters["FadeDistance"].SetValue(_fadeDistance);
			player.ManageSpecialBiomeVisuals("SpiritMod:Vignette", _vignetteActive || _lastTickVignetteActive, _targetPosition);
		}
	}
}

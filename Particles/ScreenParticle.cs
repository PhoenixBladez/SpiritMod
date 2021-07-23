using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	/// <summary>
	/// Represents a particle that can draw with a parallax effect, screen wrapping, adjusting for game-zoom and automatic fade-in and fade-outs.
	/// </summary>
	public abstract class ScreenParticle : Particle
	{
		public Vector2 OriginalScreenPosition;
		public float ParallaxStrength;
		protected float activeOpacity;

		/// <summary>
		/// The condition determining if the particle can spawn naturally, used in conjunction with ScreenParticle.ScreenSpawnChance.
		/// If true, the particle will fade in until it is at full opacity. Otherwise, it'll fade out until it is fully transparent.
		/// This will not automatically kill the particle, use Particle.Kill() for that.
		/// </summary>
		public virtual bool ActiveCondition => true;

		/// <summary>
		/// Override and use this instead of Particle.SpawnChance. The latter is reserved by ScreenParticle.
		/// If ScreenParticle.ActiveCondition is false, then no particles will naturally spawn, regardless of this property's value.
		/// </summary>
		public virtual float ScreenSpawnChance => 0f;

		/// <summary>
		/// Override and use this instead of Particle.UseCustomDraw. The latter is reserved by ScreenParticle.
		/// </summary>
		public virtual bool UseCustomScreenDraw => false;

		public sealed override bool UseCustomDraw => true;

		public sealed override float SpawnChance {
			get {
				if (!ActiveCondition)
					return 0f;

				return ScreenSpawnChance;
			}
		}

		/// <summary>
		/// Used by the default spritebatch drawing for ScreenParticles, use if needed for custom drawing.
		/// </summary>
		/// <returns>The position on the screen to draw the particle, with a parallax effect, screen wrapping, and adjusting for game zoom</returns>
		public Vector2 GetDrawPosition()
		{
			//modify the drawing position based on the difference between the current screen position, the original screen position when spawned, and the strength of the parallax effect
			Vector2 drawPosition = Position - Vector2.Lerp(Main.screenPosition, Main.screenPosition - 2 * (OriginalScreenPosition - Main.screenPosition), ParallaxStrength);

			//modify the position to keep particles onscreen at all times, as to not easily all be cleared due to parallax effect
			Vector2 ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
			Vector2 UiScreenSize = ScreenSize * Main.UIScale;

			while (drawPosition.X < 0)
				drawPosition.X += UiScreenSize.X;

			while (drawPosition.Y < 0)
				drawPosition.Y += UiScreenSize.Y;

			drawPosition = new Vector2(drawPosition.X % UiScreenSize.X, drawPosition.Y % UiScreenSize.Y) * Main.GameViewMatrix.Zoom;

			return drawPosition - 3 * ((ScreenSize * Main.GameViewMatrix.Zoom) - ScreenSize) / 4;
		}

		public sealed override void Update()
		{
			UpdateOnScreen();

			if (ActiveCondition)
				activeOpacity = Math.Min(activeOpacity + 0.075f, 1);
			else
				activeOpacity = Math.Max(activeOpacity - 0.075f, 0);
		}

		public sealed override void CustomDraw(SpriteBatch spriteBatch)
		{
			if (UseCustomScreenDraw)
				CustomScreenDraw(spriteBatch);
			else
				spriteBatch.Draw(Texture, GetDrawPosition(), null, Color * activeOpacity, Rotation, Origin, Scale * Main.GameViewMatrix.Zoom, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Override and use this instead of Particle.Update. The latter is reserved by ScreenParticle.
		/// </summary>
		public virtual void UpdateOnScreen() { }

		/// <summary>
		/// Override and use this instead of Particle.CustomDraw. The latter is reserved by ScreenParticle.
		/// </summary>
		public virtual void CustomScreenDraw(SpriteBatch spriteBatch) { }
	}
}

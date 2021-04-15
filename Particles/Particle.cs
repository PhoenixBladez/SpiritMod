using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SpiritMod.Particles
{
	/// <summary>
	/// Represents a particle with a position, velocity, rotation, scale and transparency.
	/// Particles are in used in conjunction with ParticleManager. Each type of Particle has its own ParticleManager.
	/// </summary>
	public class Particle
	{
		public int ID; // you don't have to use this
		public int Type; // you don't have to use this
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 origScreenpos;
		public Vector2 Origin;
		public Color Color;
		public float Rotation;
		public float Scale;
		public uint TimeActive;

		/// <summary>
		/// Used by the default spritebatch drawing for particles, use if needed for custom drawing
		/// </summary>
		/// <returns>The position on the screen to draw the particle, with a parallax effect, screen wrapping, and adjusting for game zoom</returns>
		public Vector2 DrawPosition() {
			//modify the drawing position based on the difference between the current screen position, the original screen position when spawned, and the scale of the particle
			Vector2 drawPosition = Position - Vector2.Lerp(Main.screenPosition, Main.screenPosition - 2 * (origScreenpos - Main.screenPosition), Scale);
			//modify the position to keep particles onscreen at all times, as to not easily all be cleared due to parallax effect
			Vector2 ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
			Vector2 UiScreenSize = ScreenSize * Main.UIScale;
			while (drawPosition.X < 0)
				drawPosition.X += UiScreenSize.X;
			while (drawPosition.Y < 0)
				drawPosition.Y += UiScreenSize.Y;
			drawPosition = new Vector2(drawPosition.X % UiScreenSize.X, drawPosition.Y % UiScreenSize.Y) * Main.GameViewMatrix.Zoom;
			return drawPosition - 3 * ((ScreenSize * Main.GameViewMatrix.Zoom) - ScreenSize)/4;
		}

		public Texture2D Texture => ParticleHandler.GetTexture(Type);

		/// <summary>
		/// Set this to true to disable default particle drawing, thus calling Particle.CustomDraw() instead.
		/// </summary>
		public virtual bool UseCustomDraw => false;

		/// <summary>
		/// Set this to true to make your particle use additive blending instead of alphablend.
		/// </summary>
		public virtual bool UseAdditiveBlend => false;

		/// <summary>
		/// Call this when you want to clear your particle and remove it from the world.
		/// </summary>
		public void Kill() => ParticleHandler.DeleteParticleAtIndex(ID);

		/// <summary>
		/// Called every tick. Update your particle in this method.
		/// Particle velocity is automatically added to the particle position for you, and TimeAlive is incremented.
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Allows you to do custom drawing for your particle. Only called if Particle.UseCustomDrawing is true.
		/// </summary>
		public virtual void CustomDraw(SpriteBatch spriteBatch) { }

		/// <summary>
		/// The condition determining when the particle fades in or out, and when the particle is able to spawn.
		/// Used in conjunction with spawnchance to determine when a particle is spawned.
		/// </summary>
		public virtual bool ActiveCondition() => false;

		/// <summary>
		/// The opacity of the particle, adjusts to be more transparent or less transparent depending on the active condition
		/// 0 means the particle is invisible, 1 means it is fully visible
		/// </summary>
		public float ActiveOpacity = 0;

		/// <summary>
		/// The chance at any given tick that this particle will spawn.
		/// Return 0f if you want the particle to not naturally spawn (if you want to spawn it yourself).
		/// </summary>
		/// <returns>The chance for the particle to spawn at any given tick.</returns>
		public virtual float SpawnChance() => 0f;

		/// <summary>
		/// Called if the particle tries to naturally spawn. This can only be called if Particle.SpawnChance() returns a value greater than 0f.
		/// Use this to spawn your particle randomly.
		/// </summary>
		public virtual void OnSpawnAttempt() { }
	}
}

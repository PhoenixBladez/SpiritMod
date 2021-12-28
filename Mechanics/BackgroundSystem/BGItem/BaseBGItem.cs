using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace SpiritMod.Mechanics.BackgroundSystem.BGItem
{
	public class BaseBGItem
	{
		/// <summary>Texture used when drawing this background item.</summary>
		internal Texture2D tex;
		/// <summary>Position in world coordinates.</summary>
		internal Vector2 position = new Vector2();
		/// <summary>Velocity. Added to position per frame.</summary>
		internal Vector2 velocity = new Vector2();
		/// <summary>Rotation in radians.</summary>
		internal float rotation = 0f;
		internal float scale = 1f;
		/// <summary>If true, this background item will be removed next frame.</summary>
		internal bool killMe = false;
		/// <summary>Adjusts how quickly or slowly it moves according to parallax, and is used for layering.</summary>
		internal float parallax = 1f;
		/// <summary>Multiplies parallax scroll speed by a value.</summary>
		internal float parallaxScale = 1f;
		/// <summary>The colour to draw this like.</summary>
		internal Color drawColor = Color.White;
		/// <summary>SourceRectangle for different frames.</summary>
		internal Rectangle source = new Rectangle(0, 0, 0, 0);

		/// <summary>If true, this background item will be saved and loaded, as per <see cref="Save()"/> and <see cref="Load(TagCompound)"/>.</summary>
		public bool SaveMe { get; protected set; }
		/// <summary>Used for draw position, so that stuff that is offscreen does not need to be drawn. Might not work, needs tweaking.</summary>
		public Vector2 DrawPosition { get; protected set; }

		/// <summary>Center of the background item.</summary>
		internal Vector2 Center
		{
			get => position + (source.Size() / 2);
			set => position = value + (source.Size() / 2);
		}

		/// <summary>Default with only a save/don't save value.</summary>
		/// <param name="save">If this BGItem saves or not.</param>
		public BaseBGItem(bool save = false)
		{
			SaveMe = save;
		}

		/// <summary>Creates a BGItem with a position, scale and frame size, along with the save value.</summary>
		/// <param name="initPos">Position this BGItem is spawned at.</param>
		/// <param name="sc">Scale this BGItem is spawned with.</param>
		/// <param name="size">Frame size.</param>
		/// <param name="save">If this BGItem saves or not.</param>
		public BaseBGItem(Vector2 initPos, float sc, Point size, bool save = false)
		{
			position = initPos;
			scale = sc;

			source = new Rectangle(0, 0, size.X, size.Y);

			DrawPosition = position;

			SaveMe = save;
		}

		/// <summary>Creates a BGItem with a texture, position and scale, along with the save value.</summary>
		/// <param name="t">Texture this BGItem uses.</param>
		/// <param name="initPos">Position this BGItem is spawned at.</param>
		/// <param name="sc">Scale this BGItem is spawned with.</param>
		/// <param name="save">If this BGItem saves or not.</param>
		public BaseBGItem(Texture2D t, Vector2 initPos, float sc, bool save = false)
		{
			tex = t;
			position = initPos;
			scale = sc;

			source = new Rectangle(0, 0, t.Width, t.Height);
			DrawPosition = position;

			SaveMe = save;
		}

		/// <summary>Equivalent to Update(). Adds velocity to position by default.</summary>
		internal virtual void Behaviour() => position += velocity;

		/// <summary>Draws the BGItem.</summary>
		/// <param name="off">Offset for drawing.</param>
		internal virtual void Draw(Vector2 off)
		{
			DrawPosition = position + off;
			Main.spriteBatch.Draw(tex, DrawPosition - Main.screenPosition + off, source, drawColor, rotation, tex.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0f);
		}

		/// <summary>Weird hacky thing I did for parallax. Offsets position to look parallaxed.</summary>
		/// <returns>Parallax value.</returns>
		internal Vector2 GetParallax()
		{
			Vector2 pC = Main.screenPosition;
			Vector2 offset = (pC - position) * parallax;
			return offset;
		}

		/// <summary>Saves the current BGItem.</summary>
		/// <returns>Info to save.</returns>
		public virtual TagCompound Save() => null;

		/// <summary>Loads info given a tag.</summary>
		/// <param name="tag">Info to use to load.</param>
		public virtual void Load(TagCompound tag) { }

		/// <summary>Simple parallax.</summary>
		/// <param name="mul">Offsets parallax by a multiplicative.</param>
		internal void BaseParallax(float mul)
		{
			parallax = (0.8f - scale) * parallaxScale * mul;
			if (parallax > 1) parallax = 1f;
		}
		internal void BaseParallax() => BaseParallax(1f);
	}
}
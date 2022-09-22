using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.Linq;

namespace SpiritMod.Items.Weapon.Magic.Rhythm
{
	public class RhythmNote
	{
		public const float AbsoluteTimeLeniency = 0.15f; // 100ms
		public const float VerticalTrack = 60;

		public static Texture2D Note { get; set; }
		public static Texture2D SideNote { get; set; }

		public RhythmMinigame Owner { get; set; }
		public float TimeLeft { get; protected set; }
		public bool Active { get; protected set; }

		private bool closeToEnd = false;
		private InterpolatedFloat vertOffset;
		private InterpolatedFloat disappearTimer;

		public RhythmNote(RhythmMinigame owner)
		{
			Owner = owner;

			TimeLeft = 1f;
			Active = true;
			vertOffset = new InterpolatedFloat(0f, 0.2f, InterpolatedFloat.EaseInOutBack);
			vertOffset.Set(20f);

			disappearTimer = new InterpolatedFloat(0f, 0.3f, InterpolatedFloat.EaseInOut);
		}

		public virtual void CheckHit(bool trackButtonDown, bool previousTrackButtonDown)
		{
			if (trackButtonDown && !previousTrackButtonDown)
			{
				if (CanBeHit())
				{
					Owner.Beat();
					Destroy(true);
				}
			}
		}

		public virtual bool CanBeHit()
		{
			return Active && Math.Abs(TimeLeft) < AbsoluteTimeLeniency;
		}

		public virtual void Destroy(bool good)
		{
			if (!good)
			{
				Owner.Notes.Remove(this);
			}
			Active = false;
			disappearTimer.Set(1f);
		}

		public void Update(float deltaTime, bool trackButtonDown, bool previousTrackButtonDown)
		{
			if (disappearTimer == 1f)
			{
				Owner.Notes.Remove(this);
			}
			else if (Active)
			{
				var mostLikelyBeat = Owner.Notes.Where(n => n.Active).OrderBy(n => Math.Abs(TimeLeft)).FirstOrDefault();

				if (mostLikelyBeat == this)
					CheckHit(trackButtonDown, previousTrackButtonDown);

				if (TimeLeft < -AbsoluteTimeLeniency)
				{
					Owner.Break();
					Destroy(false);
				}

				if (TimeLeft < 0.2f && !closeToEnd)
				{
					closeToEnd = true;
					vertOffset.Set(0f);
				}

				TimeLeft -= deltaTime;
			}

			vertOffset.Process(deltaTime);
			disappearTimer.Process(deltaTime);
		}

		public virtual void Draw(SpriteBatch sB, Vector2 midPerfectPoint, float visibility)
		{
			Vector2 pos = midPerfectPoint + new Vector2(0, -VerticalTrack * TimeLeft);
			if (Active)
			{
				Color color = new Color(255, 255, 255, (byte)(visibility * 255)); // Track == Track.LeftTrack ? new Color(255, 129, 251) : new Color(58, 126, 255);

				sB.Draw(Note, pos, null, color, 0f, new Vector2(6f, 2f), 2f, SpriteEffects.None, 0);

				sB.Draw(SideNote, pos + new Vector2(vertOffset, 0), null, color, 0f, new Vector2(2f, 1f), 2f, SpriteEffects.FlipHorizontally, 0);
				sB.Draw(SideNote, pos + new Vector2(-vertOffset, 0), null, color, 0f, new Vector2(2f, 1f), 2f, SpriteEffects.None, 0);
			}
			else
			{
				// IDK
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.Rhythm
{
	public delegate void BeatEventHandler(bool successful, int combo);

	public class RhythmMinigame
	{
		public Vector2 Position { get; set; }
		public InterpolatedFloat Visibility { get; set; }
		public int BPM { get; set; }
		public int Combo { get; protected set; }
		public InterpolatedFloat ComboScale { get; protected set; }
		public InterpolatedFloat BeatScale { get; protected set; }
		public List<RhythmNote> Notes { get; set; }
		protected SoundEffectInstance Music { get; set; }
		protected InterpolatedFloat MusicVolume { get; set; }
		protected bool Paused { get; set; }
		protected Player Owner { get; set; }
		protected IRhythmWeapon Weapon { get; set; }

		protected static Color Left = new Color(184, 136, 255);
		protected static Color LeftOutline = new Color(227, 180, 255);
		protected static Color Right = new Color(83, 191, 255);
		protected static Color RightOutline = new Color(187, 238, 255);

		protected static SpriteFont GameFont { get; set; }
		protected static Texture2D RhythmComboCounter { get; set; }
		protected static Texture2D RhythmBeatCircle { get; set; }
		protected static Texture2D RhythmTrack { get; set; }
		protected static Texture2D RhythmTrackOutline { get; set; }
		protected static Texture2D Line { get; set; }
		
		bool lastM1;
		bool lastM2;
		bool lastPaused;

		float maxTime;
		float timer = 0f;

		public RhythmMinigame(Vector2 position, Player owner, IRhythmWeapon item, int bPM, SoundEffect music)
		{
			Position = position;
			BPM = bPM;

			maxTime = 60f / BPM;

			Notes = new List<RhythmNote>();
			ComboScale = new InterpolatedFloat(0f, 0.1f, InterpolatedFloat.EaseInOut);
			BeatScale = new InterpolatedFloat(0, 0.25f);

			Music = music.CreateInstance();

			Music.Volume = 0;
			Music.IsLooped = true;
			Music.Play();

			MusicVolume = new InterpolatedFloat(0f, 0.25f);
			MusicVolume.Set(0.05f);

			Visibility = new InterpolatedFloat(0f, 0.5f, InterpolatedFloat.EaseInOut);
			Visibility.Set(1f);

			Main.OnTick += MinigameDisposal;

			Owner = owner;
			Weapon = item;
		}

		private void MinigameDisposal() // fucking items dont have a "this item is no longer updating" hook so i gotta do it the Dirty Way
		{
			if (Owner.dead)
				Pause();
			else if (!Owner.inventory.Any(i => { if (i.ModItem is IRhythmWeapon rw) return rw == Weapon; return false; }) || Main.gameInactive || Main.gameMenu || Main.gamePaused)
			{
				if (Weapon is IRhythmWeapon wep)
				{
					Music.Dispose();
					wep.Minigame = null;

					Main.OnTick -= MinigameDisposal;
				}
			}

		}

		public static void LoadStatic()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				RhythmComboCounter = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/RhythmMid");
				RhythmTrack = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/RhythmTrack");
				RhythmTrackOutline = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/RhythmTrackOutline");
				RhythmBeatCircle = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/BeatCircle");

				RhythmNote.Note = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/Note");
				RhythmNote.SideNote = ModContent.Request<Texture2D>("Items/Weapon/Magic/Rhythm/SideNote");
			}
		}

		public void Beat()
		{
			Combo++;
			ComboScale.Set(Combo);

			BeatScale = new InterpolatedFloat(1f, 0.25f);
			BeatScale.Set(0f);
			Weapon.OnBeat(true, Combo);
			MusicVolume.Set(0.1f + Utils.Clamp(Combo, 0, 10) * 0.09f);
		}

		public void Break()
		{
			Weapon.OnBeat(false, Combo);

			Combo = 0;
			ComboScale.Set(Combo);
			MusicVolume.Set(0.1f);
		}

		public virtual void Update(float deltaTime)
		{
			bool m1 = Mouse.GetState().LeftButton == ButtonState.Pressed;
			bool m2 = Mouse.GetState().RightButton == ButtonState.Pressed;

			if (!Paused)
			{
				if (((m1 && !lastM1) || (m2 && !lastM2)) && !Notes.Any(n => n.CanBeHit()))
				{
					Break();
				}

				foreach (var note in Notes.ToList())
				{
					note.Update(deltaTime, m1, lastM1);
				}



				timer += deltaTime;

				if (timer >= maxTime)
				{
					timer -= maxTime;

					Notes.Add(new RhythmNote(this));
				}
			}

			ComboScale.Process(deltaTime);
			BeatScale.Process(deltaTime);
			MusicVolume.Process(deltaTime);
			Visibility.Process(deltaTime);

			Music.Volume = MusicVolume * Main.soundVolume;

			lastM1 = m1;
			lastM2 = m2;
			lastPaused = Paused;
		}

		public void Pause()
		{
			if (!Paused)
			{
				MusicVolume.Set(0f);
				Music.Pause();
				Paused = true;
				Visibility.Set(0f);
			}
		}

		public void Unpause()
		{
			if (Paused)
			{
				MusicVolume = new InterpolatedFloat(0f, 0.25f);
				MusicVolume.Set(0.1f);

				Music.Play();

				Paused = false;
				Visibility.Set(1f);
			}
		}

		public virtual void Draw(SpriteBatch sB)
		{
			if (Visibility > 0f)
			{
				sB.End();

				sB.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

				foreach (var note in Notes)
				{
					note.Draw(sB, Position, Visibility);
				}

				Color visibilityAdjustedWhite = new Color(255, 255, 255, (byte)(Visibility * 255));

				sB.Draw(RhythmComboCounter, Position + new Vector2(0, 36), null, visibilityAdjustedWhite, (float)Math.Sin(BeatScale * 8) * (0.1f + 0.2f * BeatScale), new Vector2(12, 8), 2f + BeatScale * 0.2f, SpriteEffects.None, 0f);

				float trackRot = (float)Math.Sin(-BeatScale * 8) * (0.1f * BeatScale);

				sB.Draw(RhythmTrack, Position, null, visibilityAdjustedWhite, 0f, new Vector2(10, 35), 2f + trackRot, SpriteEffects.None, 0f);
				sB.Draw(RhythmTrackOutline, Position, null, new Color(Color.White.R, Color.White.G, Color.White.B, (byte)(BeatScale * Visibility * 255)), 0f, new Vector2(10, 35), 2f + trackRot, SpriteEffects.None, 0f);

				int offset = 0;

				if (BeatScale > 0.7f) offset = 40;
				else if (BeatScale > 0.3f) offset = 20;

				Rectangle circleRect = new Rectangle(offset, 0, 20, 43);

				sB.Draw(RhythmBeatCircle, Position, circleRect, visibilityAdjustedWhite, 0f, new Vector2(10, 35), 2f, SpriteEffects.None, 0f);

				string comboText = $"x{Combo}";
				Vector2 size = FontAssets.MouseText.Value.MeasureString(comboText);

				Utils.DrawBorderString(sB, comboText, Position + new Vector2(size.X / -2, 24), visibilityAdjustedWhite);

				sB.End();

				sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

				//sB.DrawString(Main.fontMouseText, comboText, Position + new Vector2(size.X / -2, -12), Color.White);
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.AnimationTester
{
	internal class AnimationCommand : ModCommand
	{
		public override string Command => "anm";
		public override CommandType Type => CommandType.Chat;

		private ref Animation animation => ref Main.LocalPlayer.GetModPlayer<AnimationTesterPlayer>().animation;
		private bool AnimationLoaded => Main.LocalPlayer.GetModPlayer<AnimationTesterPlayer>().AnimationLoaded;

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			HandleInput(args);
		}

		private void HandleInput(string[] args)
		{
			bool ComplainAboutTooFewArgs(int necessaryArgs)
			{
				if (args.Length < necessaryArgs)
				{
					Main.NewText($"[ANIMATIONTESTER] Not enough arguments! {necessaryArgs} arguments are needed.", Color.DarkRed);
					return true;
				}
				return false;
			}

			if (args[0].ToUpper() == "HELP")
			{
				if (ComplainAboutTooFewArgs(1))
					return;
				Main.NewText($"[ANIMATIONTESTER] HELP CONTENT - ALL commands prefacebd by /{Command}\nfps {{x}} - Sets FPS of the given animation\n" +
					"load {path} - Loads an image from disk and sets the animation. This is the necessary first step." +
					"\nsource {source} - Controls what spawns the animation test. Valid types are None, OnHitNPC and OnItemUse." +
					"\ntime {x} - How long the animation lasts in-world.\nmaxframes {x}" +
					"\nall {maxFrames} {fps} {time} {source}");
				return;
			}

			if (ComplainAboutTooFewArgs(2))
				return;

			if (args[0].ToUpper() == "LOAD")
			{
				string s = "";
				for (int i = 1; i < args.Length; ++i)
					s += args[i] + " ";

				string path = s;

				//if (!File.Exists(@path))
				//	Main.NewText("[ANIMATIONTESTER] Invalid texture path.", Color.DarkRed);
				//else
				{
					using FileStream reader = new FileStream(path, FileMode.Open);
					Texture2D tex = Texture2D.FromStream(Main.instance.GraphicsDevice, reader);

					animation = new Animation(tex);
					Main.NewText($"[ANIMATIONTESTER] Animation created and set.", Color.Green);
				}
			}
			else if (args[0].ToUpper() == "FPS")
				SetFPS(args[1]);
			else if (args[0].ToUpper() == "TIME")
				SetTime(args[1]);
			else if (args[0].ToUpper() == "SOURCE")
				SetSource(args[1]);
			else if (args[0].ToUpper() == "MAXFRAMES")
				SetMaxFrames(args[1]);
			else if (args[0].ToUpper() == "ALL")
			{
				if (ComplainAboutTooFewArgs(5))
					return;

				SetMaxFrames(args[1]);
				SetFPS(args[2]);
				SetTime(args[3]);
				SetSource(args[4]);
			}
		}

		private void SetSource(string source)
		{
			for (int i = 0; i < (int)AnimationActivation.Count; ++i)
			{
				if (source.ToUpper() == ((AnimationActivation)i).ToString().ToUpper() && i != (int)AnimationActivation.Count)
				{
					animation.activation = (AnimationActivation)i;
					Main.NewText($"[ANIMATIONTESTER] Set source to {animation.activation}.", Color.Green);
					return;
				}
			}
		}

		private void SetTime(string time)
		{
			if (!int.TryParse(time, out int value))
				Main.NewText("[ANIMATIONTESTER] Invalid timeleft.", Color.DarkRed);
			else if (!AnimationLoaded)
				Main.NewText("[ANIMATIONTESTER] Attempted to set timeleft before setting animation texture.", Color.DarkRed);
			else
			{
				animation.timeLeft = value;
				Main.NewText($"[ANIMATIONTESTER] Set timeleft to {value} frames.", Color.Green);
			}
		}

		private void SetFPS(string speed)
		{
			if (!int.TryParse(speed, out int value))
				Main.NewText("[ANIMATIONTESTER] Invalid frame speed.", Color.DarkRed);
			else if (!AnimationLoaded)
				Main.NewText("[ANIMATIONTESTER] Attempted to set frame speed before setting animation texture.", Color.DarkRed);
			else
			{
				animation.frameSpeed = value;
				Main.NewText($"[ANIMATIONTESTER] Set frame speed to {value} frames/second.", Color.Green);
			}
		}

		private void SetMaxFrames(string speed)
		{
			if (!int.TryParse(speed, out int value))
				Main.NewText("[ANIMATIONTESTER] Invalid max frame count.", Color.DarkRed);
			else if (!AnimationLoaded)
				Main.NewText("[ANIMATIONTESTER] Attempted to set max frames before setting animation texture.", Color.DarkRed);
			else
			{
				animation.maxFrames = value;
				Main.NewText($"[ANIMATIONTESTER] Set max frme count to {value}.", Color.Green);
			}
		}
	}
}

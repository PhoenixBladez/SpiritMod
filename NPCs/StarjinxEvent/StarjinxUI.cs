using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>
	/// Static class managing the drawing of starjinx event UI, and the constant updating of variables used for its drawing
	/// </summary>
	public class StarjinxUI
    {
		private static float BarProgress = 0; //How much of the bar is filled, based on enemies killed
		private static float Scale = 0; //The global scale of every element in the UI
		private static float Alpha = 0; //The global transparency of every element in the UI
		private static float FadeTime = 0; //The amount of ticks that the UI has been fading in/out for

		//Maximum desired values for scale and alpha
		private const float MAX_SCALE = 0.8f;
		private const float MAX_ALPHA = 0.5f;

		//Time, in ticks, for the ui to fully fade in or out
		private const int FADE_MAX_TIME = 60;

		private const float InternalOffset = 3; //Vertical distance between each background
		private const float OffsetX = 20; //Horizontal offset
		private const float OffsetY = 20; //Vertical offset

		private static Texture2D EventIcon => SpiritMod.Instance.GetTexture("Textures/InvasionIcons/Starjinx_Icon");

		//Array of textures used to draw the comets
		private static readonly Texture2D[] Comets = new Texture2D[]
		{
			SpiritMod.Instance.GetTexture("Textures/InvasionIcons/Starjinx_CometS"),
			SpiritMod.Instance.GetTexture("Textures/InvasionIcons/Starjinx_CometM"),
			SpiritMod.Instance.GetTexture("Textures/InvasionIcons/Starjinx_CometL")
		};

		private static Vector2 Size => new Vector2(220f, 60f);

		private static readonly StarjinxUIComet[] UiComets = new StarjinxUIComet[]
		{
			new StarjinxUIComet(0, Comets[0]),
			new StarjinxUIComet(1, Comets[0]),
			new StarjinxUIComet(2, Comets[0]),
			new StarjinxUIComet(3, Comets[1]),
			new StarjinxUIComet(4, Comets[1]),
			new StarjinxUIComet(5, Comets[2])
		};

		/// <summary>
		/// Called during the world's initialization to reset certain values, that could otherwise linger unwantedly upon exiting a world and rejoining
		/// </summary>
		public static void Initialize()
		{
			BarProgress = 0;
			Scale = 0;
			Alpha = 0;
			FadeTime = FADE_MAX_TIME;
		}

		//Maybe make this called in player's updating instead, since its all just client based drawing? Not sure which is better, or if there's even a difference and I'm just overthinking it
		/// <summary>
		/// Called every tick during the world's updating process, used to handle logic for fading in/out, bar progress increasing, etc.
		/// </summary>
		public static void Update()
		{
			if (!Main.LocalPlayer.GetModPlayer<StarjinxPlayer>().zoneStarjinxEvent) //If player is not in starjinx event, fade out and decrease in scale, and don't do any other UI updating
			{
				BarProgress = MathHelper.Lerp(BarProgress, 0, 0.03f);
				FadeTime = Math.Min(FadeTime + 1, FADE_MAX_TIME);

				Scale = MathHelper.Lerp(MAX_SCALE, 0, (float)Math.Pow(FadeTime / FADE_MAX_TIME, 0.75f));
				Alpha = MathHelper.Lerp(MAX_ALPHA, 0, (float)Math.Pow(FadeTime / FADE_MAX_TIME, 0.75f));

				return;
			}

			FadeTime = Math.Max(FadeTime - 1, 0);

			//Increase scale and fade in if player is in starjinx event, to a cap
			Scale = MathHelper.Lerp(MAX_SCALE, 0, (float)Math.Pow(FadeTime / FADE_MAX_TIME, 1.5f));
			Alpha = MathHelper.Lerp(MAX_ALPHA, 0, (float)Math.Pow(FadeTime / FADE_MAX_TIME, 1.5f));

			//Variable increase, as to not be too static
			BarProgress = MathHelper.Lerp(BarProgress, StarjinxEventWorld.KilledEnemies / (float)StarjinxEventWorld.MaxEnemies, 0.03f);

			//Constant increase, as to not go too slow when close to desired value
			if (BarProgress < StarjinxEventWorld.KilledEnemies / (float)StarjinxEventWorld.MaxEnemies)
				BarProgress = MathHelper.Min(BarProgress + 0.0015f, StarjinxEventWorld.KilledEnemies / (float)StarjinxEventWorld.MaxEnemies);

			foreach (StarjinxUIComet comet in UiComets)
				comet.Update();
		}

		private static Color DescColor => new Color(33, 24, 68); //Color for the background of the event icon and name, and for the background of the comet icons
		private static Color ProgressBarBGColor => new Color(109, 6, 128); //Background color for the progress bar and progress % text
		private static Color TextColor => Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 2), Color.White, 0.75f); //Color of the event's text

		//Mostly edited from tide ui, which I imagine is just edited from vanilla code
		public static void DrawStarjinxEventUI(SpriteBatch spriteBatch)
		{
			if (Alpha == 0) //Skip drawing if alpha is 0, because terraria's inventory background drawing function defaults to blue if full transparency is used
				return;

			ProgressBarSection(spriteBatch, Scale, Alpha, out Rectangle progressBarBG);
			CometSection(spriteBatch, Scale, Alpha, progressBarBG, out Rectangle cometBG);
			DescriptionSection(spriteBatch, Scale, Alpha, cometBG);
		}

		private static void ProgressBarSection(SpriteBatch spriteBatch, float Scale, float Alpha, out Rectangle progressBarBG)
		{
			//Draw the background for the progress bar
			Rectangle progressBarBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 23f), Size * Scale);
			Utils.DrawInvBG(spriteBatch, progressBarBackground, ProgressBarBGColor * 0.785f * Alpha);
			progressBarBG = progressBarBackground;

			//Display the % for how much of the wave has been cleared
			int progress = (int)(100 * StarjinxEventWorld.KilledEnemies / (float)StarjinxEventWorld.MaxEnemies);
			string waveText = "Wave Progress : " + progress + "%";
			Utils.DrawBorderString(spriteBatch, waveText, new Vector2(progressBarBackground.Center.X, progressBarBackground.Y + 2.5f), TextColor, Scale, 0.5f, -0.1f);
			Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(progressBarBackground.Center.X, progressBarBackground.Y + progressBarBackground.Height * 0.75f), Main.colorBarTexture.Size());

			//Scale the amount of the bar drawn based on progress
			var waveProgressAmount = new Rectangle(0, 0, (int)(Main.colorBarTexture.Width * BarProgress), Main.colorBarTexture.Height);
			var offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * Scale)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * Scale)) * 0.5f);

			//Make the part of the bar not filled out black, by drawing a full length black bar beneath the color bar
			spriteBatch.Draw(Main.colorBarTexture, waveProgressBar.Location.ToVector2() + offset, null, Color.White * Alpha, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

			//Bloom underneath shader bar
			Texture2D bloom = SpiritMod.Instance.GetTexture("Effects/Masks/CircleGradient");
			float sineCounter = (float)(Math.Sin(Main.GlobalTime * 4) * 0.05f) + 1f;

			Color bloomColor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 0.75f), Color.White, 0.33f) * BarProgress * sineCounter;
			bloomColor.A = 0;

			//Scale to match color bar texture size
			Vector2 bloomScale = Main.colorBarTexture.Size() / bloom.Size();
			bloomScale *= 2.2f * Scale * sineCounter;
			bloomScale.Y *= 1.75f;
			bloomScale.X *= BarProgress; //Match progress bar effective width

			Vector2 bloomPos = waveProgressBar.Location.ToVector2() + offset + new Vector2((Main.colorBarTexture.Width / 2) * BarProgress * 0.95f, Main.colorBarTexture.Height / 2) * Scale;
			spriteBatch.Draw(bloom, bloomPos, null, bloomColor, 0f, bloom.Size() / 2, bloomScale, SpriteEffects.None, 0);

			//Draw the colored part of the bar, with a shader
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Effect progressbarEffect = SpiritMod.Instance.GetEffect("Effects/SjinxProgressBar");
			progressbarEffect.Parameters["vnoiseTex"].SetValue(SpiritMod.Instance.GetTexture("Textures/Trails/Trail_2"));
			progressbarEffect.Parameters["timer"].SetValue(Main.GlobalTime * 0.75f);
			progressbarEffect.Parameters["progress"].SetValue(BarProgress);
			progressbarEffect.Parameters["Yellow"].SetValue(new Color(245, 236, 74).ToVector4());
			progressbarEffect.Parameters["Orange"].SetValue(new Color(255, 112, 68).ToVector4());
			progressbarEffect.Parameters["Pink"].SetValue(new Color(255, 84, 231).ToVector4());
			progressbarEffect.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Main.colorBarTexture, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, Color.White, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			//Weaker bloom above shader bar
			spriteBatch.Draw(bloom, bloomPos, null, bloomColor * 0.33f, 0f, bloom.Size() / 2, bloomScale, SpriteEffects.None, 0);
		}

		private static void CometSection(SpriteBatch spriteBatch, float Scale, float Alpha, Rectangle progressBarBG, out Rectangle cometBG)
		{
			//Draw background for comets
			Vector2 cometBgOffset = Vector2.UnitY * InternalOffset;
			Vector2 cometBgSize = new Vector2(6 * Comets[2].Width, Comets[2].Height) * 1.1f * Scale; //Calculated based on size of comet textures and how many to draw, then upscales from that
			Rectangle cometBackground = Utils.CenteredRectangle(progressBarBG.Top() - cometBgOffset - (Vector2.UnitY * cometBgSize.Y / 2), cometBgSize);
			cometBG = cometBackground;
			Utils.DrawInvBG(spriteBatch, cometBackground, DescColor * Alpha);

			//Draw comets

			foreach(StarjinxUIComet comet in UiComets)
			{
				Vector2 position = new Vector2(cometBackground.Center.X + (cometBackground.Width * 0.15f * (comet.Index - 2.5f)), cometBackground.Y + cometBackground.Height * 0.5f);
				comet.Draw(spriteBatch, position, Scale, Alpha * 2);
			}
		}

		private static void DescriptionSection(SpriteBatch spriteBatch, float Scale, float Alpha, Rectangle cometBG)
		{
			//Draw the background for the event icon and name
			Vector2 descSize = new Vector2(154, 40) * Scale;
			Rectangle descBackground = Utils.CenteredRectangle(new Vector2(cometBG.Center.X, cometBG.Y - InternalOffset - descSize.Y * 0.5f), descSize * 0.9f);
			Utils.DrawInvBG(spriteBatch, descBackground, DescColor * Alpha);

			//Draw the event icon and name
			int descOffset = (descBackground.Height - (int)(32f * Scale)) / 2;
			var icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * Scale), (int)(32 * Scale));
			spriteBatch.Draw(EventIcon, icon, Color.White);

			//Make localizeable 
			Utils.DrawBorderString(spriteBatch, "Starjinx", new Vector2(cometBG.Center.X, cometBG.Y - InternalOffset - descSize.Y * 0.5f), TextColor, Scale, 0.3f, 0.4f);
		}
	}

	public class StarjinxUIComet
	{
		public readonly int Index;
		public float Scale { get; set; }
		public float Darkness { get; set; }
		public Texture2D Texture { get; set; }

		public bool IsDestroyed() => Index < 6 - StarjinxEventWorld.CometsRemaining;

		public bool IsActive() => Index == 6 - StarjinxEventWorld.CometsRemaining;

		public StarjinxUIComet(int Index, Texture2D Texture)
		{
			this.Index = Index;
			this.Texture = Texture;
			Scale = 0.7f;
			Darkness = 0.6f;
		}

		public void Update()
		{
			float inactiveScale = 0.7f; //Scale value for when the comet the icon is corresponding to is not the outermost comet
			float inactiveDarkness = 0.6f; //Darkness value for when the comet the icon is corresponding to is not the outermost comet
			float lerpRate = 0.04f;
			float destroyedDarkness = 0.9f;

			if (IsActive())
			{
				Darkness = MathHelper.Lerp(Darkness, 0f, lerpRate);
				Scale = MathHelper.Lerp(Scale, 1f, lerpRate);
			}

			else if (IsDestroyed())
			{
				Darkness = MathHelper.Lerp(Darkness, destroyedDarkness, lerpRate);
				Scale = MathHelper.Lerp(Scale, inactiveScale, lerpRate);
			}

			else
			{
				Darkness = MathHelper.Lerp(Darkness, inactiveDarkness, lerpRate);
				Scale = MathHelper.Lerp(Scale, inactiveScale, lerpRate);
			}
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position, float scaleModifier, float opacity) => spriteBatch.Draw(Texture, position, null, Color.Lerp(Color.White, Color.Black, Darkness) * opacity, 0, Texture.Size() / 2, Scale * scaleModifier, SpriteEffects.None, 0);
	}
}
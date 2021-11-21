using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public static class StarjinxUI
    {
		public static void DrawStarjinxEventUI(SpriteBatch spriteBatch)
		{
			const float Scale = 0.875f;
			const float Alpha = 0.5f;
			const int InternalOffset = 6;
			const int OffsetX = 20;
			const int OffsetY = 20;

			Texture2D EventIcon = SpiritMod.Instance.GetTexture("Textures/InvasionIcons/Starjinx_Icon");
			Color descColor = new Color(33, 24, 68);
			Color waveColor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 2), Color.White, 0.75f);
			Color waveBGColor = new Color(109, 6, 128);
			Color textColor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 2), Color.White, 0.33f);

			int width = (int)(200f * Scale);
			int height = (int)(46f * Scale);

			Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 23f), new Vector2(width, height));
			Utils.DrawInvBG(spriteBatch, waveBackground, waveBGColor * 0.785f);

			int progress = (int)(100 * StarjinxEventWorld.KilledEnemies / (float)StarjinxEventWorld.MaxEnemies);
			string waveText = "Wave Progress : " + progress + "%";
			Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.Center.X, waveBackground.Y + 2.5f), textColor, Scale, 0.5f, -0.1f);
			Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.Center.X, waveBackground.Y + waveBackground.Height * 0.75f), Main.colorBarTexture.Size());

			var waveProgressAmount = new Rectangle(0, 0, (int)(Main.colorBarTexture.Width * 0.01f * progress), Main.colorBarTexture.Height);
			var offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * Scale)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * Scale)) * 0.5f);
			spriteBatch.Draw(Main.colorBarTexture, waveProgressBar.Location.ToVector2() + offset, null, Color.White * Alpha, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.colorBarTexture, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

			Vector2 descSize = new Vector2(154, 40) * Scale;
			Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 19f), new Vector2(width, height));
			Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), descSize * 0.9f);
			Utils.DrawInvBG(spriteBatch, descBackground, descColor * Alpha);

			int descOffset = (descBackground.Height - (int)(32f * Scale)) / 2;
			var icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * Scale), (int)(32 * Scale));
			spriteBatch.Draw(EventIcon, icon, Color.White);
			Utils.DrawBorderString(spriteBatch, "Starjinx", new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), textColor, 0.8f, 0.3f, 0.4f);
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace SpiritMod.Skies.Overlays
{
	public class AuroraOverlay : Overlay
	{
		private readonly int BG_TOP_OFFSET = 460; //this is the offset that the top of the aurora has in comparison to the top of the backgrounds, before camera offset
		private readonly int CAMERA_OFFSET_MULT = 700; //this is how much moving the camera up and down effects the position of the auroras

		private float time;

		public AuroraOverlay(EffectPriority priority = EffectPriority.High, RenderLayers layer = RenderLayers.Sky) : base(priority, layer)
		{
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			this.Mode = OverlayMode.FadeIn;
		}

		public override void Deactivate(params object[] args)
		{
			this.Mode = OverlayMode.FadeOut;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			DrawAurora(spriteBatch);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}

		public override bool IsVisible()
		{
			return !Main.gameMenu;
		}

		public override void Update(GameTime gameTime)
		{
			time = (float)gameTime.TotalGameTime.TotalSeconds * 3f;
		}

		private void DrawAurora(SpriteBatch spriteBatch)
		{
			//check if the effect is null or not
			if (SpiritMod.auroraEffect == null)
				return;

			//ignore this stuff here
			SpiritMod.auroraEffect.Parameters["noiseTexture"].SetValue(SpiritMod.noise);
			int bgTop = (int)((-Main.screenPosition.Y) / (Main.worldSurface * 16.0 - 550.0) * 200.0);
			float percent = Main.screenPosition.Y / ((float)Main.worldSurface * 16f);
			float bonus = 0;
			if (Main.screenPosition.Y < 1600) {
				bonus = (1600f - Main.screenPosition.Y) * 0.036f;
				bonus *= bonus;
			}
			int basePoint = (int)(bgTop + BG_TOP_OFFSET - percent * (CAMERA_OFFSET_MULT - bonus));

            //--THIS IS WHERE YOU EDIT!
            if (MyWorld.auroraType == 0)
            {
                DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(137, 48, 255), new Color(125, 0, 255), 0.0031f, 0.8f, 0.2f);
                DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(0, 143, 233), new Color(0, 255, 255), 0.0031f, 1f, 0.32f);
            }
            if (MyWorld.auroraType == 1) {
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(0, 143, 233), new Color(0, 255, 255), 0.0031f, 1f, 0.1f);
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(60, 200, 183), new Color(0, 255, 100), 0.0031f, 1f, 0.4f);
			}
			if (MyWorld.auroraType == 2) {
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(0, 143, 233), new Color(0, 255, 255), 0.0031f, 1f, 0.1f);
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(60, 200, 183), new Color(0, 255, 100), 0.0031f, 1f, 0.4f);
			}
			if (MyWorld.auroraType == 3) {
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(0, 143, 233), new Color(0, 255, 255), 0.0031f, 1f, 0.1f);
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(60, 200, 183), new Color(0, 255, 100), 0.0031f, 1f, 0.43f);
			}
			else if (MyWorld.auroraType == 5) {
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(137, 48, 255), new Color(125, 0, 255), 0.0031f, 0.8f, 0.2f);
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(0, 143, 233), new Color(0, 255, 255), 0.0031f, 1f, 0.32f);
			}
			else if (MyWorld.auroraType == 6) {
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(247, 39, 67), new Color(206, 51, 72), 0.0031f, 0.8f, 0.1f);
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(252, 42, 186), new Color(143, 15, 155), 0.0031f, 1f, 0.32f);
			}
			else if (MyWorld.auroraType == 7) {
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(255, 170, 114), new Color(255, 170, 114), 0.0028f, 0.8f, 0.2f);
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(255, 195, 56), new Color(255, 195, 56), 0.0028f, 0.8f, 0.17f);

			}
			else if (MyWorld.auroraType == 8) {
				DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(124, 142, 255), new Color(124, 142, 255), 0.0028f, 0.8f, 0.1f);
				DrawSingularAurora(spriteBatch, basePoint, 20, 400, new Color(127, 255, 250), new Color(127, 255, 250), 0.0028f, 0.8f, 0.37f);
			}
            else if (MyWorld.auroraType == 9)
            {
                DrawSingularAurora(spriteBatch, basePoint, 0, 400, new Color(255, 156, 197), new Color(255, 223, 153), 0.0028f, 0.8f, 0.1f);
            }
            if (!MyWorld.aurora) {
				Deactivate();
			}
		}

		//basePoint is the value that has already been provided
		//yOffset is how far from the base point you want the aurora to be in the y direction
		//height is how tall the aurora should be
		//topColor is the colour at the top (not very visible but there)
		//bottomColor is the colour at the bottom
		//speed is how fast the aurora moves, this doesnt affect the waviness, only the bands
		//opacity is the opacity of the aurora (this does not include the normal overlay opacity)
		//randY is a random y value you have to specify to change up each one, just pick a random y value each time you make a new one (no two should have the same decimal number, like 0.6 and 1.6 is bad, but 0.6 and 1.3 is good :D )
		private void DrawSingularAurora(SpriteBatch spriteBatch, int basePoint, int yOffset, int height, Color topColor, Color bottomColor, float speed, float opacity, float randY)
		{
			spriteBatch.Begin(SpriteSortMode.Immediate, null);

			SpiritMod.auroraEffect.Parameters["time"].SetValue(time + (int)(randY * 1000));
			SpiritMod.auroraEffect.Parameters["yCoord"].SetValue(randY);
			SpiritMod.auroraEffect.Parameters["colour1"].SetValue(topColor.ToVector3()); //these are your colours, the first one is the colour at the top
			SpiritMod.auroraEffect.Parameters["colour2"].SetValue(bottomColor.ToVector3());
			SpiritMod.auroraEffect.Parameters["movement"].SetValue(speed); //this is the movement of each band layer. wind speed maybe?
			SpiritMod.auroraEffect.Parameters["opacity"].SetValue(opacity * this.Opacity); //this is the movement of each band layer. wind speed maybe?
			SpiritMod.auroraEffect.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, basePoint + yOffset, Main.screenWidth, height), Color.White);

			spriteBatch.End();
		}
	}
}

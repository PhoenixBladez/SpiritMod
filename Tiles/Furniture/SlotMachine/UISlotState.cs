using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using SpiritMod;

namespace SpiritMod.Tiles.Furniture.SlotMachine
{
	class UISlotState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		Player _player;
		int X;
		int Y;
		public UISlotState(int x, int y, Player player) {
			X = x;
			Y = y;
			_player = player;
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			//mainPanel.Left.Set(300f, 0f);
			//mainPanel.Top.Set(300f, 0f);
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(300f, 0f);
			mainPanel.Height.Set(188f, 0f);
			//mainPanel.SetPadding(12);
			//mainPanel.SetPadding(0);
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
				//mainPanel.HAlign = 0f;
				//mainPanel.VAlign = 0f;
			}

			//mainPanel.BackgroundColor = UICommon.DefaultUIBlue;

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/SlotMachineBaseFramer"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/Close");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-50, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			Texture2D pullTexture = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/SlotMachineBaseFramer");
			UIImageButton pullButton = new UIImageButton(pullTexture);
			pullButton.Left.Set(-10, 1f);
			pullButton.Top.Set(-8, 0f);
			pullButton.Width.Set(24, 0f);
			pullButton.Height.Set(24, 0f);
			pullButton.OnClick += PullButton_OnClick;
			panelBackground.Append(pullButton);

        }
		private int drawCounter;
		private float pulleyCounter = -1;
		
		private float symbolCounterOne = 0;
		private float symbolCounterTwo = 0;
		private float symbolCounterThree = 0;
		public override void Update(GameTime gameTime)
		{
			if (pulleyCounter < 30)
			{
				symbolCounterOne+= Main.rand.NextFloat(0.25f,0.45f) * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
			}
			if (pulleyCounter < 40)
			{
				symbolCounterTwo+= Main.rand.NextFloat(0.25f,0.45f) * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
			}
			if (pulleyCounter < 50)
			{
				symbolCounterThree+= Main.rand.NextFloat(0.25f,0.45f) * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
			}
			if (pulleyCounter >= 0)
			{
				pulleyCounter += 0.2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
			}
			base.Update(gameTime);

			Vector2 dist = _player.Center - (new Vector2(X,Y) * 16);
			if (dist.Length() > 100)
			{
				Main.PlaySound(SoundID.MenuClose);
				ModContent.GetInstance<SpiritMod>().SlotUserInterface.SetState(null);
			}
			if (pulleyCounter >= 55)
			{
				CheckRewards();
			}
		}
		private void CheckRewards()
		{
			if (((int)symbolCounterThree % numberOfSymbols) == ((int)symbolCounterTwo % numberOfSymbols) && ((int)symbolCounterThree % numberOfSymbols) == ((int)symbolCounterOne % numberOfSymbols))
			{
				switch ((int)symbolCounterThree % numberOfSymbols)
				{
					case 0: //bar
						CombatText.NewText(new Microsoft.Xna.Framework.Rectangle(X * 16, Y * 16, _player.width, _player.height), Microsoft.Xna.Framework.Color.Gold,
				   		"Bar!");
						break;
					case 1: //seven
						CombatText.NewText(new Microsoft.Xna.Framework.Rectangle(X * 16, Y * 16, _player.width, _player.height), Microsoft.Xna.Framework.Color.Gold,
				   		"Lucky Seven!");
						break;
					case 2: //cherry
						CombatText.NewText(new Microsoft.Xna.Framework.Rectangle(X * 16, Y * 16, _player.width, _player.height), Microsoft.Xna.Framework.Color.Red,
				   		"Smells like Cherries!");
						break;
				}
				ModContent.GetInstance<SpiritMod>().SlotUserInterface.SetState(null);
			}
		}
		const int numberOfSymbols = 3;
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (mainPanel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (mainPanel.Left.Pixels != 0) {
				offsetX = mainPanel.Left.Pixels;
				offsetY = mainPanel.Top.Pixels;
			}
			drawCounter++;
			Microsoft.Xna.Framework.Rectangle rectbase = new Microsoft.Xna.Framework.Rectangle(drawCounter % 16 > 7 ? 0 : 246, 0, 246, 188);
			int width = spriteBatch.GraphicsDevice.Viewport.Width;
			int height = spriteBatch.GraphicsDevice.Viewport.Height;
			Vector2 position = new Vector2(width, height) * new Vector2(mainPanel.HAlign, mainPanel.VAlign);	
			Texture2D texturebase = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/SlotMachineBase");
			spriteBatch.Draw(texturebase, new Vector2(offsetX, offsetY) + position, rectbase, Microsoft.Xna.Framework.Color.White, 0, new Vector2(texturebase.Width / 4, texturebase.Height / 2), 1, SpriteEffects.None, 0f);

			Texture2D texturepull = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/SlotMachinePulley");
			Microsoft.Xna.Framework.Rectangle rectpull = new Microsoft.Xna.Framework.Rectangle(26 * Clamp((int)pulleyCounter, 0, 6), 0, 26, 80);
			spriteBatch.Draw(texturepull, new Vector2(offsetX, offsetY) + position + new Vector2(texturebase.Width / 4, texturebase.Height / -2), rectpull, Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

			Texture2D texturesymbols = ModContent.GetTexture("SpiritMod/Tiles/Furniture/SlotMachine/SlotMachineSymbols");
			
			Vector2 symbolOffsetOne = new Vector2(24,70);
			Vector2 symbolOffsetTwo = symbolOffsetOne + new Vector2(68,0);
			Vector2 symbolOffsetThree = symbolOffsetTwo + new Vector2(68,0);

			Microsoft.Xna.Framework.Rectangle rectSymbolOne = new Microsoft.Xna.Framework.Rectangle(62 * ((int)symbolCounterOne % numberOfSymbols), 0, 62, 70);
			Microsoft.Xna.Framework.Rectangle rectSymbolTwo = new Microsoft.Xna.Framework.Rectangle(62 * ((int)symbolCounterTwo % numberOfSymbols), 0, 62, 70);
			Microsoft.Xna.Framework.Rectangle rectSymbolThree = new Microsoft.Xna.Framework.Rectangle(62 * ((int)symbolCounterThree % numberOfSymbols), 0, 62, 70);

			spriteBatch.Draw(texturesymbols, symbolOffsetOne + position + new Vector2(offsetX, offsetY) + new Vector2(texturebase.Width / -4, texturebase.Height / -2), rectSymbolOne, Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
			spriteBatch.Draw(texturesymbols, symbolOffsetTwo + position + new Vector2(offsetX, offsetY) + new Vector2(texturebase.Width / -4, texturebase.Height / -2), rectSymbolTwo, Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
			spriteBatch.Draw(texturesymbols, symbolOffsetThree + position + new Vector2(offsetX, offsetY) + new Vector2(texturebase.Width / -4, texturebase.Height / -2), rectSymbolThree, Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

			base.DrawSelf(spriteBatch);
		}

		private void CloseButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
			Main.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<SpiritMod>().SlotUserInterface.SetState(null);
		}
		private void PullButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
			if (pulleyCounter == -1 || pulleyCounter > 60)
			{
				Main.PlaySound(SoundID.MenuClose);
				pulleyCounter = 0;
			}
		}
		private static int Clamp(int x, int min, int max) //for some reason Math.Clamp wouldn't work
		{
			if (min <= x && x <= max)
			{
				return x;
			}
			if (min > x)
			{
				return min;
			}
			if (max < x)
			{
				return max;
			}
			return x;
		}
	}
}

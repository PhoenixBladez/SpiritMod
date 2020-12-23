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

namespace SpiritMod.Items.Books.UI
{
	class UISpiritArtState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UISpiritArtState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			//mainPanel.Left.Set(300f, 0f);
			//mainPanel.Top.Set(300f, 0f);
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(1280f, 0f);
			mainPanel.Height.Set(720f, 0f);
			//mainPanel.SetPadding(12);
			//mainPanel.SetPadding(0);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
				//mainPanel.HAlign = 0f;
				//mainPanel.VAlign = 0f;
			}

			//mainPanel.BackgroundColor = UICommon.DefaultUIBlue;

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/TheSpirit"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-30, 1f);
			closeButton.Top.Set(0, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);


			//UIPanel messageBoxPanel = new UIPanel {
			//	Width = { Percent = 1f },
			//	Height = { Pixels = -110, Percent = 1f },
			//	Top = { Pixels = 110, },
			//	BackgroundColor = UICommon.MainPanelBackground
			//};
			//mainPanel.Append(messageBoxPanel);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

            //var messageBoxScrollbar = new FixedUIScrollbar(ModContent.GetInstance<SpiritMod>().BookUserInterface) {
            //	Height = { Pixels = -20, Percent = 1f },
            //	VAlign = 0.5f,
            //	HAlign = 1f
            //}.WithView(100f, 1000f);
            ////messageBoxPanel.Append(messageBoxScrollbar);
            //messageBox.Append(messageBoxScrollbar);

            //messageBox.SetScrollbar(messageBoxScrollbar);

        }

		// A hack to fix scroll bar usage scrolling the item hotbar
		internal static void OnScrollWheel_FixHotbarScroll(UIScrollWheelEvent evt, UIElement listeningElement) {
			Main.LocalPlayer.ScrollHotbar(Terraria.GameInput.PlayerInput.ScrollWheelDelta / 120);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (mainPanel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (mainPanel.Left.Pixels != 0) {
				offsetX = mainPanel.Left.Pixels;
				offsetY = mainPanel.Top.Pixels;
			}
			base.DrawSelf(spriteBatch);
		}

		private void CloseButton_OnClick(UIMouseEvent evt, UIElement listeningElement) {
			Main.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(null);
		}
	}
}

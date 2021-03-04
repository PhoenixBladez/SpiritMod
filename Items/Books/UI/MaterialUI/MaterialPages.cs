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

namespace SpiritMod.Items.Books.UI.MaterialUI
{
	class UIGraniteMaterialPageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIGraniteMaterialPageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/GraniteMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
	class UIMarbleMaterialPageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIMarbleMaterialPageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/MarbleMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
	class UIGlowrootPageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIGlowrootPageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/GlowrootPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
	class UIBismitePageStsate : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIBismitePageStsate() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/BismiteMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
	class UIFrigidFragmentPageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIFrigidFragmentPageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/FrigidFragmentMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
	class UIHeartScalePageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIHeartScalePageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/HeartScaleMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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
    class UIEnchantedLeafPageState : UIState
	{
		private UIDragableElement mainPanel;

		internal static float offsetX = -1;
		internal static float offsetY = -1;

		public UIEnchantedLeafPageState() {
		}

		public override void OnInitialize() {
			mainPanel = new UIDragableElement();
			mainPanel.HAlign = 0.5f;
			mainPanel.VAlign = 0.5f;
			mainPanel.Width.Set(756f, 0f);
			mainPanel.Height.Set(323f, 0f);
			mainPanel.OnScrollWheel += OnScrollWheel_FixHotbarScroll;
			Append(mainPanel);

			if (offsetX != -1) {
				mainPanel.Left.Set(offsetX, 0f);
				mainPanel.Top.Set(offsetY, 0f);
			}

			var panelBackground = new UIImage(ModContent.GetTexture("SpiritMod/Items/Books/UI/MaterialUI/EnchantedLeafMaterialPage"));
			panelBackground.SetPadding(12);
			mainPanel.Append(panelBackground);
			mainPanel.AddDragTarget(panelBackground);

			Texture2D closeTexture = ModContent.GetTexture("SpiritMod/Items/Books/UI/closeButton");
			UIImageButton closeButton = new UIImageButton(closeTexture);
			closeButton.Left.Set(-20, 1f);
			closeButton.Top.Set(5, 0f);
			closeButton.Width.Set(15, 0f);
			closeButton.Height.Set(15, 0f);
			closeButton.OnClick += CloseButton_OnClick;
			panelBackground.Append(closeButton);

			UIElement messageBoxPanel = new UIElement {
				Width = { Percent = 1f },
				Height = { Pixels = -50, Percent = 1f },
				Top = { Pixels = 50, },
			};
			panelBackground.Append(messageBoxPanel);
			mainPanel.AddDragTarget(messageBoxPanel);

        }
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

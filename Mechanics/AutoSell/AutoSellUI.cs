using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace SpiritMod.Mechanics.AutoSell
{
	public class AutoSellUI : UIState
	{
		public static bool visible = false;
		private AutoSellUIElement fullBrightUI = new AutoSellUIElement();

		public override void OnInitialize()
		{
			Texture2D buttonPlayTexture = SpiritModAutoSellTextures.autoSellUIButton;
			UIImageButton playButton = new UIImageButton(buttonPlayTexture);
			playButton.Left.Set(494, 0f);
			playButton.Top.Set(312, 0f);
			playButton.Width.Set(39, 0f);
			playButton.Height.Set(39, 0f);
			playButton.OnClick += new MouseEvent(PlayButtonClicked);
			
			playButton.Append(fullBrightUI);
			Append(playButton);
		}

		private void PlayButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Player player = Main.LocalPlayer;
			player.GetModPlayer<AutoSellPlayer>().SpiritModMechanic = true;
			SoundEngine.PlaySound(SoundID.Coins);
		}
	}
}
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SpiritMod.Mechanics.AutoSell.Sell_Lock
{
	public class Sell_Lock : UIState
	{
		public static bool visible = false;
		public override void OnInitialize()
		{
			Texture2D buttonPlayTexture = SpiritModAutoSellTextures.sellLockButton;
			UIImageButton playButton = new UIImageButton(buttonPlayTexture);
			playButton.Left.Set(502, 0f);
			playButton.Top.Set(356, 0f);
			playButton.Width.Set(24, 0f);
			playButton.Height.Set(24, 0f);
			playButton.OnClick += new MouseEvent(PlayButtonClicked);

			base.Append(playButton);
		}

		private void PlayButtonClicked(UIMouseEvent evt, UIElement listeningElement)
		{
			Player player = Main.LocalPlayer;
			if (!player.GetModPlayer<AutoSellPlayer>().sell_Lock)
			{
				player.GetModPlayer<AutoSellPlayer>().sell_Lock = true;
				Main.PlaySound(SoundID.MenuOpen);
			}
			else
			{
				Main.PlaySound(SoundID.MenuClose);
				player.GetModPlayer<AutoSellPlayer>().sell_Lock = false;
			}
		}
	}
}
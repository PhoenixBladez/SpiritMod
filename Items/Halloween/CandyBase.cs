using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public abstract class CandyBase : ModItem
	{
		public override bool ItemSpace(Player player)
		{
			Item[] inv = player.inventory;
			for (int i = 0; i < 50; i++) {
				if (inv[i].IsAir || inv[i].type != ModContent.ItemType<CandyBag>())
					continue;
				if (!((CandyBag)inv[i].ModItem).Full)
					return true;
			}
			return false;
		}

		public override bool OnPickup(Player player)
		{
			Item[] inv = player.inventory;
			for (int i = 0; i < 50; i++) {
				if (inv[i].IsAir || inv[i].type != ModContent.ItemType<CandyBag>())
					continue;
				if (((CandyBag)inv[i].ModItem).TryAdd(this))
				{
					PopupText.NewText(PopupTextContext.Advanced, );
					SoundEngine.PlaySound(SoundID.Grab, player.Center);
					return false;
				}
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (CanRightClick())
				tooltips.Add(new TooltipLine(Mod, "RightclickHint", "Right click to put into Candy Bag"));
		}

		public override bool CanRightClick()
		{
			return ItemSpace(Main.player[Main.myPlayer]);
		}

		public override void RightClick(Player player)
		{
			Item[] inv = player.inventory;
			for (int i = 0; i < 50; i++) {
				if (inv[i].IsAir || inv[i].type != ModContent.ItemType<CandyBag>())
					continue;
				if (((CandyBag)inv[i].ModItem).TryAdd(this)) {
					SoundEngine.PlaySound(SoundID.Grab, player.Center);
					return;
				}
			}
			//No bags with free space found.

			//Needed to counter the default consuption.
			Item.stack++;
		}
	}
}

using SpiritMod.GlobalClasses.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Accessory
{
	public class AssassinMagazine : ModItem
	{
		public override bool Autoload(ref string name)
		{
			DoubleTapPlayer.OnDoubleTap += DoubleTapPlayer_OnDoubleTapUp;
			return base.Autoload(ref name);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assassin's Magazine");
			Tooltip.SetDefault("Double tap {0} while holding a ranged weapon to swap ammo types\nWorks while in the inventory");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string down = !Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";

			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
					line.Text = line.Text.Replace("{0}", down);
			}
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		private void DoubleTapPlayer_OnDoubleTapUp(Player player, int keyDir)
		{
			if (keyDir == 1 && player.GetSpiritPlayer().assassinMag && player.HeldItem.useAmmo > AmmoID.None)
			{
				var ammoItems = new List<Item>();
				var ammoPos = new List<int>();
				// 54-57 are the ammo slots
				for (int i = 54; i < 58; i++)
				{
					if (!player.inventory[i].IsAir && player.inventory[i].ammo == player.HeldItem.useAmmo)
					{
						ammoItems.Add(player.inventory[i]);
						ammoPos.Add(i);
					}
				}

				if (ammoItems.Count > 0)
				{
					// Shift the top item to the bottom
					var temp = ammoItems[0];
					ammoItems.RemoveAt(0);
					ammoItems.Add(temp);
					// Move the items around accordingly and trigger sync messages
					for (int i = 0; i < ammoItems.Count; i++)
					{
						player.inventory[ammoPos[i]] = ammoItems[i];
						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.SyncEquipment, -1, -1, null, player.whoAmI, ammoPos[i]);
					}
					//Display a text for the item you just swapped to
					Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
					CombatText.NewText(textPos, ammoItems[0].RarityColor(), ammoItems[0].Name);
				}
			}
		}

		public override void UpdateInventory(Player player) => player.GetSpiritPlayer().assassinMag = true;
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().assassinMag = true;
	}
}

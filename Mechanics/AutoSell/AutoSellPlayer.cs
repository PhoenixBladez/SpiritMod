using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.ModLoader.IO;
using SpiritMod.Utilities;
using Terraria.ID;

namespace SpiritMod.Mechanics.AutoSell
{
	public class AutoSellPlayer : ModPlayer
	{
		public bool SpiritModMechanic = false;
		public bool sell_NoValue = false;
		public bool sell_Lock = false;
		public bool sell_Weapons = false;

		public override void PostUpdate()
		{
			var config = ModContent.GetInstance<SpiritClientConfig>();			
			if (config.QuickSell)
			{
				for (int index = 10; index < 50; index++)
				{
					if (sell_Weapons)
					{
						if (!Player.inventory[index].favorited && SpiritModMechanic && Main.npcShop > 0 && Main.mouseItem.type == ItemID.None && !sell_Lock && Player.inventory[index].damage < 1)
						{
							if (sell_NoValue)
							{
								Player.SellItem(Player.inventory[index], Player.inventory[index].stack);
								Player.inventory[index].TurnToAir();
							}
							else
							{
								if (Player.inventory[index].value > 0)
								{
									Player.SellItem(Player.inventory[index], Player.inventory[index].stack);
									Player.inventory[index].TurnToAir();
								}
							}
						}
					}
					else
					{
						if (!Player.inventory[index].favorited && SpiritModMechanic && Main.npcShop > 0 && Main.mouseItem.type == ItemID.None && !sell_Lock)
						{
							if (sell_NoValue)
							{
								Player.SellItem(Player.inventory[index], Player.inventory[index].stack);
								Player.inventory[index].TurnToAir();
							}
							else
							{
								if (Player.inventory[index].value > 0)
								{
									Player.SellItem(Player.inventory[index], Player.inventory[index].stack);
									Player.inventory[index].TurnToAir();
								}
							}
						}
					}
				}
				SpiritModMechanic = false;
				if (Main.playerInventory && Main.npcShop > 0)
				{
					SpiritModAutoSellTextures.Load();
					AutoSellUI.visible = true;
					Sell_NoValue.Sell_NoValue.visible = true;
					Sell_Lock.Sell_Lock.visible = true;
					Sell_Weapons.Sell_Weapons.visible = true;			
				}
				else
				{
					SpiritModAutoSellTextures.Unload();
					AutoSellUI.visible = false;
					Sell_NoValue.Sell_NoValue.visible = false;
					Sell_Lock.Sell_Lock.visible = false;
					Sell_Weapons.Sell_Weapons.visible = false;
				}
			}
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Write(sell_NoValue);
			packet.Write(sell_Lock);
			packet.Write(sell_Weapons);
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("sell_NoValue", sell_NoValue);
			tag.Add("sell_Lock", sell_Lock);
			tag.Add("sell_Weapons", sell_Weapons);
		}

		public override void LoadData(TagCompound tag)
		{
			sell_NoValue = tag.GetBool("sell_NoValue");
			sell_Lock = tag.GetBool("sell_Lock");
			sell_Weapons = tag.GetBool("sell_Weapons");
		}
	}
}
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
						if (!player.inventory[index].favorited && SpiritModMechanic && Main.npcShop > 0 && Main.mouseItem.type == ItemID.None && !sell_Lock && player.inventory[index].damage < 1)
						{
							if (sell_NoValue)
							{
								player.SellItem(player.inventory[index].value, player.inventory[index].stack);
								player.inventory[index].TurnToAir();
							}
							else
							{
								if (player.inventory[index].value > 0)
								{
									player.SellItem(player.inventory[index].value, player.inventory[index].stack);
									player.inventory[index].TurnToAir();
								}
							}
						}
					}
					else
					{
						if (!player.inventory[index].favorited && SpiritModMechanic && Main.npcShop > 0 && Main.mouseItem.type == ItemID.None && !sell_Lock)
						{
							if (sell_NoValue)
							{
								player.SellItem(player.inventory[index].value, player.inventory[index].stack);
								player.inventory[index].TurnToAir();
							}
							else
							{
								if (player.inventory[index].value > 0)
								{
									player.SellItem(player.inventory[index].value, player.inventory[index].stack);
									player.inventory[index].TurnToAir();
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
		
		public override void clientClone(ModPlayer clientClone)
		{
			AutoSellPlayer clone = clientClone as AutoSellPlayer;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(sell_NoValue);
			packet.Write(sell_Lock);
			packet.Write(sell_Weapons);
		}

		public override TagCompound Save()
		{
			return new TagCompound {
				{"sell_NoValue", sell_NoValue},
				{"sell_Lock", sell_Lock},
				{"sell_Weapons", sell_Weapons}
			};
		}

		public override void Load(TagCompound tag)
		{
			sell_NoValue = tag.GetBool("sell_NoValue");
			sell_Lock = tag.GetBool("sell_Lock");
			sell_Weapons = tag.GetBool("sell_Weapons");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
		}
	}
}
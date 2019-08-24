using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Halloween
{
	class CandyBag : ModItem
	{
		public const ushort MaxCandy = 99;
		public const int CandyTypes = 9;
		public static int _type;

		private static int[] types;
		internal static void Initialize()
		{
			types = new int[CandyTypes];
			types[0] = Candy._type;
			types[1] = Apple._type;
			types[2] = ChocolateBar._type;
			types[3] = Lollipop._type;
			types[4] = Taffy._type;
			types[5] = HealthCandy._type;
			types[6] = ManaCandy._type;
			types[7] = GoldCandy._type;
			types[8] = MysteryCandy._type;
		}

		public static int TypeToSlot(int type)
		{
			for (int i = types.Length - 1; i >= 0; i--)
			{
				if (types[i] == type)
					return i;
			}
			return -1;
		}
		public static int SlotToType(int slot)
		{
			if (slot < types.Length)
				return types[slot];
			return Candy._type;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Candy Bag");
			Tooltip.SetDefault("Holds up to "+ MaxCandy +" pieces of candy");
		}


		private int pieces;
		private byte[] candy;
		private byte[] variants;

		public bool ContainsCandy => pieces > 0;
		public bool Full => pieces >= MaxCandy;

		public CandyBag()
		{
			pieces = 0;
			variants = new byte[Candy.CandyNames.Count];
			candy = new byte[CandyTypes];
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = 3;
			item.maxStack = 1;
		}

		//public override bool CanUseItem(Player player)
		//{
		//	return ContainsCandy;
		//}

		//public override bool UseItem(Player player)
		//{
		//	PrintContents();
		//	int total = 0;
		//	for (int i = candy.Length-1; i >= 0; i--)
		//		total += candy[i];
		//	int remove = Main.rand.Next(total);
		//	for (int i = candy.Length-1; i >= 0; i--)
		//	{
		//		if (remove < candy[i])
		//		{
		//			candy[i]--;
		//			break;
		//		}
		//		remove -= candy[i];
		//	}
		//	return false;
		//}

		public bool TryAdd(ModItem item)
		{
			if (Full)
				return false;

			int slot = TypeToSlot(item.item.type);
			if (slot < 0)
				return false;
			if (slot == 0)
				variants[((Candy)item).Variant]++;

			candy[slot]++;
			pieces++;
			return true;
		}

		public void PrintContents()
		{
			Main.NewText("Candy Bag ["+ pieces +"]");
			Main.NewText("Candy: "+ candy[0]);
			for (int i = variants.Length-1; i >= 0; i--)
			{
				if (variants[i] > 0)
					Main.NewText("["+i+"]"+Candy.CandyNames[i] + ": " + variants[i]);
			}
		}

		public override bool CanRightClick()
		{
			return ContainsCandy;
		}

		public override void RightClick(Player player)
		{
			//Needed to counter the default consuption.
			this.item.stack++;

			if (!ContainsCandy)
				return;
			int remove = Main.rand.Next(pieces);
			int i = 0;
			for (; i < candy.Length; i++)
			{
				if (remove < candy[i])
					break;
				
				remove -= candy[i];
			}
			int slot = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, SlotToType(i), 1, true);
			Item item = Main.item[slot];
			if (i == 0)
			{
				remove = Main.rand.Next(candy[0]);
				int v = variants.Length-1;
				for (; v >= 0; v--)
				{
					if (remove < variants[v])
					{
						variants[v]--;
						((Candy)item.modItem).Variant = v;
						break;
					}
					remove -= variants[v];
				}
			}
			if (i < candy.Length)
				candy[i]--;
			pieces--;
			Item[] inv = player.inventory;
			for (int j = 0; j < 50; j++)
			{
				if (!inv[j].IsAir)
					continue;
				inv[j] = item;
				Main.item[slot] = new Item();
				return;
			}
			item.velocity.X = 4 * player.direction + player.velocity.X;
			item.velocity.Y = -2f;
			item.noGrabDelay = 100;
			if (Main.netMode != 0)
			{
				NetMessage.SendData(21, -1, -1, null, slot, 1f);
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!ContainsCandy)
				return;

			TooltipLine line = new TooltipLine(mod, "BagContents", "Contains " + pieces + (pieces==1?" piece":" pieces") +" of Candy");
			tooltips.Add(line);
			line = new TooltipLine(mod, "RightclickHint", "Right click to take a piece of Candy");
			tooltips.Add(line);
		}


		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag.Add("candy", candy);
			tag.Add("variants", variants);
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			pieces = 0;
			byte[] arr = tag.GetByteArray("candy");
			for (int i = Math.Min(arr.Length, candy.Length) - 1; i >= 0; i--)
				pieces += (candy[i] = arr[i]);
			arr = tag.GetByteArray("variants");
			for (int i = Math.Min(arr.Length, variants.Length) - 1; i >= 0; i--)
				variants[i] = arr[i];
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = candy.Length-1; i >= 0; i--)
				writer.Write(candy[i]);
			for (int i = variants.Length-1; i >= 0; i--)
				writer.Write(variants[i]);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			pieces = 0;
			for (int i = candy.Length-1; i >= 0; i--)
				pieces += (candy[i] = reader.ReadByte());
			for (int i = variants.Length-1; i >= 0; i--)
				variants[i] = reader.ReadByte();
		}

		public override ModItem Clone(Item item)
		{
			CandyBag clone = (CandyBag)NewInstance(item);
			Array.Copy(candy, clone.candy, candy.Length);
			Array.Copy(variants, clone.variants, variants.Length);
			clone.pieces = pieces;
			return clone;
		}
	}
}

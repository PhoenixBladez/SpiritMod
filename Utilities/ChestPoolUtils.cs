using Terraria.ID;
using System;
using System.Collections.Generic;
using Terraria;
using System.Linq;

namespace SpiritMod.Utilities
{
	public static class ChestPoolUtils
	{
		/// <summary>
		/// Struct containing information related to chest item pools. <br />
		/// Inputting an int[] for Items results in one of the items from that array being picked for that slot randomly, while inputting an int or short directly adds the corresponding item.
		/// </summary>
		public struct ChestInfo
		{
			public object Items;
			public int Stack;
			public float Chance;
			public ChestInfo(object Items, int Stack = 1, float Chance = 1)
			{
				this.Items = Items;
				this.Stack = Stack;
				this.Chance = Chance;
			}

			public List<ChestInfo> ToList() => new List<ChestInfo> { this };
		}

		/// <summary>
		/// Method to greatly reduce the amount of effort needed to make a chest pool. <br />
		/// Input the chest's pool as a list of structs representing the item pool for each slot, stack for that pool, and chance to be added.
		/// </summary>
		public static void PlaceChestItems(List<ChestInfo> list, Chest chest, int startIndex = 0)
		{
			int itemindex = startIndex;

			List<ChestInfo> newlist = new List<ChestInfo>();

			foreach (ChestInfo c in list) { //prune the list based on the chances of items being added and stacks
				if (Main.rand.NextFloat() >= c.Chance || c.Stack == 0)
					continue; //skip

				newlist.Add(c);
			}

			if (chest.item[itemindex].active && newlist.Count > 0) { //check if the spot items are being added to is active
				int itemstomove = itemindex + chest.item.Skip(itemindex).Where(x => x.active).Count();
				for(int i = itemstomove; i >= itemindex; i--) { //copy all the active items after the index being added to over by the amount of elements in the pruned list
					chest.item[i + newlist.Count] = chest.item[i].Clone();
				}
			}

			foreach(ChestInfo c in newlist) { //finally, add the items to the pool
				switch (c.Items) {
					case Array a:
						int[] itempool = (int[])c.Items;
						chest.item[itemindex].SetDefaults(itempool[Main.rand.Next(itempool.Length)]);
						chest.item[itemindex].stack = c.Stack;
						break;
					case int i:
						chest.item[itemindex].SetDefaults((int)c.Items);
						chest.item[itemindex].stack = c.Stack;
						break;
					case short s:
						chest.item[itemindex].SetDefaults((short)c.Items);
						chest.item[itemindex].stack = c.Stack;
						break;
				}

				itemindex++;
			}
		}

		public static void PlaceModChestItemsWCheck(List<ChestInfo> list, Chest chest, ref bool[] placedItems)
		{
			int itemindex = 0;

			int[] importantitempool = (int[])(list.ElementAt(0).Items);
			int itemtoplace = 0;
			bool canplace = false;
			while (!canplace) { //check if the chosen item has been placed before, and if all items havent already been placed
				itemtoplace = WorldGen.genRand.Next(importantitempool.Length);
				if (!placedItems[itemtoplace] || placedItems.All(x => x == true)) {
					placedItems[itemtoplace] = true;
					canplace = true;
					break;
				}
			}
			if (canplace) {
				placedItems[itemtoplace] = true;
				chest.item[itemindex].SetDefaults(importantitempool[itemtoplace]);
				itemindex++;
			}

			foreach (ChestInfo c in list.Skip(1)) { //add the other items to the pool
				switch (c.Items) {
					case Array a:
						int[] itempool = (int[])c.Items;
						chest.item[itemindex].SetDefaults(itempool[Main.rand.Next(itempool.Length)]);
						chest.item[itemindex].stack = c.Stack;
						break;
					case int i:
						chest.item[itemindex].SetDefaults((int)c.Items);
						chest.item[itemindex].stack = c.Stack;
						break;
					case short s:
						chest.item[itemindex].SetDefaults((short)c.Items);
						chest.item[itemindex].stack = c.Stack;
						break;
				}

				itemindex++;
			}
		}

		public static void AddToModdedChest(List<ChestInfo> list, int chestType)
		{
			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if(chest != null && Main.tile[chest.x, chest.y].type == chestType) 
					PlaceChestItems(list, chest);
			}
		}

		public static void AddToModdedChestWithOverlapCheck(List<ChestInfo> list, int chestType)
		{
			int[] items = (int[])(list.ElementAt(0).Items);
			bool[] placeditems = new bool[items.Length];
			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == chestType)
					PlaceModChestItemsWCheck(list, chest, ref placeditems);
			}
		}

		public const int woodChests = 0; 
		public const int goldChests = 1;
		public const int lockedgoldChests = 2;
		public const int ivyChests = 10;
		public const int skyChests = 13;
		public const int spiderChests = 15;
		public const int waterChests = 17;
		public const int dynastyChests = 28;

		public static void AddToVanillaChest(List<ChestInfo> list, int chestFrame, int index = 0)
		{
			chestFrame *= 36;
			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == chestFrame)
					PlaceChestItems(list, chest, index);
			}
		}

		public static void AddToVanillaChest(ChestInfo item, int chestFrame, int index) 
		{
			chestFrame *= 36;
			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == chestFrame)
					PlaceChestItems(item.ToList(), chest, index);

			}
		}
	}
}

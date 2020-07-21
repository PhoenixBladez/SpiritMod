using Terraria;

namespace SpiritMod
{
	public static class NPCUtils
	{
		public static bool CanDamage(this NPC npc)
		{
			return npc.lifeMax > 5 && !npc.friendly;
		}

		public static bool CanLeech(this NPC npc)
		{
			return npc.lifeMax > 5 && !npc.friendly && !npc.dontTakeDamage && !npc.immortal;
		}

		public static bool CanDropLoot(this NPC npc)
		{
			return npc.lifeMax > 5 && !npc.friendly && !npc.SpawnedFromStatue;
		}

		public static void AddItem(ref Chest shop, ref int nextSlot, int item, int price = -1, bool check = true)
		{
			if (check) {
				shop.item[nextSlot].SetDefaults(item);
				if (price >= 0) {
					shop.item[nextSlot].shopCustomPrice = price;
				}

				nextSlot++;
			}
		}
	}
}

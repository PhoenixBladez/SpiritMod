using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

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
	}
}

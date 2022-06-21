using System;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
	public struct QuestPoolData
	{
		public float? NewRate; //Spawn rate mod
		public bool Exclusive; //By default, increase spawns only if
		public bool Forced;
		public Func<NPCSpawnInfo, bool> Conditions;

		public QuestPoolData(float? rate, bool exc = true, bool forced = false, Func<NPCSpawnInfo, bool> cond = null)
		{
			NewRate = rate;
			Exclusive = exc;
			Forced = forced;
			Conditions = cond;

			if (forced && !rate.HasValue)
				throw new ArgumentException("QuestPoolData created wherein forced is true but the rate is not set. Rate must be set in order for forced to work properly.");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public class QuestGlobalNPC : GlobalNPC
    {
		public override bool Autoload(ref string name)
		{
			// we need to manually load this one to ensure it's loaded *after* our other globalnpcs. 
			// actually, it's best if this is after every mod's globalnpc. for cross-mod content purposes and for general mod support.
			// TODO: Figure that one out ^
			return false;
		}

		public static event Action<IDictionary<int, float>, NPCSpawnInfo> OnEditSpawnPool;
        public static event Action<NPC> OnNPCLoot;

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            OnEditSpawnPool?.Invoke(pool, spawnInfo);
        }

        public override void NPCLoot(NPC npc)
        {
            OnNPCLoot?.Invoke(npc);
        }
    }
}

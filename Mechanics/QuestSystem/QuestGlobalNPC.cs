using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Mechanics.QuestSystem
{
    public class QuestGlobalNPC : GlobalNPC
    {
		public static event Action<IDictionary<int, float>, NPCSpawnInfo> OnEditSpawnPool;
        public static event Action<NPC> OnNPCLoot;

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            OnEditSpawnPool?.Invoke(pool, spawnInfo);
        }

        public override void NPCLoot(NPC npc)
        {            
            bool showUnlocks = true;
            if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedAncientFlier || MyWorld.downedMoonWizard || MyWorld.downedRaider)
            {
                QuestManager.UnlockQuest<SlayerQuestOccultist>(showUnlocks);
                QuestManager.UnlockQuest<UnidentifiedFloatingObjects>(showUnlocks);
            }
            if (NPC.downedBoss2)
            {
                QuestManager.UnlockQuest<SlayerQuestMarble>(showUnlocks);
                QuestManager.UnlockQuest<SlayerQuestMeteor>(showUnlocks);                
            }
            if (NPC.downedBoss3)
            {
                QuestManager.UnlockQuest<RaidingTheStars>(showUnlocks);
                QuestManager.UnlockQuest<SongOfIceAndFire>(showUnlocks);  
                QuestManager.UnlockQuest<StrangeSeas>(showUnlocks);                
            }
            OnNPCLoot?.Invoke(npc);
        }
    }
}

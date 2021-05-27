using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ID;

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
            if (npc.type == NPCID.EyeofCthulhu || 
				npc.type == NPCID.EaterofWorldsHead || 
				npc.type == NPCID.SkeletronHead || 
				npc.type == ModContent.NPCType<NPCs.Boss.Scarabeus.Scarabeus>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.AncientFlyer>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.MoonWizard.MoonWizard>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.SteamRaider.SteamRaiderHead>())
            {
                QuestManager.UnlockQuest<SlayerQuestOccultist>(true);
                QuestManager.UnlockQuest<UnidentifiedFloatingObjects>(true);
            }

            if (npc.type == NPCID.EaterofWorldsHead)
			{
                QuestManager.UnlockQuest<SlayerQuestMarble>(true);
                QuestManager.UnlockQuest<SlayerQuestMeteor>(true);                
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                QuestManager.UnlockQuest<RaidingTheStars>(true);
                QuestManager.UnlockQuest<SongOfIceAndFire>(true);  
                QuestManager.UnlockQuest<StrangeSeas>(true);                
            }
            OnNPCLoot?.Invoke(npc);
        }
    }
}

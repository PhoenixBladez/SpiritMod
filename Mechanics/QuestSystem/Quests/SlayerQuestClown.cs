using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestClown : Quest
    {
        public override string QuestName => "No Laughing Matter";
		public override string QuestClient => "The Party Girl";
		public override string QuestDescription => "I was planning a party last week when these DISGUSTING clowns showed up and cramped my style. They blew up my entire venue, so now it's personal. I want you to find those clowns and end them. There can only be one partyholic around these parts.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		//public override bool AnnounceRelocking => true;
		//public override bool LimitedUnlock => true;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)ItemID.Bananarang, 5),
			((int)ItemID.LightShard, 1),
			((int)ItemID.GoldCoin, 3)
		};

		private SlayerQuestClown()
        {
            _tasks.AddTask(new SlayTask((int)NPCID.Clown, 3));
        }

		public override bool IsQuestPossible()
		{
            return Main.hardMode;
        }
		public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			base.OnDeactivate();
		}        
		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool.ContainsKey(NPCID.Clown))
			{
				pool[ModContent.NPCType<NPCs.Hookbat.Hookbat>()] = 0.09f;
			}
		}
		/*public override void OnUnlock()
		{
			UnlockTime = 900;
		}*/
	}
}
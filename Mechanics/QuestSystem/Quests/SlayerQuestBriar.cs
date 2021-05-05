using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestBriar : Quest
    {
        public override string QuestName => "Flowery Fiends";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => " I need ya to go down to the Briar and show those beasts who's boss. Kill some of those big hounds an' those Thorn Stalkers. Maybe we can make the Briar a safer place... Ah, who am I kiddin'? That place is bound to stay a hellhole.";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestBriar()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
        public override void OnQuestComplete()
		{
            bool showUnlocks = true;
			QuestManager.UnlockQuest<RootOfTheProblem>(showUnlocks);
            QuestManager.UnlockQuest<SlayerQuestValkyrie>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestDrBones>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestNymph>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestUGDesert>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestCavern>(showUnlocks);

			base.OnQuestComplete();
        }
    }
}
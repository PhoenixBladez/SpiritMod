using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestMushroom : Quest
    {
        public override string QuestName => "Glowing a Garden";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'm looking for info on any nearby Glowing Mushroom Caverns. You see, I made a bet with the merchant that they DO exist an' that I don't hallucinate giant blue glowing mushroom trees. On second thought, I may not sound too sane sayin' that, but I swear they're real! Go prove me right!";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestMushroom()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
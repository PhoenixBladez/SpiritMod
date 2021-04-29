using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestMarble : Quest
    {
        public override string QuestName => "Forgotten Civilizations";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'd like ya to head out there and map out the lower reaches of this land. A few caverns are covered in marble and the ruins of some ancient civilization. After you stumble upon one of these Marble Caverns, wander around for a while and take some notes for me, alright? Hopefully, those ruins are desolate, eh?";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestMarble()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
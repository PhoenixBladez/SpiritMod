using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestGranite : Quest
    {
        public override string QuestName => "Rocky Road";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A couple of underground cave systems seem to be made almost entirely of dark granite. Some kinda energy source seems to be bringin' the rocks to life, too. I'd like ya to go and investigate. After you stumble upon one of these Granite Caverns, wander around for a while and take some notes for me, alright?";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestGranite()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
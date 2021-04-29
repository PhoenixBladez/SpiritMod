using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestUGDesert : Quest
    {
        public override string QuestName => "To Go Deeper";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "We need to go deeper! That's right, deeper into the desert to slay those pesky terrors that live down there. They've made spelunkin' so difficult for some local miners, and I need you to help clear the path for them!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestUGDesert()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
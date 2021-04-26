using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class SlayerQuestNymph : Quest
    {
        public override string QuestName => "She's a Maniac";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Y'know, after some time resting after being stranded in the Briar, I was really excited to return to the datin' game. Had a nice date lined up and everything. The lady was super pretty an' nice. But when I got to the cave we were supposed to meet in, she tried to eat me! Pesky monster- kill it to give me some closure!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestNymph()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class ExplorerQuestHive : Quest
    {
        public override string QuestName => "Hive Hunting";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Have you checked out the lower parts of the Jungle? I've recently heard about a series of massive hives around there. I loathe bees... an' hornets... an' giant man eatin' plants, so would ya like to check one of these hives out for me?";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestHive()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
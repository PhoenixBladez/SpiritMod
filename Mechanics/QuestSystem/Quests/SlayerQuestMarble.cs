using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestMarble : Quest
    {
        public override string QuestName => "Ancient Gazes";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You've really shaken up the world after slaying the great evil in that disgusting biome. New horrors are startin' to crop up everwhere. The Marble Caverns have seen quite a stir especially. This new monstrosity's got tentacles, eyes, fireballs, you name it. Can you go kill this monstrosity for me, lad?";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestMarble()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestVultureMatriarch : Quest
    {
        public override string QuestName => "Broodmother";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Ever since you took care of that giant wall of flesh, the world's gotten way scarier, lad. We need to take stock of our situation and keep fightin' no matter what. A couple of wanderers said they spotted a giant vulture sleepin' in the desert a few days ago. Check it out and make sure the desert sands stay safe.";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestVultureMatriarch()
        {
            _questSections.Add(new ConcurrentTask(new SlayTask(10, 10), new SlayTask(15, 10), new SlayTask(20, 10)));
        }
    }
}
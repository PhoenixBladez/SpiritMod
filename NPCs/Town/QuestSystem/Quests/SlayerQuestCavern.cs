using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class SlayerQuestCavern : Quest
    {
        public override string QuestName => "Creepy Crawlies";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Some new creepy crawlies have taken to calling the caverns their home. Disgustin' little fellas that belch poison gas and some spiny little buggers. Do us all a favor and exterminate those nasty things. Killing a dozen of 'em will surely make the underground a less nasty place.";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestCavern()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
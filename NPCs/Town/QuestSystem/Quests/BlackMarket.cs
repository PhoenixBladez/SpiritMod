using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class BlackMarket : Quest
    {
        public override string QuestName => "Black Market";
		public override string QuestClient => "The Arms Dealer";
		public override string QuestDescription => "Hey there! Interested in a proposition by yours truly? Now, I'd take every opportunity I could to put a few bullets into anything that moves, but I haven't tested this new shipment of guns I received. Mind taking one of them and testing it out on a few baddies for me?";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Other;

        public BlackMarket()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
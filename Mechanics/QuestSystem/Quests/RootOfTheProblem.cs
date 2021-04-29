using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class RootOfTheProblem : Quest
    {
        public override string QuestName => "Root of the Problem";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Ever since I was captured by those savages from the Briar, I've been doin' some research on the place. That altar you found me at is supposed to harbor a really venegeful nature spirit. Mind investigating? ";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer | QuestType.Main;

        public RootOfTheProblem()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
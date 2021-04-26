using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class ItsNoSalmon : Quest
    {
        public override string QuestName => "It's No Salmon...";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've got a, uh, perfectly normal quest for ya. Why don't you go ahead and head to the Jungle to fish up a Hornetfish for me? It's supposed to be a real delicacy. Be careful, though. I've heard it can be a... tough catch. Whaddya mean, this sounds exactly like something the Angler would want you to do?";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Forager;

        public ItsNoSalmon()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
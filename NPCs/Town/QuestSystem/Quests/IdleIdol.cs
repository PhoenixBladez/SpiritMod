using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class IdleIdol : Quest
    {
        public override string QuestName => "Idle Idol";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "The sands of the desert hide a lot of secrets beneath 'em. There's supposed to be an Ancient Ziggurat buried near the surface of one of those wastelands. Could ya head down there and scavenge some relics from me? ";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Main;

        public IdleIdol()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
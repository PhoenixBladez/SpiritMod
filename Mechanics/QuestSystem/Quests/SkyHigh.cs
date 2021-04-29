using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SkyHigh : Quest
    {
        public override string QuestName => "Sky High";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've been looking at some old maps and I've learned about a cluster of Floating Pagodas above the oceans of this world. Trouble is, I can't make out whether it's to the left or right, so would ya go explorin' for me? I'm looking for an ornate staff, hundreds of years old. Happy hunting!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Forager;

        public SkyHigh()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
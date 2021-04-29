using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class BlastFromThePast : Quest
    {
        public override string QuestName => "Blast From The Past";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I see you've got some Discharge Tubules there, lad. I'm glad you've taken my design advice, so let's take it one step further. Try grabbin' some Enchanted Marble Chunks to really set the scene with a Hyperspace Bust. I'm tellin' ya, it'll look spectacular!";
		public override int Difficulty => 1;
        public override QuestType QuestType => QuestType.Designer;

        public BlastFromThePast()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
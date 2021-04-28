using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class AncestralWorship : Quest
    {
        public override string QuestName => "Ancestral Worship";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "When I was explorin' the Briar with Professor Laywatts and her team, I remember her talkin' about some massive statues the old Briar dwellers used to construct. They've all been destroyed over time, but maybe we could create a replica right here?";
		public override int Difficulty => 1;
        public override QuestType QuestType => QuestType.Designer;

        public AncestralWorship()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
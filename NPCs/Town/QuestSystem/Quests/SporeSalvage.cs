using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class SporeSalvage : Quest
    {
        public override string QuestName => "Spore Salvage";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've been hearin' stories about some new flora that's cropped up around those strange Mushroom Forests recently. These lil' buggers seem to just sway from side to side, as if they're dancin'. I have no real motive this time around, I just wanna see one of 'em. Mind fetching one for me?";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Forager;

        public SporeSalvage()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
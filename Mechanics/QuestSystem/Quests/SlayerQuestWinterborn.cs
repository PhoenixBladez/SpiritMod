using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestWinterborn : Quest
    {
        public override string QuestName => "Beneath the Ice";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A few of my associates have scouted a growing threat in the icy caverns. Sometimes, they even get bold enough to roam the surface durin' heavy blizzards. I need ya to thin their numbers a bit. I'd be cautious when roaming those caverns. Some of those Ice Sculptures look a little too lifelike...";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestWinterborn()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
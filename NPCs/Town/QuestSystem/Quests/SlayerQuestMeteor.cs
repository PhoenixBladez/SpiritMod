using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class SlayerQuestMeteor : Quest
    {
        public override string QuestName => "Astral Inversion";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "We're under attack! Well, not really. But we might be soon- strange aliens and creatures have been gatherin' around that meteor that crashed nearby a while ago. We need to make sure that they're not up to anythin' scary. And by we, I mean you. Get out there an' save our planet, lad!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestMeteor()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
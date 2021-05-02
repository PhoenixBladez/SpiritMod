using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class FirstAdventure : Quest
    {
        public override string QuestName => "The First Adventure";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "So you wanna be an adventurer, eh? Well, pack a bag and get out there! I'd actually planned to craft you a set of special armor. Unfortunately, some mangy Hookbats stole the sheaf of Durasilk I was usin'! They only come out at night around the forest surface. Mind retrievin' that silk for me so I can thank you properly?";
		public override int Difficulty => 1;
        public override QuestType QuestType => QuestType.Main;
		public override bool TutorialActivateButton => true;

		public FirstAdventure()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
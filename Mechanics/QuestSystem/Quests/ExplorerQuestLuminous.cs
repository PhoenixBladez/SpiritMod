using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestLuminous : Quest
    {
        public override string QuestName => "High Tide, Glow Tide";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I love the beach. Now, you may be picturin' the sun and the sand, and I love that stuff too! But I'm talkin' about the beach at night. If you're lucky, you can see some beautiful glowing algae wash up on the shore and turn the water into pretty colors! I'd really recommend kicking back and relaxing at a Luminous Ocean.";
		public override int Difficulty => 1;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestLuminous()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
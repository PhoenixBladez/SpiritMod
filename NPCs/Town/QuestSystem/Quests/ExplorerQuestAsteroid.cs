using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class ExplorerQuestAsteroid : Quest
    {
        public override string QuestName => "Space Rocks";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "There's an asteroid field smack-dab above one of the oceans. Completely uncharted. How cool is that?! We have to learn everything we can about that place. If you stumble upon those asteroids, wander around for a while and take some notes for me, alright? Can you even write in space? Go find out, and don't fall off!";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

        public ExplorerQuestAsteroid()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
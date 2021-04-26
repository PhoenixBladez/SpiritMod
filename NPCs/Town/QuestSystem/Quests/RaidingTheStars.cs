using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class RaidingTheStars : Quest
    {
        public override string QuestName => "Raiding the Stars";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You've checked out the Asteroid Fields near the far corner of the world, right? My sources have reported some increasin' mechanical activity around there." +
				" Some kinda weird automatonic worms made of metal are streakin' through the sky there. Something has to be putting 'em on edge. Could you kill a couple and see what makes 'em tick?";
		public override int Difficulty => 3;
        public override QuestType QuestType =>  QuestType.Main;

        public RaidingTheStars()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
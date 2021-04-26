using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class SongOfIceAndFire : Quest
    {
        public override string QuestName => "Song of Ice and Fire";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "It's been feelin' like the world has gotten more restless recently. I'm sure the monsters around the world haven't taken to kindly to you shakin' things up. It's time to gear up and get stronger. I've noticed two new materials that could be useful to you. Unfortunately, one of 'em is found only in the deepest pits of the Underworld, while the other can only be mined in the frigid depths of the tundra. Are you up to the challenge?";
		public override int Difficulty => 3;
        public override QuestType QuestType =>  QuestType.Forager | QuestType.Main;

        public SongOfIceAndFire()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
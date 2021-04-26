using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class UnidentifiedFloatingObjects : Quest
    {
        public override string QuestName => "Unidentified Floaters";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I quit drinkin' years ago, but I swear the sky's been lighting up something fierce recently! I've been doin' some research and I think the skies may be home to some mystical jellyfish swarms. Now, the only 'proof' I have are some sources of, er, ill repute, but I know I can count on you to check it out! And capture me the tastiest- I mean most interesting one!";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Main;

        public UnidentifiedFloatingObjects()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
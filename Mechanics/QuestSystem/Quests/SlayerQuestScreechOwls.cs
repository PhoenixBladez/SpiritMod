using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestScreechOwls : Quest
    {
        public override string QuestName => "Cacophonous Cries";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'd like to think I'm an animal lover, y'know? I love dogs, an' cats, and all kinds of furry creatures. Except for that horrifying beast... You guessed it, I'm talkin' about Screech Owls. Those things are horrifyin'! Every night, I hear their screeches echoin' from the snowy tundra. It ruins my sleep! Could ya get rid of some for me?";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Slayer;

        public SlayerQuestScreechOwls()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
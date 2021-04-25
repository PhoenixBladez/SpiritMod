using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class CacophonousCries : Quest
    {
        public override string QuestName => "Cacophonous Cries";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'd like to think I'm an animal lover, y'know? I promise I love dogs, an' cats, and all kinds of furry creatures. Except for those horrifying beasts... You guessed it, I'm talkin' about Screech Owls, lad. Whaddya mean 'that isn't what you thought I'd say?' Those things are horrifyin'! Every night, I hear their screeches echoin' from the snowy tundra. It ruins my sleep! Could ya get rid of some for me, just so that I can have peace of mind?";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Slayer;

        public CacophonousCries()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
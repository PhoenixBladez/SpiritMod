using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class FriendSafari : Quest
    {
        public override string QuestName => "Friend Safari";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A few years ago, I was running a job with a friend of mine who's never really been on the good side of the law. He's a bandit, and a mighty fine one at that. We've kept in touch ever since, but he's been awfully silent as of late. Last I remember, he was holed up with a bandit group near the far shores of the world. Would you mind checkin' up on him?";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Main;

        public FriendSafari()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
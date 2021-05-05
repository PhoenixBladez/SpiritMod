using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestNymph : Quest
    {
        public override string QuestName => "She's a Maniac";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Y'know, after some time resting after being stranded in the Briar, I was really excited to return to the datin' game. Had a nice date lined up and everything. The lady was super pretty an' nice. But when I got to the cave we were supposed to meet in, she tried to eat me! Pesky monster- kill it to give me some closure!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.NypmhBanner, 1),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};
        public SlayerQuestNymph()
        {
            _questSections.Add(new KillSection(Terraria.ID.NPCID.Nymph, 1));
        }
    }
}
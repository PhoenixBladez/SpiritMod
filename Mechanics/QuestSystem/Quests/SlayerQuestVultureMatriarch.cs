using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestVultureMatriarch : Quest
    {
        public override string QuestName => "Broodmother";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Ever since you took care of that giant wall of flesh, the world's gotten way scarier, lad. We need to take stock of our situation and keep fightin' no matter what. A couple of wanderers said they spotted a giant vulture sleepin' in the desert a few days ago. Check it out and make sure the desert sands stay safe.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.Vulture_Matriarch.Vulture_Matriarch_Mask>(), 1),
			(Terraria.ID.ItemID.Sandgun, 3),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		private SlayerQuestVultureMatriarch()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Vulture_Matriarch.Vulture_Matriarch>(), 1));
        }
    }
}
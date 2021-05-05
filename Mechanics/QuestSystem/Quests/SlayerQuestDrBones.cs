using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestDrBones : Quest
    {
        public override string QuestName => "Zombies... Why Zombies";
		public override float QuestTitleScale => 0.7f;
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "My colleague, an expert archaeologist, went roamin' the jungle for some ancient temple. He didn't make it, though. Reports have told me that he's still roamin' the Jungle surface as a zombie. Mind going out there and puttin' him to rest for me? He's been exploring enough.";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.ArchaeologistsJacket, 1),
            (Terraria.ID.ItemID.ArchaeologistsPants, 1),
            (Terraria.ID.ItemID.TigerSkin, 1),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};
        public SlayerQuestDrBones()
        {
            _questSections.Add(new KillSection(Terraria.ID.NPCID.DoctorBones, 1));    
        }
    }
}
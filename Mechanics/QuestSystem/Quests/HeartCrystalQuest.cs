using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class HeartCrystalQuest : Quest
    {
        public override string QuestName => "Heart to the Cause";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "So you've got some armor now, an' even a trusty weapon to boot! That's a great start, lad, but you're going to need to get even stronger to adventure properly. Head underground and collect a Life Crystal. They'll make sure you live longer when you're backpackin' across the land!";
		public override int Difficulty => 2;
        public override QuestType QuestType => QuestType.Forager;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.GoldCoin, 1)
		};
		public override void OnQuestComplete()
		{
            bool showUnlocks = true;
			QuestManager.UnlockQuest<RootOfTheProblem>(showUnlocks);
			QuestManager.UnlockQuest<DecrepitDepths>(showUnlocks);
			QuestManager.UnlockQuest<SkyHigh>(showUnlocks);
			QuestManager.UnlockQuest<ItsNoSalmon>(showUnlocks);
			QuestManager.UnlockQuest<SporeSalvage>(showUnlocks);
			QuestManager.UnlockQuest<ManicMage>(showUnlocks);

			base.OnQuestComplete();
		}
		public HeartCrystalQuest()
        {
            _questSections.Add(new RetrievalSection(Terraria.ID.ItemID.LifeCrystal, 1));
        }
    }
}
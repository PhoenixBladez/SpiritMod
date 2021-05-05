using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestCrimson : Quest
    {
        public override string QuestName => "Body Horrors";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "So you're plannin' on living in this crazy world, huh? We've got a lot of explorin' to do before we can get a lay of the land aroun' these parts. Let's start by bitin' the bullet- we need to know how dangerous the Crimson fields and how to deal with those disgusting Face Monsters. Tread with caution, lad.";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.PurificationPowder, 10),
			(Terraria.ID.ItemID.SilverCoin, 60)
		};

		public ExplorerQuestCrimson()
        {
                _questSections.Add(new ExploreSection((Player player) => player.ZoneCrimson, 4000f, "the Crimson"));
        }
		public override bool IsQuestPossible()
		{
            return !WorldGen.crimson;
        }
        public override void OnQuestComplete()
		{
            bool showUnlocks = true;
			QuestManager.UnlockQuest<RootOfTheProblem>(showUnlocks);
            QuestManager.UnlockQuest<ExplorerQuestMarble>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestGranite>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestAsteroid>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestHive>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestMushroom>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestAurora>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestLuminous>(showUnlocks);

            base.OnQuestComplete();
        }
    }
}
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
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "So you're planning on staying in this world for a while, huh? We've got a lot of exploring to do before we can get a grasp of this region. We need to know how dangerous the Crimson fields are and how to deal with those disgusting Face Monsters. I'm not strong enough, so you'll need to handle this alone.";
		public override int Difficulty => 2;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.PurificationPowder, 10),
			(Terraria.ID.ItemID.SilverCoin, 60)
		};

		private ExplorerQuestCrimson()
        {
            _tasks.AddTask(new ExploreTask((Player player) => player.ZoneCrimson, 4000f, "the Crimson"));
        }

		public override bool IsQuestPossible() => WorldGen.crimson;

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
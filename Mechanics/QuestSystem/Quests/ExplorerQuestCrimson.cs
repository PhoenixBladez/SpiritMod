using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestCrimson : Quest
    {
        public override string QuestName => "Body Horrors";
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "So you're planning on staying in this world for a while, huh? We've got a lot of exploring to do before we can get a grasp of this region. We need to know how dangerous the Crimson fields are. I'm not strong enough, so you'll need to handle this alone. If you can survive there, you can explore the other territories the Adventurer mentioned in his journal.";
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
            QuestManager.UnlockQuest<ExplorerQuestMarble>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestGranite>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestAsteroid>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestHive>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestMushroom>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestAurora>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestLuminous>(showUnlocks);

			QuestManager.SayInChat("Click on quests in the chat to open them in the book!", Color.White);

			base.OnQuestComplete();
        }
    }
}
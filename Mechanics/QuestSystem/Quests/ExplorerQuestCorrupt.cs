using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestCorrupt : Quest
    {
        public override string QuestName => "Vile Wastes";
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "So you're planning on staying in this world for a while, huh? We've got a lot of exploring to do before we can get a grasp of this region. We need to know how dangerous those Corrupt Chasms are and how to deal with those freaky Eaters. Tread with caution, you're going alone. If you can survive there, you can explore the other dangerous areas the Adventurer mentioned in his journal.";
		public override int Difficulty => 2;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.PurificationPowder, 10),
			(Terraria.ID.ItemID.SilverCoin, 60)
		};

		private ExplorerQuestCorrupt()
        {
            _tasks.AddTask(new ExploreTask((Player player) => player.ZoneCorrupt, 4000f, "the Corruption"));
        }

		public override bool IsQuestPossible() => !WorldGen.crimson;

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
			base.OnQuestComplete();
        }
	}
}
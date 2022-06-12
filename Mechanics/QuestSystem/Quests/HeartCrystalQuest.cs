namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class HeartCrystalQuest : Quest
    {
        public override string QuestName => "Heart to the Cause";
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "If you want to stay alive and keep exploring this world (with my help, of course), we'll need to make sure you're hale and hearty. Get it? You'll need to head underground to retrieve a Heart Crystal or too. It'll give you the boost you need! After that, you should be able to try out some of the more difficult quests that the Adventurer left behind.";
		public override int Difficulty => 2;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.GoldCoin, 1)
		};

		public override void OnQuestComplete()
		{
            bool showUnlocks = true;
			QuestManager.UnlockQuest<DecrepitDepths>(showUnlocks);
			QuestManager.UnlockQuest<SkyHigh>(showUnlocks);
			QuestManager.UnlockQuest<ItsNoSalmon>(showUnlocks);
			QuestManager.UnlockQuest<ManicMage>(showUnlocks);
			QuestManager.UnlockQuest<FriendSafari>(showUnlocks);
			QuestManager.UnlockQuest<BreakingAndEntering>(showUnlocks);
			base.OnQuestComplete();
		}

		private HeartCrystalQuest()
        {
            _tasks.AddTask(new RetrievalTask(Terraria.ID.ItemID.LifeCrystal, 1));
        }
    }
}
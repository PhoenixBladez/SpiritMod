using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	[System.Obsolete("Quest is not in use until it is redone.", true)]
    public abstract class RootOfTheProblem : Quest
    {
        public override string QuestName => "Root of the Problem";
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "The Adventurer's gone missing! Apparently, a few of my friends heard news that he was captured in the Briar while protecting some scientists. You just killed those Hookbats, right? I'm sure you've got this- be safe!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.GladeWraithMask>(), 1),
			(ModContent.ItemType<Items.Sets.FloranSet.FloranOre>(), 15),
			(Terraria.ID.ItemID.HealingPotion, 5),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private RootOfTheProblem()
        {
			_tasks.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.Adventurer>(), "I thought I was a real goner there! If you didn't butt in, I probably would've been fed to whatever those monsters were trying to conjure up over there. I wouldn't touch it if I were you... Look, you have my thanks; but just between you and me, it's been a long few months, and all I want is a vacation from adventuring for a while. Life is short, and I'd rather not make it shorter. I'll see you around sometime. Could you get rid of that altar for me, too?", "Go to the Underground Briar and rescue the Adventurer"));
		}

		public override void OnQuestComplete()
		{
			bool showUnlocks = true;

			QuestManager.UnlockQuest<ReturnToYourRoots>(showUnlocks);
			QuestManager.UnlockQuest<IdleIdol>(showUnlocks);
			QuestManager.UnlockQuest<BareNecessities>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestBriar>(showUnlocks);

			base.OnQuestComplete();
		}
    }
}
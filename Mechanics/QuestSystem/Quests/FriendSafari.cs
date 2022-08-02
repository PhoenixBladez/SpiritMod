using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class FriendSafari : Quest
    {
        public override string QuestName => "Friend Safari";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A few years ago, I was running a job with a friend of mine who's never really been on the good side of the law. He's a bandit, and a mighty fine one at that. We've kept in touch ever since, but he's been awfully silent as of late. Last I remember, he was holed up with a bandit group near the far shores of the world. Would you mind checkin' up on him?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.GamblerChestLoot.SilverChest>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.TreasureChest>(), 2),
			(ModContent.ItemType<Items.Placeable.Furniture.PottedWillow>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private FriendSafari()
        {
            _tasks.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.Rogue>(), "Hey! Thanks for saving me- Now, mind getting us out of this pickle? They duped me, took all my cash and left me for dead here! Don't think it means I'll discount my wares for you, though. Just kidding! Not.", "Find the bandit hideout and rescue the prisoner."));
        }

		public override bool IsQuestPossible() => MyWorld.gennedBandits;
	}
}
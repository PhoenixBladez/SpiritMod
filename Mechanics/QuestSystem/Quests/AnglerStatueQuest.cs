using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class AnglerStatueQuest : Quest
    {
        public override string QuestName => "Fishy Business";
		public override string QuestClient => "The Angler";
		public override string QuestDescription => "I'd like to think that I'm the best fisherman in the world. Actually, I know I am! But I've heard some pesky rumors about crates washing ashore that are filled with way more fish than I could catch. I need you to go to the beach and prove those rumors wrong. Why? Because I said so! Hop to it!";
		public override int Difficulty => 1;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)ItemID.CopperCoin, 1)
		};

		private AnglerStatueQuest()
        {
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new RetrievalTask(ItemID.RedSnapper, 3424, "Appease him by catching"))
			       .AddTask(new GiveNPCTask(NPCID.Angler, ItemID.RedSnapper, 3424, "How is this supposed to make me feel better? Are you trying to show me up, too?! You should just stick to running my errands and catching my fish. Thanks for nothing!", "Give the Red Snappers to the Angler", true, true));
		
			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new TalkNPCTask(NPCID.Guide, "The Angler's upset, huh? Well, he is just a kid- I suppose it makes sense for him to be hurt by something like this. This may be a bit, well, demanding, but I suppose you should check in with him to see if anything would make him feel better.", "Or ask the Guide what to do"))
				   .AddTask(new TalkNPCTask(NPCID.Angler, "*sniff* What do you want? You want to try to cheer me up? What's that? You'll do ANYTHING? What do ya mean, that's not what you said? Too bad! I want you to melt those disgusting Packing Crates down and build a giant, buff, half-shark, half-angler statue to show my dominance over those factory losers! You said you'd do it, so shoo!", "Talk to the Angler"))
				   .AddTask(new RetrievalTask(ModContent.ItemType<Items.Placeable.Furniture.GiantAnglerStatue>(), 1))
				   .AddTask(new GiveNPCTask(NPCID.Angler, ModContent.ItemType<Items.Placeable.Furniture.GiantAnglerStatue>(), 1, "This is the most amazing thing ever! Industrial fishing will never break my spirit, and I'm sure they don't have a half-man, half-shark statue like this one! You've impressed me today, so I'll actually give you a reward. Make sure to put that statue up pronto! I'm going to ask the Merchant to sell copies, so tell your friends (if you have any)!", "Show the Angler your creation", true, false, ItemID.FishermansGuide));

			TaskBuilder branch3 = new TaskBuilder();
			branch3.AddTask(new RetrievalTask(ModContent.ItemType<Items.Placeable.FishCrate>(), 3))
			      .AddTask(new GiveNPCTask(NPCID.Angler, ModContent.ItemType<Items.Placeable.FishCrate>(), 3, "WHAT?! Industrial... scale? Machine powered? Factory farming? I thought I was the best angler around! This sucks. *sniffle* Thanks for putting me in a rotten mood, loser. Leave me alone!", "Show the Packing Crates to the Angler", true, false))
				  .AddBranches(branch1, branch2);

			_tasks.AddBranches(branch3);
		}
	}
}
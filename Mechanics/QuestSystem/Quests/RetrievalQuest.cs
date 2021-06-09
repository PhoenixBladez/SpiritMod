using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	class RetrievalQuest : Quest
	{
		public override string QuestName => "Epic Testing Time!";
		public override string QuestClient => "Gabe";
		public override string QuestDescription => "You feel 30x more epic with this quest being active.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;

		private (int, int)[] _rewards = new[]
		{
			((int)ItemID.PlatinumCoin, 100),
			(ItemID.SilverCoin, 90)
		};

		public override bool IsQuestPossible() => true;

		private RetrievalQuest()
		{
			var branch1 = new TaskBuilder();
			branch1.AddTask(new GiveNPCTask(NPCID.Guide, new int[]{ ItemID.MudBlock, ItemID.StoneBlock, ItemID.LunarOre }, new int[] { 1, 10, 100 }, "finally i needed this", "Begin by giving the Guide 1 mud, 10 stone and 100 luminite...", true))
				.AddTask(new GiveNPCTask(NPCID.Merchant, new int[] { ItemID.DirtBlock }, new int[] { 1 }, "thanks i wanted that bud", "...and give the Merchant a dirt block.", true));

			var branch2 = new TaskBuilder();
			branch2.AddTask(new GiveNPCTask(NPCID.Dryad, new int[] { ItemID.PlatinumCoin, ItemID.SilverCoin }, new int[] { 33, 9 }, "yoo sick stuff thanks", "Or show the Dryad 33 platinum coins and 9 silver bullets...", true, false)).
				AddTask(new GiveNPCTask(NPCID.Guide, new int[] { ItemID.PlatinumOre, ItemID.CopperOre }, new int[] { 10, 10 }, "yoo sick stuff thanks", "...and then give the Guide 10 copper and 10 platinum ore.", true));

			_tasks.AddBranches(branch1, branch2);
		}
	}
}

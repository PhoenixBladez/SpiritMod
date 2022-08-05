using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestOcean : Quest
    {
        public override string QuestName => "Below the Waves";
		public override string QuestClient => "The Angler";
		public override string QuestDescription => "Do ya know why I love having you catch fish for me? I have the itch to see every darn fish on this planet before I kick the bucket. And I want you to feel that way, too! Take a dive and see how amazing the ocean can be. Hopefully it'll make you a better errand monkey, too!";
		public override int Difficulty => 1;
		public override string QuestCategory => "Explorer";
		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.CascadeSet.Coral_Catcher.Coral_Catcher>(), 1),
			(ModContent.ItemType<Items.Sets.FloatingItems.FishLure>(), 2),
			(ItemID.CopperCoin, 80)
		};

		public override void OnQuestComplete()
		{
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Angler, QuestManager.GetQuest<AnglerStatueQuest>());

			base.OnQuestComplete();
		}

		private ExplorerQuestOcean()
		{
			_tasks.AddTask(new ExploreTask((Player player) => player.ZoneBeach && player.GetModPlayer<MyPlayer>().Submerged(30), 5000f, "the ocean depths"))
			       .AddTask(new TalkNPCTask(NPCID.Angler, "It's beautiful, right? The best view in the whole world is down there. One day, when I'm rich, I'll build a giant underwater castle and order you around from there! Now scram! Don't you have some of my fish to catch?", "Return to the Angler"));
		}
	}
}
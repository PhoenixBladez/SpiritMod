using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;

using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ZombieOriginQuest : Quest
    {
        public override string QuestName => "Unholy Undertaking";
		public override string QuestClient => "Unknown";
		public override string QuestDescription => "I've found an odd wall scroll listing important information about Zombies and, potentially, their origins. I think I should talk to the Guide or Dryad about this.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;

		private (int, int)[] _rewards = new[]
		{
			(ItemID.SkyMill, 1),
			(ItemType<Items.Consumable.ChaosPearl>(), 25),
			(ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(ItemID.SilverCoin, 90)
		};

		public override bool IsQuestPossible() => Main.player[Main.myPlayer].HasItem(ModContent.ItemType<Items.Placeable.Furniture.OccultistMap>());

		private ZombieOriginQuest()
        {
            TaskBuilder branch1 = new TaskBuilder();
            branch1.AddTask(new TalkNPCTask(NPCID.Guide, "Talk to the Guide about the mysterious scroll."))
            .AddTask(new RetrievalTask(516, 3));

            TaskBuilder branch2 = new TaskBuilder();
            branch2.AddTask(new TalkNPCTask(NPCID.Dryad, "Or talk to the Dryad about the mysterious scroll."))
                .AddTask(new RetrievalTask(201, 5));

            _tasks.AddBranches(branch1, branch2);

        }
    }
}
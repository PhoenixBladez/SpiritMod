using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestMushroom : Quest
    {
        public override string QuestName => "Glowing a Garden";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'm looking for info on any nearby Glowing Mushroom Caverns. You see, I made a bet with the merchant that they DO exist an' that I don't hallucinate giant blue glowing mushroom trees. On second thought, I may not sound too sane sayin' that, but I swear they're real! Go prove me right!";
		public override int Difficulty => 2;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Pins.PinBlue>(), 1),
			(Terraria.ID.ItemID.MushroomGrassSeeds, 5),
			(Terraria.ID.ItemID.GlowingMushroom, 10),
			(ModContent.ItemType<Items.Consumable.Quest.ExplorerScrollMushroomFull>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private ExplorerQuestMushroom()
        {
            _tasks.AddTask(new ExploreTask((Player player) => player.ZoneGlowshroom, 5000f, "glowing mushroom fields"));
        }
   }
}
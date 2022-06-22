using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class AncestralWorship : Quest
    {
        public override string QuestName => "Ancestral Worship";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "When I was explorin' the Briar with Professor Laywatts and her team, I remember her talkin' about some massive statues the old Briar dwellers used to construct. They've all been destroyed over time, but maybe we could create a replica right here?";
		public override int Difficulty => 1;
		public override string QuestCategory => "Designer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_BriarArt>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.BriarFlowerItem>(), 2),
			(ModContent.ItemType<Items.Placeable.Tiles.BriarGrassSeeds>(), 5),
			(Terraria.ID.ItemID.SilverCoin, 40)
		};

		private AncestralWorship()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Placeable.Furniture.Reach.TreemanStatue>(), 1, "Craft"));
        }
    }
}
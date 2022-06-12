using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestBlueMoon : Quest
    {
        public override string QuestName => "Once in a...";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Just when you'd think this world's run out of things to kill you with, eh? We've all seen those disgusting Blood Moons, but recently, the moon's taken to turning a deep blue. I bet you may think that sounds sweet, but terrifyin' creatures come out during these Mystic Moons. Make sure none of 'em get too close to the town, you hear?";
		public override int Difficulty => 4;
		public override string QuestCategory => "Explorer";
		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.Potion.MoonJellyDonut>(), 5),
			(ModContent.ItemType<Items.Sets.SeraphSet.MoonStone>(), 6),
			(ModContent.ItemType<Items.Placeable.MusicBox.BlueMoonBox>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};


		private ExplorerQuestBlueMoon()
        {
			_tasks.AddTask(new ExploreTask((Player player) => player.ZoneOverworldHeight && MyWorld.BlueMoon, 5000f, "a Mystic Moon"));
        }
    }
}
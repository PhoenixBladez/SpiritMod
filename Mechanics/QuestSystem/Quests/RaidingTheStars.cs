using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class RaidingTheStars : Quest
    {
        public override string QuestName => "Raiding the Stars";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You've checked out the Asteroid Fields near the far corner of the world, right? My sources have reported some increasin' mechanical activity around there." +
				" Some kinda weird automatonic worms made of metal are streakin' through the sky there. Something has to be putting 'em on edge. Could you kill a couple and see what makes 'em tick?";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.StarplatePainting>(), 1),
			(ModContent.ItemType<Items.Sets.CoilSet.TechDrive>(), 7),
			(ModContent.ItemType<Items.Placeable.Tiles.ScrapItem>(), 50),
			(Terraria.ID.ItemID.GoldCoin, 4)
		};

		private RaidingTheStars()
        {
			_tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Starfarer.CogTrapperHead>(), 2))
				.AddTask(new RetrievalTask(ModContent.ItemType<Items.Material.StarEnergy>(), 1, "Craft"));
        }
    }
}
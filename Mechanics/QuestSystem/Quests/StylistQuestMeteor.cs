using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	[System.Obsolete("Modded hair dyes do not work and I don't know a solution, removed until further notice")]
	public class StylistQuestMeteor : Quest
    {
        public override string QuestName => "Dye Pursuit: Tektites";
		public override string QuestClient => "The Stylist";
		public override string QuestDescription => "Hey, you! So get this- I was talking to the Adventurer, who was talking to the Guide, who was... you know how it goes around here! We were discussing that spooky meteor that crashed around these parts recently. I just know that I could make some brand new hair dye from some of those monster parts. Fashion must be innovated! And it's on you and me to make that happen.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.MeteorDye>(), 1),
			(ModContent.ItemType<Items.Sets.CoilSet.TechDrive>(), 5),
			(ModContent.ItemType<Items.Ammo.FaerieStar>(), 75),
			(ItemID.SilverCoin, 45)
		};

		private StylistQuestMeteor()
        {
            _tasks.AddParallelTasks(new SlayTask(new int[] { ModContent.NPCType<NPCs.AstralAdventurer.AstralAdventurer>(), ModContent.NPCType<NPCs.FallingAsteroid.Falling_Asteroid>(), ModContent.NPCType<NPCs.MoltenCore.Molten_Core>()}, 5), new RetrievalTask(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>(), 1, "Harvest"))
				  .AddTask(new GiveNPCTask(NPCID.Stylist, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>(), 1, "Welcome back! Looks like you need a hair day right about now, all your ends are burnt! I'm glad you got back safe, and I can't wait to change hair chic as we know it with this new hair dye I'm whipping up. Ouch, that's hot. Remember, your next makeover day is on me, 'kay?", "Bring the Photosphere Shard back to the Stylist", true, true));
		}
	}
}
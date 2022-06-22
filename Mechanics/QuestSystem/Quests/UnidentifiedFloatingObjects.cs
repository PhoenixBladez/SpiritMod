using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	public class UnidentifiedFloatingObjects : Quest
    {
        public override string QuestName => "Unidentified Floaters";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I quit drinkin' years ago, but I swear the sky's been lighting up something fierce recently! I've been doin' some research and I think the skies may be home to some mystical jellyfish swarms. Now, the only 'proof' I have are some sources of, er, ill repute, but I know I can count on you to check it out! And capture me the tastiest- I mean most interesting one!";
		public override int Difficulty => 2;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.DistressJellyItem>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private UnidentifiedFloatingObjects()
        {
            _tasks.AddTask(new ExploreTask((Player player) => (player.ZoneOverworldHeight || player.ZoneSkyHeight) && MyWorld.jellySky, 500f, "the strange Jelly Deluge"))
				.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.DreamlightJellyItem>(), 1, "Catch"));
        }
    }
}
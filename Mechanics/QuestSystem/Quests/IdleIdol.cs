using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	public class IdleIdol : Quest
    {
        public override string QuestName => "Idle Idol";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "The sands of the desert hide a lot of secrets beneath 'em. There's supposed to be an Ancient Ziggurat buried near the surface of one of those wastelands. Could ya head down there and scavenge some relics from me? ";
		public override int Difficulty => 2;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.ScarabIdol>(), 1),
			(Terraria.ID.ItemID.Topaz, 2),
			(Terraria.ID.ItemID.Sapphire, 2),
			(Terraria.ID.ItemID.PharaohsMask, 1),
			(Terraria.ID.ItemID.PharaohsRobe, 1),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		public IdleIdol()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.ScarabIdolQuest>(), 1))
				  .AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.Town.Adventurer>(), new int[] {ModContent.ItemType<Items.Consumable.Quest.ScarabIdolQuest>()}, new int[] { 1 }, "I knew it. I was polishin' up this old thing when it started to look real familiar. That's a Scarab Idol right there. I'm warning ya, don't mess with it until you get real strong. Me and some bounty hunters tried to take that thing on years ago. We barely escaped with our lives. Be safe, lad.", "Return to the Adventurer with what you've found", true));
        }

        public override void OnQuestComplete()
		{
			QuestManager.UnlockQuest<SinisterSands>(true);

			base.OnQuestComplete();
		}
    }
}
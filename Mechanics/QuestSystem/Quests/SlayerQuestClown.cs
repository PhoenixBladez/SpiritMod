using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestClown : Quest
    {
        public override string QuestName => "No Laughing Matter";
		public override string QuestClient => "The Party Girl";
		public override string QuestDescription => "I was planning a party last week when these DISGUSTING clowns showed up and cramped my style. They blew up my entire venue, so now it's personal. I want you to find those clowns and end them. There can only be one partyholic around these parts.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		//public override bool AnnounceRelocking => true;
		//public override bool LimitedUnlock => true;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)ItemID.Bananarang, 5),
			(ItemID.LightShard, 1),
			(ItemID.GoldCoin, 3)
		};

		private SlayerQuestClown()
        {
            _tasks.AddTask(new SlayTask(NPCID.Clown, 3, null, new QuestPoolData(0.2f, true, true, SpawnConditions)));
        }

		private bool SpawnConditions(NPCSpawnInfo arg) => Main.bloodMoon && arg.spawnTileY < Main.worldSurface;

		public override bool IsQuestPossible() => Main.hardMode;
	}
}
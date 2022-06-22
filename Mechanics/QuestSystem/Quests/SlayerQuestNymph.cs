using SpiritMod.Mechanics.QuestSystem.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestNymph : Quest
    {
        public override string QuestName => "She's a Maniac";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Y'know, after some time resting after being stranded in the Briar, I was really excited to return to the datin' game. Had a nice date lined up and everything. The lady was super pretty an' nice. But when I got to the cave we were supposed to meet in, she tried to eat me! Pesky monster- kill her to give me some closure!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(Terraria.ID.ItemID.NypmhBanner, 1),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private SlayerQuestNymph()
        {
            _tasks.AddTask(new SlayTask(Terraria.ID.NPCID.Nymph, 1, null, new QuestPoolData(0.1f, true, true, SpawnConditions)));
        }

		private bool SpawnConditions(NPCSpawnInfo conditions) => conditions.spawnTileY > Main.rockLayer && conditions.spawnTileY < Main.maxTilesY - 200;
	}
}
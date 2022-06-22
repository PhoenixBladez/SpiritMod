using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestMarble : Quest
    {
        public override string QuestName => "Ancient Gazes";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You've really shaken up the world after slaying the great evil in that disgusting biome. New horrors are startin' to crop up everwhere. The Marble Caverns have seen quite a stir especially. This new monstrosity's got tentacles, eyes, fireballs, you name it. Can you go kill this monstrosity for me, lad?";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.BeholderMask>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.MarblePillars.Pillar1>(), 5),
			(ModContent.ItemType<Items.Sets.MarbleSet.MarbleChunk>(), 15),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private SlayerQuestMarble()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Beholder.Beholder>(), 1, null, new QuestPoolData(0.2f, true, false, null)));
        }
    }
}
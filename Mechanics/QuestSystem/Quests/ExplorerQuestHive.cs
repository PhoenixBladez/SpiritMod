using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestHive : Quest
    {
        public override string QuestName => "Hive Hunting";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Have you checked out the lower parts of the Jungle? I've recently heard about a series of massive hives around there. I loathe bees... an' hornets... an' giant man eatin' plants, so would ya like to check one of these hives out for me?";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.BeekeeperSet.BeekeeperHead>(), 1),
			(ModContent.ItemType<Items.Armor.BeekeeperSet.BeekeeperBody>(), 1),
			(ModContent.ItemType<Items.Armor.BeekeeperSet.BeekeeperLegs>(), 1),
			(ModContent.ItemType<Items.Consumable.Quest.ExplorerScrollHiveFull>(), 1),
			(Terraria.ID.ItemID.BottledHoney, 10),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		public ExplorerQuestHive()
        {
            _questTasks.Add(new ExploreTask((Player player) => player.GetModPlayer<MyPlayer>().ZoneHive, 2000f, "giant beehives in the Jungle"));
        }
    }
}
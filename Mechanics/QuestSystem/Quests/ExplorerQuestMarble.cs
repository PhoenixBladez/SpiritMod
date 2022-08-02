﻿using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestMarble : Quest
    {
        public override string QuestName => "Forgotten Civilizations";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'd like ya to head out there and map out the lower reaches of this land. A few caverns are covered in marble and the ruins of some ancient civilization. After you stumble upon one of these Marble Caverns, wander around for a while and take some notes for me, alright? Hopefully, those ruins are desolate, eh?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.CenturionSet.CenturionHead>(), 1),
			(ModContent.ItemType<Items.Armor.CenturionSet.CenturionBody>(), 1),
			(ModContent.ItemType<Items.Armor.CenturionSet.CenturionLegs>(), 1),
			(ModContent.ItemType<Items.Consumable.Quest.ExplorerScrollMarbleFull>(), 1),
			(ModContent.ItemType<Items.Placeable.MusicBox.MarbleBox>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private ExplorerQuestMarble()
        {
            _tasks.AddTask(new ExploreTask((Player player) => player.GetModPlayer<MyPlayer>().ZoneMarble, 5000f, "marble caverns"));
        }
    }
}
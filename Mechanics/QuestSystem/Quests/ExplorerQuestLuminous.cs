﻿using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestLuminous : Quest
    {
        public override string QuestName => "High Tide, Glow Tide";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I love the beach. Now, you may be picturin' the sun and the sand, and I love that stuff too! But I'm talkin' about the beach at night. If you're lucky, you can see some beautiful glowing algae wash up on the shore and turn the water into pretty colors! I'd really recommend kicking back and relaxing at a Luminous Ocean.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_LuminousArt>(), 1),
			(ModContent.ItemType<Items.Placeable.MusicBox.LuminousNightBox>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.SilverCoin, 75)
		};

		private ExplorerQuestLuminous()
        {
             _tasks.AddTask(new ExploreTask((Player player) => player.ZoneBeach && MyWorld.luminousOcean, 1500f, "a Luminous Ocean at the beach"));
        }
    }
}
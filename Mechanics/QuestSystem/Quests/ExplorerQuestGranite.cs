using SpiritMod.Mechanics.QuestSystem.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestGranite : Quest
    {
        public override string QuestName => "Rocky Road";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A couple of underground cave systems seem to be made almost entirely of dark granite. Some kinda energy source seems to be bringin' the rocks to life, too. I'd like ya to go and investigate. After you stumble upon one of these Granite Caverns, wander around for a while and take some notes for me, alright?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.CapacitorSet.CapacitorHead>(), 1),
			(ModContent.ItemType<Items.Armor.CapacitorSet.CapacitorBody>(), 1),
			(ModContent.ItemType<Items.Armor.CapacitorSet.CapacitorLegs>(), 1),
			(ModContent.ItemType<Items.Consumable.Quest.ExplorerScrollGraniteFull>(), 1),
			(ModContent.ItemType<Items.Placeable.MusicBox.GraniteBox>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private ExplorerQuestGranite()
        {
            _tasks.AddTask(new ExploreTask((Player player) => player.GetModPlayer<MyPlayer>().ZoneGranite, 5000f, "granite caverns"));
        }
    }
}
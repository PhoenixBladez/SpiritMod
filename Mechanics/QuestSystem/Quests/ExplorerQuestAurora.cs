using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestAurora : Quest
    {
        public override string QuestName => "Lights in the Sky";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I don't really have a particular motive for this job except for sharin' the beauty of this world with more people. Have you seen a Boreal Aurora before? It might be the most magical thing in the world, and this world's actually got magic! You can find them at high altitudes and in the snowy tundra. Just go there and take in the sights.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(Terraria.ID.ItemID.WeatherRadio, 1),
			(ModContent.ItemType<Items.Placeable.Furniture.Paintings.AdvPainting15>(), 1),
			(ModContent.ItemType<Items.Placeable.MusicBox.AuroraBox>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.SilverCoin, 55)
		};

		private ExplorerQuestAurora()
        {
            _tasks.AddTask(new ExploreTask((Player player) => (player.ZoneSnow || player.ZoneSkyHeight) && MyWorld.aurora, 1500f, "boreal auroras in the snowy tundra or at high altitudes"));
        }
    }
}
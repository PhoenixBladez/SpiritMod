using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestAsteroid : Quest
    {
        public override string QuestName => "Space Rocks";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "There's an asteroid field smack-dab above one of the oceans. Completely uncharted. How cool is that?! We have to learn everything we can about that place. If you stumble upon those asteroids, wander around for a while and take some notes for me, alright? Can you even write in space? Go find out, and don't fall off!";
		public override int Difficulty => 2;
        public override QuestType QuestType =>  QuestType.Explorer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Pins.PinRed>(), 1),
			(ModContent.ItemType<Items.Placeable.JumpPadItem>(), 2),
			(ModContent.ItemType<Items.Consumable.Quest.ExplorerScrollAsteroidFull>(), 1),
			(ModContent.ItemType<Items.Placeable.MusicBox.AsteroidBox>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		public ExplorerQuestAsteroid()
        {
            _questTasks.Add(new ExploreTask((Player player) => player.GetModPlayer<MyPlayer>().ZoneAsteroid, 5000f, "the Asteroid Field"));
        }
    }
}
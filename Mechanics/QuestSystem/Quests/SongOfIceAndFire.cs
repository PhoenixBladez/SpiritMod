using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SongOfIceAndFire : Quest
    {
        public override string QuestName => "Song of Ice and Fire";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I'm sure the monsters around the world haven't taken to kindly to you shakin' things up. It's time to gear up and get stronger. I've noticed two new materials that could be useful to you. Unfortunately, one of 'em is found only in the pits of the Underworld, while the other is found in the depths of the tundra. Are you up to the challenge?";
		public override int Difficulty => 3;
        public override QuestType QuestType =>  QuestType.Forager | QuestType.Main;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.PlumbersHat, 1),
			(Terraria.ID.ItemID.PlumbersShirt, 1),
			(Terraria.ID.ItemID.PlumbersPants, 1),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};


        public SongOfIceAndFire()
        {
            _questSections.Add(new ConcurrentTask(new RetrievalTask(ModContent.ItemType<Items.Material.CryoliteOre>(), 15), new RetrievalTask(ModContent.ItemType<Items.Material.CarvedRock>(), 10)));
        }
    }
}
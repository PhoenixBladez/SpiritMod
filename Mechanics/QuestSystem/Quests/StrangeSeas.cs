using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class StrangeSeas : Quest
    {
        public override string QuestName => "Strange Seas";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "The mythical Seabreak Pearl... I've been hearin' rumors about it popping up recently. It's one of the rarest treasures out there, but it's almost like it has a mind of its own and wants to be found. I feel like somethin' deeper and more sinister is at work here. We should find the pearl an' get to the bottom of this mystery as soon as we can.";
		public override int Difficulty => 4;
        public override QuestType QuestType =>  QuestType.Main;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.DiverSet.DiverHead>(), 1),
			(ModContent.ItemType<Items.Armor.DiverSet.DiverBody>(), 1),
			(ModContent.ItemType<Items.Armor.DiverSet.DiverLegs>(), 1),
			(ModContent.ItemType<Items.Material.TribalScale>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 6)
		};
        public StrangeSeas()
        {
            _questTasks.Add(new RetrievalTask(ModContent.ItemType<Items.Consumable.BlackPearl>(), 1));
            _questTasks.Add(new SlayTask(ModContent.NPCType<NPCs.Tides.Rylheian>(), 1, "the monster controlling the Tide"));
        }
    }
}
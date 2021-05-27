using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ExplorerQuestBlueMoon : Quest
    {
        public override string QuestName => "Once in a...";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Just when you'd think this world's run out of things to kill you with, eh? We've all seen those disgusting Blood Moons, but recently, the moon's taken to turning a deep blue. I bet you may think that sounds weet, but terrifyin' creatures come out during these Mystic Moons. Make sure none of 'em get too close to the town, you hear?";
		public override int Difficulty => 4;
		public override string QuestCategory => "Explorer";

		public ExplorerQuestBlueMoon()
        {
			_tasks.AddParallelTasks(new SlayTask(10, 10), new SlayTask(15, 10), new SlayTask(20, 10));
        }
    }
}
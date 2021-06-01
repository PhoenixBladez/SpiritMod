using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class BlastFromThePast : Quest
    {
        public override string QuestName => "Blast From The Past";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I see you've got some Discharge Tubules there, lad. I'm glad you've taken my design advice, so let's take it one step further. Try grabbin' some Enchanted Marble Chunks to really set the scene with a Hyperspace Bust. I'm tellin' ya, it'll look spectacular!";
		public override int Difficulty => 1;
		public override string QuestCategory => "Designer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.Neon.Synthpalm>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.Neon.VaporwaveItem>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.Neon.ArcadeMachineItem>(), 2),
			(Terraria.ID.ItemID.SilverCoin, 75)
		};

		private BlastFromThePast()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Placeable.Furniture.SynthwaveHeadItem>(), 1, "Craft"));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class CritterCaptureBlossmoon : Quest
    {
        public override string QuestName => "Sanctuary: Peaceful Blossoms";
		public override string QuestClient => "The Dryad";
		public override string QuestDescription => "I have heard tales of a rare species of fauna that only emerges during the calmest of nights. When the moon is in its first or third quarter, and the leaves blow in the wind, little Blossmoons come out of their flowers and soak up the moonlight. They are quite defenseless on their own, though. We must catch and preserve one.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_Blossmoon>(), 1),
			(ModContent.ItemType<Tiles.Furniture.Critters.BlossomCage>(), 1),
			(ItemID.CalmingPotion, 3),
			(Terraria.ID.ItemID.SilverCoin, 55)
		};
        public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			base.OnDeactivate();
		}
		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool[ModContent.NPCType<NPCs.Critters.Blossmoon>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Critters.Blossmoon>()))
			{
				pool[ModContent.NPCType<NPCs.Critters.Blossmoon>()] = .25f;
			}
		}
		private CritterCaptureBlossmoon()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.BlossmoonItem>(), 2, "Capture"))
				  .AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.BlossmoonItem>(), 2, "Blossmoon actually have the ability to calm nearby monsters. I suspect they draw power from the moon to do so. It would be very beneficial for us all if we made sure all Blossmoon thrived. This is a good first step in that process!", "Bring the Blossmoon back to the Dryad", true, true));

		}
	}
}
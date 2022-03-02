using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class CritterCaptureFloater : Quest
    {
        public override string QuestName => "Sanctuary: Nightlights";
		public override string QuestClient => "The Dryad";
		public override string QuestDescription => "I have detected traces of another species in need of our protection. While most jellyfish in this world can protect themselves, these smaller ones cannot. They often travel in packs to appear larger than they are. Let us retrieve one of these Luminous Floaters so that we can preserve them. You can find them on the ocean floor at nighttime.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.FloatingItems.SunkenTreasure>(), 1),
			(ItemID.JellyfishNecklace, 1),
			(ItemID.SonarPotion, 3),
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
			if (pool[ModContent.NPCType<NPCs.Critters.Floater>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Critters.Floater>()))
			{
				pool[ModContent.NPCType<NPCs.Critters.Floater>()] = .25f;
			}
		}
		private CritterCaptureFloater()
        {
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.FloaterItem>(), 1, "Capture"))
				  .AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.FloaterItem>(), 1, "These marvels of nature light up the ocean with their glow. I certainly would not want to see them perish by the hands of sea monsters and greedy fishermen. They will look beautiful in my arcane nature preserve. Thank you again.", "Bring the Luminous Floater back to the Dryad", true, true));

			TaskBuilder branch3 = new TaskBuilder();
			branch3.AddParallelTasks(
				new SlayTask(new int[] { ModContent.NPCType<NPCs.Critters.Floater>() }, 2),
				new RetrievalTask(ModContent.ItemType<Items.Consumable.Fish.RawFish>(), 1, "Harvest"))
				.AddTask(new GiveNPCTask(NPCID.Angler, ModContent.ItemType<Items.Consumable.Fish.RawFish>(), 1, "This looks absolutely scrumptions! And they're all mine- oh, you're still here? Fine, I guess I'll thank you this once, but don't expect me to always be this nice!", "Return the raw fish to the Angler", true, true, ModContent.ItemType<Items.Consumable.Fish.CrystalFish>()));

			TaskBuilder branch4 = new TaskBuilder();
			branch4.AddTask(new TalkNPCTask(NPCID.Dryad, "That rotten, spoiled child! I have let him off the hook time and time again for fishing up rare creatures, but now he has gone too far. These species are meant to be admired, not gobbled up! Thank you for telling me about his nasty plan- your commitment to the natural world is admirable. Take this as thanks! Don't forget those Luminous Floaters, now!", "Or tell the Dryad about the Angler's plans", null, ItemID.PureWaterFountain))
				   .AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.FloaterItem>(), 1, "Capture"))
				   .AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.FloaterItem>(), 1, "These marvels of nature light up the ocean with their glow. I certainly would not want to see them perish by the hands of sea monsters and greedy fishermen. They will look beautiful in my arcane nature preserve. Thank you again.", "Bring the Luminous Floater back to the Dryad", true, true));


			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new TalkNPCTask(NPCID.Angler, "That high and mighty Dryad always thinks she knows what's best for the fish. Well, I don't care! What's best for me is those jellyfish in my tummy, stat! Don't let 'em go to waste- chop 'em up for me and I'll make us an a-MAZ-ing seafood dish. What're you waiting for?", "Or ask the Angler about Luminous Floaters"))
				   .AddBranches(branch3, branch4);
								

			_tasks.AddBranches(branch1, branch2);
		}
	}
}
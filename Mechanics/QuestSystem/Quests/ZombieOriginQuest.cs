using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Weapon.Swung.Punching_Bag;

using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ZombieOriginQuest : Quest
    {
        public override string QuestName => "Unholy Undertaking";
		public override string QuestClient => "Unknown";
		public override string QuestDescription => "I've found an odd wall scroll listing important information about Zombies and, potentially, their origins. I think I should talk to the Guide or Dryad about this. Maybe we can put a stop to this vile necromancy!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;

		private (int, int)[] _rewards = new[]
		{
			(ItemType<Punching_Bag>(), 1),
			(ItemID.HerbBag, 3),
			(ItemType<Items.Material.OldLeather>(), 8),
			(ItemID.GoldCoin, 2)
		};

		public override bool IsQuestPossible() => Main.player[Main.myPlayer].HasItem(ModContent.ItemType<Items.Placeable.Furniture.OccultistMap>());

		int itemType;
		int lureType;
		private ZombieOriginQuest()
        {
			if (WorldGen.crimson)
			{
				itemType = ItemID.ViciousPowder;
				lureType = ModContent.ItemType<WarlockLureCrimson>();
			}
			else
			{
				itemType = ItemID.VilePowder;
				lureType = ModContent.ItemType<WarlockLureCorruption>();
			}
            TaskBuilder branch1 = new TaskBuilder();
            branch1.AddTask(new TalkNPCTask(NPCID.Guide, "Did you find that mysterious scroll there? I assume you did. It's an interesting lead on why these zombies have been appearing recently. I'd say we need to do more research on who's behind the zombie uprising. Maybe it's a crazed scientist, or something? There should be books on the matter near the Dungeon.", "Talk to the Guide about the mysterious scroll."))
            	.AddTask(new RetrievalTask(149, 3))
            	.AddTask(new GiveNPCTask(NPCID.Guide, 149, 3, "This research is ambiguous, but I think it points toward a maniacal scientist creating hordes of zombies. You may need to find a way to get their attention tonight. How about we stitch together a lure using some grisly zombie bits? I'm sure the researcher would find it interesting.", "Return the books to the Guide"))
               	.AddTask(new RetrievalTask(ModContent.ItemType<ScientistLure>(), 1, "Craft"))
				.AddTask(new SlayTask(ModContent.NPCType<NPCs.Dead_Scientist.Dead_Scientist>(), 1, "Undead Scientist", 0.75f)); 

            TaskBuilder branch2 = new TaskBuilder();
            branch2.AddTask(new TalkNPCTask(NPCID.Dryad, "This scroll is alarming. You found it on the remains of a zombie, no? Poor, wretched things. I think this scroll could help us understand why zombies have been plaguing our land. This faint powder on the scroll smells just like the evil that plagues our world. We should collect some herbs from that biome and compare.", "Or talk to the Dryad about the mysterious scroll."))
           		.AddTask(new RetrievalTask(itemType, 5))
                .AddTask(new GiveNPCTask(NPCID.Dryad, itemType, 5, "I was right! I now think some kind of evil wizard may have a hand in creating these vile monstrosities. We need to get this horrible necromancer's attention tonight. I think that putrid powder you collected can help craft a lure for it. Dispatch them quickly, and perhaps we will be rid of zombies for good!", "Return the powder to the Dryad"))
               	.AddTask(new RetrievalTask(lureType, 1, "Craft"))
				.AddTask(new SlayTask(ModContent.NPCType<NPCs.Undead_Warlock.Undead_Warlock>(), 1, "Undead Warlock", 0.75f)); 
            _tasks.AddBranches(branch1, branch2);

        }
	}
}
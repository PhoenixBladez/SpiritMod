using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;

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
			(ItemID.SkyMill, 1),
			(ItemType<Items.Consumable.ChaosPearl>(), 25),
			(ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(ItemID.SilverCoin, 90)
		};

		public override bool IsQuestPossible() => Main.player[Main.myPlayer].HasItem(ModContent.ItemType<Items.Placeable.Furniture.OccultistMap>());

		int itemType;
		private ZombieOriginQuest()
        {
			if (WorldGen.crimson)
			{
				itemType = ItemID.ViciousPowder;
			}
			else
			{
				itemType = ItemID.VilePowder;
			}
            TaskBuilder branch1 = new TaskBuilder();
            branch1.AddTask(new TalkNPCTask(NPCID.Guide, "Did you find that mysterious scroll there? I assume you did. It's an interesting lead on why these zombies have been appearing recently. I'd say we need to do more research on who's behind zombies. Maybe it's a crazed scientist, or something? There should be books on the matter near the Dungeon.", "Talk to the Guide about the mysterious scroll."))
            .AddTask(new RetrievalTask(149, 3))
            .AddTask(new GiveNPCTask(NPCID.Guide, new int[] { 149 }, new int[] { 3 }, "This research is ambiguous, but I think it points toward a maniacal scientist creating hordes of zombies. You may need to find a way to get their attention. How about we stitch together a lure using some grisly zombie bits? I'm sure the researcher would find it interesting.", "Return the books to the Guide"))
			.AddTask(new SlayTaskWithSpawnIncrease(ModContent.NPCType<NPCs.Dead_Scientist.Dead_Scientist>(), 1)); 

            TaskBuilder branch2 = new TaskBuilder();
            branch2.AddTask(new TalkNPCTask(NPCID.Dryad, "This scroll is alarming. You found it on the remains of a zombie, no? Poor, wretched things. I think this scroll could help us understand why zombies have been plaguing our land. This faint powder on the scroll smells just like the evil that plagues our world. We should collect some herbs from that biome and compare.", "Or talk to the Dryad about the mysterious scroll."))
           		.AddTask(new RetrievalTask(itemType, 5))
                .AddTask(new GiveNPCTask(NPCID.Dryad, new int[] { itemType }, new int[] { 5 }, "How interesting! I now think some kind of evil wizard may have a hand in creating these vile monstrosities. We need to get this horrible necromancer's attention, somehow. I think that putrid powder you collected can help craft a lure for it. Dispatch them quickly, and perhaps we will be rid of zombies for good!", "Return the powder to the Dryad"))
				.AddTask(new SlayTaskWithSpawnIncrease(ModContent.NPCType<NPCs.Undead_Warlock.Undead_Warlock>(), 1)); 
            _tasks.AddBranches(branch1, branch2);

        }
    }
}
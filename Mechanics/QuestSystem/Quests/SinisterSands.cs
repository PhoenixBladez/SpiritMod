﻿using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SinisterSands : Quest
    {
        public override string QuestName => "Sinister Sands";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I knew it. I was polishin' up this relic you retreived  when it started to look real familiar. " +
				"That's a Scarab Idol right there. I'm warning ya, don't mess with it until you get real strong. " +
				"Me and some bounty hunters tried to take that thing on years ago. We barely escaped with our lives. Be safe, lad.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Equipment.SandsOfTime>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 5)
		};

		private SinisterSands()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Boss.Scarabeus.Scarabeus>(), 1, "Kill Scarabeus"));        
        }
    }
}
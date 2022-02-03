﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class AuroraStagQuest : Quest
    {
        public override string QuestName => "Taming the Stars";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Hey there, lad! The world's gotten a lot more dangerous recently, but it's not all bad. My scouts have been hearin' about a big, beautiful creature that reveals itself under the light of the Aurora. It's a massive stag, an' I think you could tame it if you found the right food for it! It's a bit skittish, though, so try not to startle the beast.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Other";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			((int)ItemID.Holly, 1),
			(ItemID.HandWarmer, 1),
			(ItemID.ChristmasTree, 1),
			(ItemID.PineTreeBlock, 120),
			(Terraria.ID.ItemID.GoldCoin, 3)
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
			if (pool[ModContent.NPCType<NPCs.AuroraStag.AuroraStag>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.AuroraStag.AuroraStag>()))
			{
				pool[ModContent.NPCType<NPCs.AuroraStag.AuroraStag>()] = .05f;
			}
		}
		private AuroraStagQuest()
        {
			_tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Food.IceBerries>(), 1, "Retrieve"))
				  .AddTask(new RetrievalTask(ModContent.ItemType<Items.Equipment.AuroraSaddle.AuroraSaddle>(), 1, "Retrieve", "Slowly approach an Aurora Stag and feed it Ice Berries"));
		}
	}
}
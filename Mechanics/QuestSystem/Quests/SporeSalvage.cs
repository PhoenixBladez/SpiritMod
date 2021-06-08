﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SporeSalvage : Quest
    {
        public override string QuestName => "Spore Salvage";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've been hearin' stories about some new flora that's cropped up around those strange Mushroom Forests recently. These lil' buggers seem to just sway from side to side, as if they're dancin'. I have no real motive this time around, I just wanna see one of 'em. Mind fetching one for me?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Tiles.Furniture.Critters.VibeshroomJarItem>(), 1),
			(ModContent.ItemType<Items.Material.GlowRoot>(), 3),
			(Terraria.ID.ItemID.GlowingMushroom, 16),
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
			if (pool[ModContent.NPCType<NPCs.Critters.Vibeshroom>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Critters.Vibeshroom>()))
			{
				pool[ModContent.NPCType<NPCs.Critters.Vibeshroom>()] = 1.15f;
			}
		}
		private SporeSalvage()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.VibeshroomItem>(), 1, "Capture"));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestMarble : Quest
    {
        public override string QuestName => "Ancient Gazes";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You've really shaken up the world after slaying the great evil in that disgusting biome. New horrors are startin' to crop up everwhere. The Marble Caverns have seen quite a stir especially. This new monstrosity's got tentacles, eyes, fireballs, you name it. Can you go kill this monstrosity for me, lad?";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.BeholderMask>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.MarblePillars.Pillar1>(), 5),
			(ModContent.ItemType<Items.Material.MarbleChunk>(), 15),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};
        public SlayerQuestMarble()
        {
            _questSections.Add(new SlayTask(ModContent.NPCType<NPCs.Beholder.Beholder>(), 1));

        }
		
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
			if (pool[ModContent.NPCType<NPCs.Beholder.Beholder>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Beholder.Beholder>()))
			{
				pool[ModContent.NPCType<NPCs.Beholder.Beholder>()] = 0.15f;
			}
		}
    }
}
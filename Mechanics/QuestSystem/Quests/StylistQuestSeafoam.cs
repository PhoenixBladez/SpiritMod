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
    public class StylistQuestSeafoam : Quest
    {
        public override string QuestName => "Dye Pursuit: Seafoam";
		public override string QuestClient => "The Stylist";
		public override string QuestDescription => "If it isn't my favorite customer! I've got a job for you, and it isn't cutting hair. I need some rare materials to expand my dye collection ASAP! I love the Dye Trader's style, but his dyes don't cut it for me. Could you start by hunting Green Jellyfish for their ink sacs? You're the best!";
		public override int Difficulty => 2;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.SeafoamDye>(), 1),
			(ModContent.ItemType<Items.Material.HeartScale>(), 4),
			(ItemID.SilverCoin, 30)
		};

		private int EnemyID => ModContent.NPCType<NPCs.Horned_Crustacean.Horned_Crustacean>();

		public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
			base.OnDeactivate();
		}

		public override void OnQuestComplete()
		{
			bool showUnlocks = true;
			QuestManager.UnlockQuest<StylistQuestCorrupt>(showUnlocks);
			QuestManager.UnlockQuest<StylistQuestCrimson>(showUnlocks);
			base.OnQuestComplete();
		}

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == EnemyID)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.SeaMandrakeSac>());
		}

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool[EnemyID] > 0f && !NPC.AnyNPCs(EnemyID))
				pool[EnemyID] = 0.15f;
		}

		private StylistQuestSeafoam()
        {
			_tasks.AddParallelTasks(new SlayTask(EnemyID, 1), new RetrievalTask(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.SeaMandrakeSac>(), 1, "Harvest"))
				  .AddTask(new GiveNPCTask(NPCID.Stylist, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.SeaMandrakeSac>(), 1, "You're back in one piece! I'm glad you didn't dye trying to get the goods. Get it? Anyway, this ink sac is really pretty- I'll be able to synthesize a hair dye that brings out the ocean breeze in you! Am I great or what? Come to me anytime you'd like to try this new dye or anything else to glam you up!", "Bring the Ink Sac back to the Stylist", true, true));
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ManicMage : Quest
    {
        public override string QuestName => "The Manic Mage";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Scouts at the far ends of the world have reported that a witch seems to be terrorizin' the area. Apparently, it's some type of harpy with a real dangerous staff. Mission's real simple this time. Bring me its head! Er... I promise I'm not unhinged. I mean, bring me its hat! Yeah.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Weapon.Magic.AkaviriStaff>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		private ManicMage()
        {
			_tasks.AddParallelTasks(
					new SlayTask(ModContent.NPCType<NPCs.DarkfeatherMage.DarkfeatherMage>(), 1), 
					new RetrievalTask(ModContent.ItemType<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>(), 1));
        }
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

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool.ContainsKey(ModContent.NPCType<NPCs.DarkfeatherMage.DarkfeatherMage>()))
			{
				pool[ModContent.NPCType<NPCs.DarkfeatherMage.DarkfeatherMage>()] = 0.75f;
			}
		}

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Hookbat.Hookbat>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>());
			}
		}
    }
}
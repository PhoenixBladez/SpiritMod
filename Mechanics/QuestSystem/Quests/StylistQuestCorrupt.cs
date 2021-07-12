using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiritMod.Mechanics.QuestSystem.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class StylistQuestCorrupt : Quest
    {
        public override string QuestName => "Dye Pursuit: Putrefaction";
		public override string QuestClient => "The Stylist";
		public override string QuestDescription => "Hiya, hun! I'm itching to make some new hair dyes to sell to the townsfolk- especially the Guide, he's sooo plain. I'll need your help, though, because I want a pretty nasty bit from the Corruption and I've had enough of creepy crawlies for a last time. Could you pop there and get me what I need?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.CystalDye>(), 1),
			(ItemID.WrathPotion, 3),
			(Terraria.ID.ItemID.SilverCoin, 45)
		};
		public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
			base.OnActivate();
		}
		public override bool IsQuestPossible() => !WorldGen.crimson;

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
			base.OnDeactivate();
		}
		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Cystal.Cystal>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>());
			}
		}
		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool[ModContent.NPCType<NPCs.Cystal.Cystal>()] > 0f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Cystal.Cystal>()))
			{
				pool[ModContent.NPCType<NPCs.Cystal.Cystal>()] = 0.185f;
			}
		}
		private StylistQuestCorrupt()
        {
			_tasks.AddParallelTasks(new SlayTask(ModContent.NPCType<NPCs.Cystal.Cystal>(), 1), new RetrievalTask(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "Harvest"));
			
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new GiveNPCTask(NPCID.Stylist, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "Wow, you smell pretty terrible. Take a shower, then come back to me for a shampoo on the house. I insist! But thank you for doing this for me, hun. You've been working really hard, so I wanted to give you this exclusive hair dye I've creates. You'll never find it anywhere else! This dye and the one you've helped create work best with white hair, so let me know if you want a pre-dye.", "Bring the Violet Crystal back to the Stylist", true, true, ModContent.ItemType<Items.Sets.DyesMisc.HairDye.ViciousDye>()));
			
			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new GiveNPCTask(NPCID.Merchant, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "What's this? This crystal glimmers with some strange energy. I'm sure it could fetch a high price on the right market. Thank you for showing this to me! Oh, the Stylist? I'm sure she won't mind. Besides, I can make it worth your wild with this curio in exchange, hmm?", "Or bring the Violet Crystal to the Merchant", true, true, ItemID.StrangePlant1))
			       .AddTask(new TalkNPCTask(NPCID.Stylist, "Did you manage to get that gross thing for me? What's that? Oh... I see. It's totally fine, hun. It's your choice, after all. I was going to thank you by giving you a new, rare hair dye, but I'll hold onto it for now. I'll just buy my materials back from the Merchant. My new dye is best used with white hair, so keep that in mind.", "Explain what happened to the Stylist"));

			_tasks.AddBranches(branch1, branch2);
		}
	}
}
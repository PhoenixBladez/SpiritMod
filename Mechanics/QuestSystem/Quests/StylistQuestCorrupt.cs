using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
	[System.Obsolete("Modded hair dyes do not work and I don't know a solution, removed until further notice")]
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
			(ItemID.SilverCoin, 45)
		};

		private StylistQuestCorrupt()
		{
			_tasks.AddParallelTasks(new SlayTask(ModContent.NPCType<NPCs.Cystal.Cystal>(), 1, null, new QuestPoolData(0.2f, true)), new RetrievalTask(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "Harvest"));

			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new GiveNPCTask(NPCID.Stylist, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "Wow, you smell pretty terrible. Take a shower, then come back to me for a shampoo on the house. I insist! But thank you for doing this for me, hun. You've been working really hard, so I wanted to give you this exclusive hair dye I've created. You'll never find it anywhere else! This dye and the one you've helped create work best with white hair, so let me know if you want a pre-dye.", "Bring the Violet Crystal back to the Stylist", true, true, ModContent.ItemType<Items.Sets.DyesMisc.HairDye.ViciousDye>()));

			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new GiveNPCTask(NPCID.Merchant, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>(), 1, "What's this? This crystal glimmers with some strange energy. I'm sure it could fetch a high price on the right market. Thank you for showing this to me! Oh, the Stylist? I'm sure she won't mind. Besides, I can make it worth your wild with this curio in exchange, hmm?", "Or bring the Violet Crystal to the Merchant", true, true, ItemID.StrangePlant1))
				   .AddTask(new TalkNPCTask(NPCID.Stylist, "Did you manage to get that gross thing for me? What's that? Oh... I see. It's totally fine, hun. It's your choice, after all. I was going to thank you by giving you a new, rare hair dye, but I'll hold onto it for now. I'll just buy my materials back from the Merchant. My new dye is best used with white hair, so keep that in mind.", "Explain what happened to the Stylist"));

			_tasks.AddBranches(branch1, branch2);
		}

		public override bool IsQuestPossible() => !WorldGen.crimson;
	}
}
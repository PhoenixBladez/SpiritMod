using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class LumothQuest : Quest
    {
        public override string QuestName => "Sanctuary: Luminous Luster";
		public override string QuestClient => "The Dryad";
		public override string QuestDescription => "As protectors of nature, we Dryads have an obligation to protect all flora and fauna- to 'look out for the little guy,' as you may put it. This applies especially to those creatures that cannot defend themselves. A prime example is the brilliant Lumoth. It shines in the darkness of caves. Bring it to me so we can preserve the species for future generations.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_Lumoth>(), 1),
			(Terraria.ID.ItemID.ShinePotion, 3),
			(Terraria.ID.ItemID.SilverCoin, 35)
		};
		public override void OnQuestComplete()
		{
			bool showUnlocks = true;

			QuestManager.UnlockQuest<CritterCaptureBlossmoon>(showUnlocks);
			QuestManager.UnlockQuest<CritterCaptureFloater>(showUnlocks);
			QuestManager.UnlockQuest<SporeSalvage>(showUnlocks);


			base.OnQuestComplete();

		}
		private LumothQuest()
        {
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.LumothItem>(), 1, "Capture"))
				.AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.LumothItem>(), 1, "Look at how beautiful this creature is. You truly are a kind soul for ensuring that such beauty will thrive for centuries to come. I will work my magic to start a little sanctuary for this Lumoth and its bretheren. Thank you again.", "Bring the Lumoth back to the Dryad", true, true));

			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new TalkNPCTask(NPCID.Merchant, "Hear me out, friend. Don't get me wrong, I appreciate what the Dryad is doing for the environment, but this presents a wonderful business opportunity, if you're interested. Lumoths are rare, and I'm too feeble to procure them. If you were to, say, kill a Lumoth and bring me its Brightbulb instead of bringing it to the Dryad, I'd give you something special.", "Or talk to the Merchant about Lumoths"))
				.AddTask(new RetrievalTask(ModContent.ItemType<Items.Material.Brightbulb>(), 1, "Harvest"))
				.AddTask(new GiveNPCTask(NPCID.Merchant, ModContent.ItemType<Items.Material.Brightbulb>(), 1, "I'm sure exterminating just one Lumoth has no large-scale consequences, right? Er, just in case, don't tell the Dryad about this. We aren't monsters, of course- we're just meeting the needs of the economy. And as promised, here's your extra reward. Thanks, friend!", "Bring the Brightbulb back to the Merchant", true, true, ItemID.IronCrate));

			_tasks.AddBranches(branch1, branch2);
		}
	}
}
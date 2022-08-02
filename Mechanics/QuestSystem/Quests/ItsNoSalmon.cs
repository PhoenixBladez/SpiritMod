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
    public class ItsNoSalmon : Quest
    {
        public override string QuestName => "It's No Salmon...";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've got a, uh, perfectly normal quest for ya. Why don't you go ahead and head to the Jungle to fish up a Hornetfish for me? It's supposed to be a real delicacy. Be careful, though. I've heard it can be a... tough catch. Whaddya mean, this sounds exactly like something the Angler would want you to do?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Accessory.KoiTotem>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.FishingPainting>(), 1),
			(Terraria.ID.ItemID.Vine, 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private ItsNoSalmon()
        {
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.Town.Adventurer>(), ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>(), 1, "Thanks a lot, lad. This is exactly what I've been cravin' as of late. I'm something of a chef, ya know. In fact, let me share this hornetfish with you! Thanks again. ", "Bring the Hornetfish back to the Adventurer", true, true, ModContent.ItemType<Items.Consumable.Fish.HoneySalmon>()));
			
			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new GiveNPCTask(NPCID.Angler, ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>(), 1, "What?! You were going to catch fish without ME? I'm offended, and I demand your Hornetfish as compensation. Here, take this bait, and use it to catch me fish next time. The Adventurer can eat something else. Shoo!", "Or give the Angler the Hornetfish", true, true, ItemID.MasterBait));

			_tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>(), 1))
				  .AddBranches(branch1, branch2);
		}
	}
}
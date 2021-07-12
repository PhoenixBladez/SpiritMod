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
    public class TravelingMerchantDesertQuest : Quest
    {
        public override string QuestName => "Forsaken Relics";
		public override string QuestClient => "The Traveling Merchant";
		public override string QuestDescription => "";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_BriarArt>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.BriarFlowerItem>(), 2),
			(ModContent.ItemType<Items.Placeable.Tiles.BriarGrassSeeds>(), 5),
			(Terraria.ID.ItemID.SilverCoin, 40)
		};

		private TravelingMerchantDesertQuest()
        {
			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>(), ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>(), 1, "I can never repay your kindness, but I can start by setting my life right. This crown will support our family for years, and I will try to turn a new leaf. I have no more need for this enchanted ring, so I would like to give it to you. It is nowhere close to equal payment for what you have done for me today, and I will never forget you", "Or spare the Forsaken Bandit and give him the Royal Crown", true, true, ModContent.ItemType<Items.Sets.AccessoriesMisc.DustboundRing.Dustbound_Ring>()));

			TaskBuilder branch3 = new TaskBuilder();
			branch3.AddTask(new SlayTask(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>(), 1, "the last Forsaken Bandit"))
				   .AddTask(new GiveNPCTask(NPCID.Merchant, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>(), 1, "I can never repay your kindness, but I can start by setting my life right. This crown will support our family for years, and I will try to turn a new leaf. I have no more need for this enchanted ring, so I would like to give it to you. It is nowhere close to equal payment for what you have done for me today, and I will never forget you", "Return the Royal Crown to the Merchant", true, true));

			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.BloodstainedChest.BloodstainedChest>(), "The ancient chest seems to be covered in gold, blood, and bones. It surely contains great riches, but opening it may be perilous.", "Find the strange chest in the Desert and retrieve the Royal Crown", .85f));
			branch1.AddTask(new SlayTask(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>(), 3));
			branch1.AddBranches(branch3, branch2);
			
			_tasks.AddBranches(branch1);		
		}
	}
}
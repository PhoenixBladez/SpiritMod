using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;


namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class TravelingMerchantDesertQuest : Quest
    {
		public const string ThankText = "I cannot thank you enough. Take this, hopefully it'll be some penance for what me and...my old friends...have done to you. Goodbye.";

		public override string QuestName => "Forsaken Relics";
		public override string QuestClient => "The Travelling Merchant";
		public override string QuestDescription => "I've got a deal for you! The world loves me for my ever-changing supply of quality wares, but I need to restock! I've heard that there's an ancient relic stashed away in a mysterious chest over in the desert. If you get close to the chest, you should be able to see it on your minimap. And I'll make sure to give you a hefty reward for your valuable effort.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Accessory.DesertSlab>(), 1),
			(ItemID.Fez, 1),
			(ItemID.DynastyWood, 150),
			(ItemID.GoldCoin, 1)
		};

		private TravelingMerchantDesertQuest()
        {
			var newBranch = new TaskBuilder();
			newBranch.AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>(), ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>(), 1, ThankText, "Spare the last bandit and give them the crown"))
				.AddTask(new TalkNPCTask(NPCID.TravellingMerchant, "So you chose to give the artifact to a bandit in need? You have a kind heart, that's for sure! I'm not sure I could have done the same. No matter! I'll go looking for rare curios elsewhere.", "Explain what happened to the Travelling Merchant"));

			var newBranch2 = new TaskBuilder();
			newBranch2.AddTask(new SlayTask(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>(), 4, "Or kill every one of them"))
				.AddTask(new GiveNPCTask(NPCID.TravellingMerchant, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>(), 1, "I didn't think you'd make it back alive, wow! I'm sure the folks on Xandar will love this! Thanks for helping me out, and here's your reward as promised. If you ever need a little bit of anything, give me a shout!", "Return the Royal Crown to the Travelling Merchant", true, true, ModContent.ItemType<Items.Placeable.Furniture.HourglassItem>()));

			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.BloodstainedChest.BloodstainedChest>(), "The ancient chest seems to be covered in gold, blood, and bones. It surely contains great riches, but opening it may be perilous.", "Find the strange chest in the Desert and retrieve the Royal Crown", new QuestPoolData(0.85f, true)))
			       .AddBranches(newBranch, newBranch2);
			
			_tasks.AddBranches(branch1);		
		}
	}
}
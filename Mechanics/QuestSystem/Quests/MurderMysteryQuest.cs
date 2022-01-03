/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria;
using Terraria.ID ;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class MurderMysteryQuest : Quest
    {
        public override string QuestName => "Once in a Lifetime";
		public override string QuestClient => "The Arms Dealer";
		public override string QuestDescription => "I recognize skill when I see it. And it's here. The legendary Magnus Mustafa Thrax III is looking for a town to settle in. He's the best weapons master around- outside of guns, of course. Having him in your town would be the best thing to happen to you. He's got weapons made of Titanium, Shroomite, whatever. He's visiting town tonight- I think you should meet him.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Other";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.TreasureChest>(), 2),
			(ModContent.ItemType<Items.Placeable.Furniture.PottedWillow>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private MurderMysteryQuest()
        {
            TaskBuilder talkNPCBranch = new TaskBuilder();
            TaskBuilder branch1 = new TaskBuilder();
            branch1.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.WeaponMasterHead>(), 1, "", "Approach Magnus, the Weapons Master"))
                   .AddTask(new GiveNPCTask(NPCID.ArmsDealer, ModContent.ItemType<Items.Consumable.Quest.WeaponMasterHead>(), 1, "Holy shit he's dead", "Explain what happened to the Arms Dealer", true, false))
                   .AddTask(new GiveNPCTask(NPCID.Nurse, ModContent.ItemType<Items.Consumable.Quest.WeaponMasterHead>(), 1, "Holy shit he's dead", "Provide Magnus's head to the Nurse for an autopsy"));
            _tasks.AddBranches(branch1);
        }
    }
}*/
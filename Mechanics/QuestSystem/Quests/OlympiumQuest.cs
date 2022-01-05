using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;
using SpiritMod.Items.Sets.MaterialsMisc.QuestItems;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class OlympiumQuest : Quest
    {
        public override string QuestName => "Ancient Augury";
		public override string QuestClient => "The Dryad";
		public override string QuestDescription => "The world has changed for the worse. I sense foul magic growing stronger and stronger. If we must keep the forces of evil at bay, we must seek help. As of late, I've also sensed the presence of someone familiar in the depths of the Marble Caverns. Go to her; she will be able to offer you guidance.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{          
			
			(ModContent.ItemType<Items.Sets.OlympiumSet.OlympiumToken>(), 5),
			(ModContent.ItemType<Items.Consumable.OliveBranch>(), 2),
			(ItemID.GoldCoin, 3)
		};

		private OlympiumQuest()
        {
			TaskBuilder path = new TaskBuilder();
			path.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.Oracle.Oracle>(), "I've been expecting you, traveler. My visions spoke of a time when a hero would need some guidance. I can offer you powerful armaments in exchange for Olympium, the currency of the Gods. To find some, seek out and defeat enemies that have been blessed by the Gods themselves. I can give you my blessing if you would like to find more of these creatures, but you risk incurring the wrath of the Gods.'", "Find the mysterious woman in the Marble Caverns"))
				   .AddTask(new RetrievalTask(ModContent.ItemType<Items.Sets.OlympiumSet.OlympiumToken>(), 20))
				   .AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.Town.Oracle.Oracle>(), ModContent.ItemType<Items.Sets.OlympiumSet.OlympiumToken>(), 20, "Ah, you've returned safe and sound! Wonderful. I'll exchange these tokens with you for some powerful weapons and equipment. And take this sacred parchment. If you need me, I will be by your side. Safe journey, hero.", "Return to the Oracle", true, false, ModContent.ItemType<NPCs.Town.Oracle.OracleScripture>()));

			_tasks.AddBranches(path);
		}
	}
}
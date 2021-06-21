using SpiritMod.NPCs.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class BreakingAndEntering : Quest
    {
        public override string QuestName => "Breaking and Entering";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "A couple of my scout friends spotted something troubling near the far shores of the world. They caught a glimpse of a bound woman up there. Would you mind headin' there and checking things out? Maybe she needs rescuing! If you're not feeling too altruistic, I'm sure there's plenty of loot for the taking, too!";
		public override int Difficulty => 2;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Sets.GamblerChestLoot.SilverChest>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.TreasureChest>(), 2),
			(ModContent.ItemType<Items.Placeable.Furniture.PottedSakura>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private BreakingAndEntering()
        {
            _tasks.AddTask(new TalkNPCTask(ModContent.NPCType<Gambler>(), "Must be my lucky day to see a friendly face around here!\nThese goblins didn't take too kindly to me offering a, uh, rigged deal.\nAnyway, d'you have a place to stay? Let's flip for it.","Find the Arcane Goblin Tower and rescue the prisoner."))
				.AddTask(new RetrievalTask(ModContent.ItemType<Items.Weapon.Magic.ShadowflameStoneStaff>(), 1));
		}

		public override bool IsQuestPossible()
		{
			return MyWorld.gennedTower;
		}
	}
}
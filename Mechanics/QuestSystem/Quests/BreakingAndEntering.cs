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
        public override QuestType QuestType => QuestType.Main;

        public BreakingAndEntering()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
        public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.GamblerChests.SilverChest>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.TreasureChest>(), 2),
			(ModContent.ItemType<Items.Placeable.Furniture.PottedSakura>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};
    }
}
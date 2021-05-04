using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class DecrepitDepths : Quest
    {
        public override string QuestName => "Decrepit Depths";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You ever wonder why there're so many skeletons underground? Turns out that there was a band of necromancers that holed up in the caverns all across the world and performed all kinds of experiments. Well, lucky for us they're gone! But their Sepulchres still remain. Mind scopin' the place out for me? Don't turn into a skeleton!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Forager;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.SepulchrePotItem1>(), 4),
			(ModContent.ItemType<Items.Placeable.Furniture.SepulchrePotItem1>(), 4),
			(ModContent.ItemType<Items.Placeable.Tiles.SepulchreBrickTwoItem>(), 50),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		public DecrepitDepths()
        {
            _questSections.Add(new RetrievalSection(ModContent.ItemType<Items.Placeable.Furniture.SepulchreChest>(), 1));
        }
    }
}
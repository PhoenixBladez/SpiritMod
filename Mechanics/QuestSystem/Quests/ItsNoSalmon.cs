using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
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
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Accessory.KoiTotem>(), 1),
			(Terraria.ID.ItemID.FishermansGuide, 1),
			(ModContent.ItemType<Items.Placeable.Furniture.FishingPainting>(), 1),
			(Terraria.ID.ItemID.Vine, 3),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		public ItsNoSalmon()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>(), 1));
        }
    }
}
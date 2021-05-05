using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestOccultist : Quest
    {
        public override string QuestName => "Spectral Scourge";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "As if these freaky Blood Moons weren't enough, a new necromancer has taken control over a horde of zombies! The freakshow keeps summonin' zombies and is sure to overrun our town if we don't do anythin'. If you see one, take it out immediately, you hear?";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.OccultistMap>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.SkullPile>(), 3),
			(ModContent.ItemType<Items.Material.BloodFire>(), 8),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

        public SlayerQuestOccultist()
        {
            _questSections.Add(new SlayTask(ModContent.NPCType<NPCs.Occultist.Occultist>(), 1));
        }
    }
}
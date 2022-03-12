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
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.OccultistMap>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.SkullPile>(), 3),
			(ModContent.ItemType<Items.Sets.BloodcourtSet.DreamstrideEssence>(), 8),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(Terraria.ID.ItemID.GoldCoin, 2)
		};

		private SlayerQuestOccultist()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Boss.Occultist.OccultistBoss>(), 1, null, null, true));
        }
    }
}
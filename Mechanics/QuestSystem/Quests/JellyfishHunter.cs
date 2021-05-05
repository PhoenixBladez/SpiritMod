using SpiritMod.NPCs.Boss.MoonWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class JellyfishHunter : Quest
    {
		public override string QuestName => "Jellyfish Hunter";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Hey there, lad! I was doing some diggin' into what makes these cute jellies tick, and I think they're all part of some kinda hivemind that feed on mystical energy. In fact, that one jellyfish you caught seems to be emitting a distress signal to its buddies. You may need to take on a mighty strong jellyfish soon, so stay prepared!";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Main | QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.Potion.MoonJelly>(), 8),
			(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 50),
			(Terraria.ID.ItemID.GoldCoin, 4)
		};

		public JellyfishHunter()
        {
            _questSections.Add(new SlayTask(ModContent.NPCType<MoonWizard>(), 1));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestCavern : Quest
    {
        public override string QuestName => "Creepy Crawlies";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Some new creepy crawlies have taken to calling the caverns their home. Disgustin' little fellas that belch poison gas and some spiny little buggers. Do us all a favor and exterminate those nasty things. Killing a dozen of 'em will surely make the underground a less nasty place.";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.ClatterboneArmor.ClatterboneFaceplate>(), 1),
			(ModContent.ItemType<Items.Armor.ClatterboneArmor.ClatterboneBreastplate>(), 1),
			(ModContent.ItemType<Items.Armor.ClatterboneArmor.ClatterboneLeggings>(), 1),
			(ModContent.ItemType<Items.Weapon.Thrown.ClatterSpear>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.WheezerPainting>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

        public SlayerQuestCavern()
        {
            _questSections.Add(new SlayTask(new int[] { ModContent.NPCType<NPCs.Wheezer.Wheezer>(), ModContent.NPCType<NPCs.CavernCrawler.CavernCrawler>(), Terraria.ID.NPCID.GiantShelly, Terraria.ID.NPCID.Salamander, Terraria.ID.NPCID.Crawdad}, 8, "underground monsters"));

        }
    }
}
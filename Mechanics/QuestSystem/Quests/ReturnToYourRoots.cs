using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class ReturnToYourRoots : Quest
    {
        public override string QuestName => "A Return to Your Roots";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Did you know that I was part of a research team that tried to survey the Briar? It was led by some scientist- Laywatts, I think. She learned about some interestin' stuff down there. Apparently, all the roots in the Briar connect to one central... thing. That monster is a menace to everyone, so could you deal with it once an' for all, lad?";
		public override int Difficulty => 4;
        public override QuestType QuestType =>  QuestType.Slayer | QuestType.Main;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.ReachBoss.ReachBossHead>(), 1),
			(ModContent.ItemType<Items.Armor.ReachBoss.ReachBossBody>(), 1),
			(ModContent.ItemType<Items.Armor.ReachBoss.ReachBossLegs>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.ReachPainting>(), 1),
			(Terraria.ID.ItemID.GoldCoin, 7)
		};

		public ReturnToYourRoots()
        {
            _questSections.Add(new SlayTask(ModContent.NPCType<NPCs.Boss.ReachBoss.ReachBoss1>(), 1, "Vinewrath Bane"));        
        }
    }
}
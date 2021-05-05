using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestValkyrie : Quest
    {
        public override string QuestName => "Fight of the Valkyries";
		public override float QuestTitleScale => 0.7f;
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You ever been high enough where those infuriatin' harpies can shoot at ya? Well, to make things worse, I've heard tales of a harpy clad in armor and weapons! I'm sure you can handle it. We've taken to calling it a Valkyrie, but don't go joinin' the afterlife when you take it on! ";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(Terraria.ID.ItemID.SkyMill, 1),
			(ModContent.ItemType<Items.Consumable.ChaosPearl>(), 25),
			(Terraria.ID.ItemID.SilverCoin, 90)
		};

        public SlayerQuestValkyrie()
        {
            _questSections.Add(new KillSection(ModContent.NPCType<NPCs.Valkyrie.Valkyrie>(), 1));
        }
    }
}
using SpiritMod.NPCs.Boss.MoonWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class JellyfishHunter : Quest
    {
		public override string QuestName => "Jellyfish Hunter";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Mmm gotta catch me that mmm jellyfish";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Main | QuestType.Slayer;

        public JellyfishHunter()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(ModContent.NPCType<MoonWizard>(), 1)));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class StylishSetup : Quest
    {
        public override string QuestName => "Stylish Setup";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "D'you ever feel like spicin' things up, lad? I've got the perfect new aesthetic for you- retrofuturism! What, d'you think that isn't my style? I dabble in everythin', lad! Trust me, your town will look fabulous with some '80s funk mixed in.";
		public override int Difficulty => 1;
        public override QuestType QuestType => QuestType.Designer;

        public StylishSetup()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(10, 10), new KillSection(15, 10), new KillSection(20, 10)));
        }
    }
}
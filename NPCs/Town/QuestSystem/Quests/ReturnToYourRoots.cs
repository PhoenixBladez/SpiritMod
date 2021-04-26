using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town.QuestSystem.Quests
{
    public class ReturnToYourRoots : Quest
    {
        public override string QuestName => "A Return to Your Roots";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "The Briar's a strange, strange place. Did you know that I was part of a research team that tried to survey that place? It was led by some scientist- Laywatts, I think. She learned about some real interestin' stuff down there. Apparently, all the roots in the Briar connect to one central... thing. I have no doubts that monster is a menace to everyone, so could you deal with it once an' for all, lad?";
		public override int Difficulty => 4;
        public override QuestType QuestType =>  QuestType.Slayer | QuestType.Main;

        public ReturnToYourRoots()
        {
            _questSections.Add(new KillSection(ModContent.NPCType<NPCs.Boss.ReachBoss.ReachBoss>(), 1));        
        }
    }
}
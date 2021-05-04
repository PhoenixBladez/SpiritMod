using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class RootOfTheProblem : Quest
    {
        public override string QuestName => "Root of the Problem";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "Ever since I was captured by those savages from the Briar, I've been doin' some research on the place. That altar you found me at is supposed to harbor a really venegeful nature spirit. Mind investigating? ";
		public override int Difficulty => 3;
        public override QuestType QuestType => QuestType.Slayer | QuestType.Main;

        public override void OnQuestComplete()
		{
			bool showUnlocks = true;
			QuestManager.UnlockQuest<ReturnToYourRoots>(showUnlocks);
			base.OnQuestComplete();
		}
        public RootOfTheProblem()
        {
            _questSections.Add(new ConcurrentSection(new KillSection(new int[] { ModContent.NPCType<NPCs.Reach.ForestWraith>()}, 1, "Glade Wraith"), new RetrievalSection(ModContent.ItemType<Items.Consumable.Quest.SacredVine>(), 1)));
        }
        public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.GladeWriathMask>(), 1),    
			(ModContent.ItemType<Items.Material.FloranOre>(), 15),
			(Terraria.ID.ItemID.HealingPotion, 5)
			(Terraria.ID.ItemID.GoldCoin, 1)
		};
		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Reach.ForestWraith>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Quest.SacredVine>());
			}
		}
    }
}
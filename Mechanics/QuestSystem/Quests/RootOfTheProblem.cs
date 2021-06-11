using Microsoft.Xna.Framework;
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
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "The Adventurer's gone missing! Apparently, a few of my friends heard news that he was captured in the Briar while protecting some scientists. You just killed those Hookbats, right? I'm sure you've got this- be safe!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.GladeWraithMask>(), 1),
			(ModContent.ItemType<Items.Material.FloranOre>(), 15),
			(Terraria.ID.ItemID.HealingPotion, 5),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private RootOfTheProblem()
        {
			_tasks.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.Adventurer>(), "I thought I was a real goner there! If you didn't butt in, I probably would've been fed to whatever those monsters were trying to conjure up over there. I wouldn't touch it if I were you... Look, you have my thanks; but just between you and me, it's been a long few months, and all I want is a vacation from adventuring for a while. Life is short, and I'd rather not make it shorter. I'll see you around sometime. Could you get rid of that altar for me, too?", "Go to the Underground Briar and rescue the Adventurer"))
				  .AddTask(new SlayTask(ModContent.NPCType<NPCs.Reach.ForestWraith>(), 1, "Glade Wraith")); 
		}

		public override void OnQuestComplete()
		{
			bool showUnlocks = true;
			QuestManager.UnlockQuest<ReturnToYourRoots>(showUnlocks);
			QuestManager.UnlockQuest<IdleIdol>(showUnlocks);

			QuestManager.UnlockQuest<BareNecessities>(showUnlocks);
			
			QuestManager.UnlockQuest<SlayerQuestBriar>(showUnlocks);
		
			QuestManager.UnlockQuest<FriendSafari>(showUnlocks);
			QuestManager.UnlockQuest<BreakingAndEntering>(showUnlocks);

			QuestManager.SayInChat("Click on quests in the chat to open them in the book!", Color.White);

			base.OnQuestComplete();
		}

		public override void OnActivate()
		{
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
			base.OnDeactivate();
		}

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Reach.ForestWraith>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Quest.SacredVine>());
			}
		}
    }
}
using Microsoft.Xna.Framework;
using SpiritMod.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class FirstAdventure : Quest
    {
        public override string QuestName => "The First Adventure";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "So you wanna be an adventurer, eh? Well, pack a bag and get out there! I'd actually planned to craft you a set of special armor. Unfortunately, some mangy Hookbats stole the sheaf of Durasilk I was usin'! They only come out at night around the forest surface. Mind retrievin' that silk for me so I can thank you properly?";
		public override int Difficulty => 1;
		public override string QuestCategory => "Main";
		public override bool TutorialActivateButton => true;

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerHead>(), 1),
			(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerBody>(), 1),
			(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerLegs>(), 1),
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(Terraria.ID.ItemID.GoldCoin, 1)
		};

		private FirstAdventure()
        {
			_tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 1));
		}

		public override void OnQuestComplete()
		{
			// a lot of quests, so not showing their unlocks. Feel free to change that:
			bool showUnlocks = true;
			QuestManager.UnlockQuest<RootOfTheProblem>(showUnlocks);
			QuestManager.UnlockQuest<IdleIdol>(showUnlocks);

			QuestManager.UnlockQuest<BareNecessities>(showUnlocks);

			QuestManager.UnlockQuest<ExplorerQuestCrimson>(showUnlocks);
			QuestManager.UnlockQuest<ExplorerQuestCorrupt>(showUnlocks);

			QuestManager.UnlockQuest<HeartCrystalQuest>(showUnlocks);

			QuestManager.UnlockQuest<SlayerQuestScreechOwls>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestBriar>(showUnlocks);
		
			QuestManager.UnlockQuest<FriendSafari>(showUnlocks);
			QuestManager.UnlockQuest<BreakingAndEntering>(showUnlocks);

			base.OnQuestComplete();

			QuestManager.SayInChat("Click on quests in the chat to open them in the book!", Color.White);
		}

		public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot += QuestGlobalNPC_OnNPCLoot;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			QuestGlobalNPC.OnNPCLoot -= QuestGlobalNPC_OnNPCLoot;
			base.OnDeactivate();
		}

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (pool.ContainsKey(ModContent.NPCType<NPCs.Hookbat.Hookbat>()))
			{
				pool[ModContent.NPCType<NPCs.Hookbat.Hookbat>()] = 0.75f;
			}
		}

		private void QuestGlobalNPC_OnNPCLoot(NPC npc)
		{
			if (npc.type == ModContent.NPCType<NPCs.Hookbat.Hookbat>())
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>());
			}
		}
	}
}
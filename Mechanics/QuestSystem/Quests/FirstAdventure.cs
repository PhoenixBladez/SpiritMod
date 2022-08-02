using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.QuestSystem.Tasks;
using SpiritMod.NPCs.Town;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class FirstAdventure : Quest
    {
        public override string QuestName => "The First Adventure";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "So you wanna be an adventurer, eh? Well, pack a bag and get out there! I'd actually planned to craft you a set of special armor so that you could get started. Unfortunately, some mangy Hookbats stole the sheaf of Durasilk I was usin'! They only come out at night around the forest surface. Mind retrievin' that silk and crafting your own new armor set?";
		public override int Difficulty => 1;
		public override string QuestCategory => "Main";
		public override bool TutorialActivateButton => true;

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Consumable.MapScroll>(), 2),
			(ItemID.GoldCoin, 1)
		};

		private FirstAdventure()
        {
			_tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 3))
				.AddParallelTasks(new RetrievalTask(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerHead>(), 1, "Craft"), 
					new RetrievalTask(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerBody>(), 1, "Craft"),
					new RetrievalTask(ModContent.ItemType<Items.Armor.WayfarerSet.WayfarerLegs>(), 1, "Craft"));
		}

		public override void OnQuestComplete()
		{
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Demolitionist, QuestManager.GetQuest<RescueQuestStylist>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Dryad, QuestManager.GetQuest<LumothQuest>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.TravellingMerchant, QuestManager.GetQuest<TravelingMerchantDesertQuest>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Angler, QuestManager.GetQuest<ExplorerQuestOcean>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Guide, QuestManager.GetQuest<HeartCrystalQuest>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Guide, QuestManager.GetQuest<SlayerQuestScreechOwls>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<SlayerQuestBriar>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<IdleIdol>());
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<BareNecessities>());

			if (WorldGen.crimson)
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Guide, QuestManager.GetQuest<ExplorerQuestCrimson>());
			else
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Guide, QuestManager.GetQuest<ExplorerQuestCorrupt>());

			QuestManager.SayInChat("Your residents want to talk to you. Chat with them to get more quests!", Color.ForestGreen, true);
			QuestManager.SayInChat("Click on quests in the chat to open them in the book!", Color.GreenYellow, true);
			base.OnQuestComplete();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class RescueQuestStylist : Quest
    {
        public override string QuestName => "Wrapped Up";
		public override string QuestClient => "The Demolitionist";
		public override string QuestDescription => "I heard a lass screamin' down in the spider filled caves. I'd go save her myself but, err... I've got business to do. I'm definitely not scared of those giant creepy crawlies or anything. Might ye go rescue her for me?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Other";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Accessory.LongFuse>(), 1),
			(Terraria.ID.ItemID.ManaHairDye, 1),
			(Terraria.ID.ItemID.LifeHairDye, 1),			
			(Terraria.ID.ItemID.Dynamite, 5),
			(Terraria.ID.ItemID.SilverCoin, 75)
		};
		public override void OnQuestComplete()
		{
			ModContent.GetInstance<QuestWorld>().AddQuestQueue(NPCID.Stylist, QuestManager.GetQuest<StylistQuestSeafoam>());
			base.OnQuestComplete();
		}
		private RescueQuestStylist()
        {
            _tasks.AddTask(new TalkNPCTask(NPCID.Stylist, "Don't go exploring with scissors, they said. You won't get trapped in a spider's web, they said! Who got you to rescue me, by the way? Oh, the Demolitionist? Tell him haircuts are on the house for life!", "Go to the spider caverns and rescue the captive"));
        }
    }
}
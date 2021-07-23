using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestScreechOwls : Quest
    {
        public override string QuestName => "Cacophonous Cries";
		public override string QuestClient => "The Guide";
		public override string QuestDescription => "I'd like to think I'm an animal lover, y'know? I love dog, cats, and especially bunnies! Except for that horrifying beast... You guessed it, I'm talking about snowy Screech Owls. I swear, those things are terrifying! Every night, I hear their screeches echoing from the snowy tundra. It ruins my sleep! Could you get rid of some for me?";
		public override int Difficulty => 2;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Armor.Masks.WinterHat>(), 1),
			(Terraria.ID.ItemID.MusicBox, 1),
			(ModContent.ItemType<Items.Sets.FrigidSet.FrigidFragment>(), 5),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 25),
			(Terraria.ID.ItemID.Snowball, 50),
			(Terraria.ID.ItemID.SilverCoin, 25)
		};

		private SlayerQuestScreechOwls()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.ScreechOwl.ScreechOwl>(), 2));
        }

    	public override void OnQuestComplete()
		{
            bool showUnlocks = true;
            QuestManager.UnlockQuest<SlayerQuestValkyrie>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestDrBones>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestNymph>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestUGDesert>(showUnlocks);
			QuestManager.UnlockQuest<SlayerQuestCavern>(showUnlocks);

			base.OnQuestComplete();
        }
    }
}
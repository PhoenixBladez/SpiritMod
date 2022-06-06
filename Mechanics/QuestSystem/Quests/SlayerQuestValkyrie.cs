using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SlayerQuestValkyrie : Quest
    {
        public override string QuestName => "Fight of the Valkyries";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "You ever been high enough where those infuriatin' harpies can shoot at ya? Well, to make things worse, I've heard tales of a harpy clad in armor and weapons! I'm sure you can handle it. We've taken to calling it a Valkyrie, but don't go joinin' the afterlife when you take it on! ";
		public override int Difficulty => 3;
		public override string QuestCategory => "Slayer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(Terraria.ID.ItemID.SkyMill, 1),
			(ModContent.ItemType<Items.Consumable.ChaosPearl>(), 25),
			(ModContent.ItemType<Items.Weapon.Thrown.TargetBottle>(), 35),
			(Terraria.ID.ItemID.SilverCoin, 90)
		};

		private SlayerQuestValkyrie()
        {
            _tasks.AddTask(new SlayTask(ModContent.NPCType<NPCs.Valkyrie.Valkyrie>(), 1));
        }
        public override void OnActivate()
		{
			QuestGlobalNPC.OnEditSpawnPool += QuestGlobalNPC_OnEditSpawnPool;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			QuestGlobalNPC.OnEditSpawnPool -= QuestGlobalNPC_OnEditSpawnPool;
			base.OnDeactivate();
		}

		private void QuestGlobalNPC_OnEditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) => ModifySpawnRateUnique(pool, ModContent.NPCType<NPCs.Valkyrie.Valkyrie>(), 0.15f);
	}
}
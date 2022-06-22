using SpiritMod.Mechanics.QuestSystem.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class StylishSetup : Quest
    {
        public override string QuestName => "Stylish Setup";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "D'you ever feel like spicin' things up, lad? I've got the perfect new aesthetic for you- retrofuturism! What, d'you think that isn't my style? I dabble in everythin', lad! Trust me, your town will look fabulous with some '80s funk mixed in.";
		public override int Difficulty => 1;
		public override string QuestCategory => "Designer";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.Furniture.Neon.BlueNeonSign>(), 1),
			(ModContent.ItemType<Items.Placeable.Furniture.Neon.NeonPlantBlue>(), 1),
			(ModContent.ItemType<Items.Placeable.Tiles.NeonBlockBlueItem>(), 50),
			(Terraria.ID.ItemID.SilverCoin, 30)
		};

		private StylishSetup()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Material.SynthMaterial>(), 1, "Craft"));
        }

        public override void OnQuestComplete()
		{
            bool showUnlocks = true;
            QuestManager.UnlockQuest<BlastFromThePast>(showUnlocks);

            base.OnQuestComplete();
        }
    }
}
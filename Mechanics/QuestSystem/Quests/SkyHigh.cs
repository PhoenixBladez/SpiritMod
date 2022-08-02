using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class SkyHigh : Quest
    {
        public override string QuestName => "Sky High";
		public override string QuestClient => "The Adventurer";
		public override string QuestDescription => "I've been looking at some old maps and I've learned about a cluster of Floating Pagodas above the oceans of this world. Trouble is, I can't make out whether it's to the left or right, so would ya go explorin' for me? I'm looking for an ornate staff, hundreds of years old. Happy hunting!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			((int)Terraria.ID.ItemID.GypsyRobe, 1),
			(Terraria.ID.ItemID.DynastyWood, 50),
			(Terraria.ID.ItemID.GoldCoin, 1),
			(ModContent.ItemType<Tiles.Furniture.JadeDragonStatue.DragonStatueItem>(), 1)
		};

		private SkyHigh()
        {
            _tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Weapon.Summon.JadeStaff>(), 1));
        }
    }
}
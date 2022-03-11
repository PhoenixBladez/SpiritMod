using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class CritterCaptureSoulOrb : Quest
    {
        public override string QuestName => "Sanctuary: Soul Searching";
		public override string QuestClient => "The Dryad";
		public override string QuestDescription => "The strange new Spirit fields that have emerged over the horizon bear countless new plants and animals. But one little soul stands no chance if we do not go and capture it. These Soul Orbs light up the depths of the Underground Spirit, and I'd like to make sure they are kept safe. On the way, could you also be able to provide me with some Soulbloom cuttings?";
		public override int Difficulty => 3;
		public override string QuestCategory => "Forager";

		public override (int, int)[] QuestRewards => _rewards;
		private (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Books.Book_SpiritArt>(), 1),
			(ModContent.ItemType<Items.Sets.RunicSet.Rune>(), 5),
			(ModContent.ItemType<Items.Placeable.SoulSeeds>(), 3),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		private CritterCaptureSoulOrb()
        {
			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.SoulOrbItem>(), 1, "These orbs seem to posses the souls of restless creatures. Perhaps we can bring them some peace in my sanctuary. You have done a great service for this soul and myself, traveller. Thank you.", "Bring the Soul Orb back to the Dryad", true, true));

			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new RetrievalTask(ModContent.ItemType<Items.Material.SoulBloom>(), 1, "Optional: Gather"))
				   .AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Material.SoulBloom>(), 1, "These flowers are so otherworldly and mesmerizing. Thank you for bringing them to me. I hope that you can plant a field full of these wonderful flowers.", "Bring the Soul Orb and Soulbloom back to the Dryad", true, true, ModContent.ItemType<Items.Books.Book_Soulbloom>()))
				   .AddTask(new GiveNPCTask(NPCID.Dryad, ModContent.ItemType<Items.Consumable.SoulOrbItem>(), 1, "These orbs seem to posses the souls of restless creatures. Perhaps we can bring them some peace in my sanctuary. You have done a great service for this soul and myself, traveller. Thank you.", "Bring the Soul Orb and Soulbloom back to the Dryad", true, true));

			_tasks.AddTask(new RetrievalTask(ModContent.ItemType<Items.Consumable.SoulOrbItem>(), 1, "Retrieve"));
			_tasks.AddBranches(branch1, branch2);

		}
    }
}
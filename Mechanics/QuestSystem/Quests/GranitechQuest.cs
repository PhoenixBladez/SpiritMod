using SpiritMod.Mechanics.QuestSystem.Tasks;
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
	public class GranitechQuest : Quest
	{
		public override string QuestName => "Futuristic Forces";
		public override string QuestClient => "The Mechanic";
		public override string QuestDescription => "You know, I've been seeing some strange parts in the scrap I've been buying lately. It's quite amazing, honestly! The circuitry is way out of my league, like it's from the future or something! I'd love to mess around with them and learn more, so would you get some for me? They're attached to some dangerous machinery, though. Come back in one piece!";
		public override int Difficulty => 3;
		public override string QuestCategory => "Main";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Accessory.Rangefinder.Rangefinder>(), 1),
			(ModContent.ItemType<Items.Sets.GranitechSet.GranitechMaterial>(), 10),
			(Terraria.ID.ItemID.Wire, 100),
			(Terraria.ID.ItemID.GoldCoin, 3)
		};

		private GranitechQuest()
		{
			_tasks.AddParallelTasks(new SlayTask(ModContent.NPCType<NPCs.GraniTech.GraniteSentry>(), 3),
									new RetrievalTask(ModContent.ItemType<Items.Sets.GranitechSet.GranitechMaterial>(), 10, "Retrieve"))
				  .AddTask(new GiveNPCTask(NPCID.Mechanic, ModContent.ItemType<Items.Sets.GranitechSet.GranitechMaterial>(), 10, "Wow! These parts are almost otherworldly! You said you fought a bunch of high-precision laser turrets to get these? I mean, that makes sense, but there's so much more that this circuitry could accomplish. It's a combination of magic and energy that I've never seen before, and I think the people behind those turrets are a force to be wary of. It's best you grab as many circuits as you can and prepare before they make their move!", "Return to the Mechanic with the circuits", true, false, ModContent.ItemType<NPCs.Town.Oracle.OracleScripture>()));
		}
	}
}
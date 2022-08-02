using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Tasks;
using SpiritMod.Items.Sets.MaterialsMisc.QuestItems;

namespace SpiritMod.Mechanics.QuestSystem.Quests
{
    public class IceDeityQuest : Quest
    {
        public override string QuestName => "Beneath the Ice";
		public override string QuestClient => "The Enchanter";
		public override string QuestDescription => "Welcome, friend! I've got a bit of an unusual request for you today. You see, I've sensed some strange energy emanating from the frozen tundra as of late. I'd like for you to venture there and learn more, but not before we gain some knowledge beforehand. First, let's ask around.";
		public override int Difficulty => 3;
		public override string QuestCategory => "Explorer";

		public override (int, int)[] QuestRewards => _rewards;
		private readonly (int, int)[] _rewards = new[]
		{
			(ModContent.ItemType<Items.Placeable.IceSculpture.WinterbornSculpture>(), 3),
			(ModContent.ItemType<Items.Placeable.IceSculpture.IceDeitySculpture>(), 1),
			(ModContent.ItemType<Items.Armor.HunterArmor.SnowRangerHead>(), 1),
			(ModContent.ItemType<Items.Armor.HunterArmor.SnowRangerBody>(), 1),
			(ModContent.ItemType<Items.Armor.HunterArmor.SnowRangerLegs>(), 1),
			(ModContent.ItemType<Items.Accessory.FrostGiantBelt>(), 1),
			(ItemID.GoldCoin, 3)
		};

		private IceDeityQuest()
        {
			TaskBuilder branch2 = new TaskBuilder();
			branch2.AddTask(new SlayTask(new int[] { ModContent.NPCType<NPCs.Winterborn.WinterbornMelee>(), ModContent.NPCType<NPCs.WinterbornHerald.WinterbornMagic>() }, 5, "Find clues about the past by killing Winterborn"))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard1>(), 1))
				   .AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.RuneWizard>(), "What a curious relic! It seems to be part of a much larger statue. Perhaps the strange Winterborn worshipped this deity? Or perhaps not- it could just be the creation of another long-dead civilzation whose souls roam the frozen depths. Who knows? But it's intriguing, and I wish to reassemble the sculpture. Can you collect the other missing pieces? I'm sure one can be found on Crystal Drifters, strange ice entities that emerge during Blizzards. We could probably fashion the other pieces out of Creeping Ice and Cryolite, found underground.", "Show the Enchanter the strange relic"))
				   .AddTask(new SlayTask(ModContent.NPCType<NPCs.CrystalDrifter.CrystalDrifter>(), 1, "Kill a Crystal Drifter", new QuestPoolData(0.65f, true)))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard2>(), 1))
				   .AddParallelTasks(new RetrievalTask(ModContent.ItemType<Items.Sets.CryoliteSet.CryoliteBar>(), 8), new RetrievalTask(ModContent.ItemType<Items.Placeable.Tiles.CreepingIce>(), 25))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard3>(), 1))
				   .AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.Town.RuneWizard>(), new int[] { ModContent.ItemType<IceDeityShard1>(), ModContent.ItemType<IceDeityShard2>(), ModContent.ItemType<IceDeityShard3>() }, new int[] { 1, 1, 1 }, "You've done a splendid job! These artifacts seem to create a giant statue that resembles a warrior-knight of some kind. Perhaps it's a deity. Perhaps it's just a passion project- there's so much we don't know! And though I'm not sure why the icy caverns have become so dangerous, I'm content with having learned more about our world. I do hope you put this statue up somewhere!", "Give all the artifacts to the Enchanter", true, true));

			TaskBuilder branch3 = new TaskBuilder();
			branch3.AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.FrozenSouls.WintrySoul>(), "As I approach the spirit, the cave around me seems to shake. The walls collapse, leaving me standing in a boundless chasm. I am alone. Countless piles of gold and relics lay strewn about, covered in a thin layer of frost. The only light in the cavern seems to come from the soul beside me as it waits for me to make a decision.", "Or try to find a soul in the Ice Biome", new QuestPoolData(0.85f, true)))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard1>(), 1))
				   .AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.RuneWizard>(), "What a curious relic! It seems to be part of a much larger statue. Perhaps the strange Winterborn worshipped this deity? Or perhaps not- it could just be the creation of another long-dead civilzation whose souls roam the frozen depths. Who knows? But it's intriguing, and I wish to reassemble the sculpture. Can you collect the other missing pieces? I'm sure one can be found on Crystal Drifters, strange ice entities that emerge during Blizzards. We could probably fashion the other pieces out of Creeping Ice and Cryolite, found underground.", "Show the Enchanter the strange relic"))
				   .AddTask(new SlayTask(ModContent.NPCType<NPCs.CrystalDrifter.CrystalDrifter>(), 1, "Kill a Crystal Drifter", new QuestPoolData(0.65f, true)))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard2>(), 1))
				   .AddParallelTasks(new RetrievalTask(ModContent.ItemType<Items.Sets.CryoliteSet.CryoliteBar>(), 8), new RetrievalTask(ModContent.ItemType<Items.Placeable.Tiles.CreepingIce>(), 25))
				   .AddTask(new RetrievalTask(ModContent.ItemType<IceDeityShard3>(), 1))
				   .AddTask(new GiveNPCTask(ModContent.NPCType<NPCs.Town.RuneWizard>(), new int[] { ModContent.ItemType<IceDeityShard1>(), ModContent.ItemType<IceDeityShard2>(), ModContent.ItemType<IceDeityShard3>() }, new int[] { 1, 1, 1 }, "You've done a splendid job! These artifacts seem to create a giant statue that resembles a warrior-knight of some kind. Perhaps it's a deity. Perhaps it's just a passion project- there's so much we don't know! And though I'm not sure why the icy caverns have become so dangerous, I'm content with having learned more about our world. I do hope you put this statue up somewhere!", "Give all the artifacts to the Enchanter", true, true));

			TaskBuilder branch1 = new TaskBuilder();
			branch1.AddTask(new TalkNPCTask(NPCID.Dryad, "So you wish to know about the icy caverns? Well, they have existed since before I was born- I'm not that old! However, I have heard tales about numerous civilizations rising and falling beneath the frozen surface. Some were peaceful, while others went so far as to freeze their enemies alive for sport! If you listen closely, I am sure that you could glean more information from the souls of the frozen caverns.", "Ask the Dryad about the ancient myths"))
				   .AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.Adventurer>(), "I don't know much about ancient history, but a few of my associates have scouted a growing threat in the icy caverns. Sometimes, they even get bold enough to roam the surface durin' heavy blizzards. I'm not sure what's got 'em so riled up, but the frozen fields are more dangerous than ever.", "Ask the Adventurer about the Snow Biome"))
				   .AddTask(new TalkNPCTask(ModContent.NPCType<NPCs.Town.RuneWizard>(), "I see! These leads are promising- perhaps killing some of these frozen creatures or trying to find one of these souls the Dryad mentioned could shed some light on the restlessness of the ice biome.", "Return to the Enchanter with what you know"))
				   .AddBranches(branch2, branch3);

			_tasks.AddBranches(branch1);
		}
	}
}
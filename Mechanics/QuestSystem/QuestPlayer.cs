using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestPlayer : ModPlayer
	{
		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if (player.ZoneJungle && QuestManager.GetQuest<ItsNoSalmon>().IsActive && Main.rand.NextBool(10))
				caughtType = ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>();
		}
	}
}

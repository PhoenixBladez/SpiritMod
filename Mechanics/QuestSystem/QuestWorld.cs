using System;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestWorld : ModWorld
	{
		public override void PostUpdate()
		{
			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();

            if (Main.hardMode)
            {
                QuestManager.UnlockQuest<ExplorerQuestBlueMoon>(true);
                QuestManager.UnlockQuest<SlayerQuestVultureMatriarch>(true);
            }
        }
    }
}
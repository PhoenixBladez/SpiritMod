using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.QuestSystem
{
    public class QuestGlobalNPC : GlobalNPC
    {
		public static event Action<IDictionary<int, float>, NPCSpawnInfo> OnEditSpawnPool;
        public static event Action<NPC> OnNPCLoot;

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) => OnEditSpawnPool?.Invoke(pool, spawnInfo);

		public override void NPCLoot(NPC npc)
        {            
            if (npc.type == NPCID.EyeofCthulhu || 
				npc.type == NPCID.EaterofWorldsHead || 
				npc.type == NPCID.SkeletronHead || 
				npc.type == ModContent.NPCType<NPCs.Boss.Scarabeus.Scarabeus>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.AncientFlyer>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.MoonWizard.MoonWizard>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.SteamRaider.SteamRaiderHead>())
            {
                QuestManager.UnlockQuest<SlayerQuestOccultist>(true);
                QuestManager.UnlockQuest<UnidentifiedFloatingObjects>(true);
            }

            if (npc.type == NPCID.EaterofWorldsHead)
			{
                QuestManager.UnlockQuest<SlayerQuestMarble>(true);
                QuestManager.UnlockQuest<SlayerQuestMeteor>(true);                
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                QuestManager.UnlockQuest<RaidingTheStars>(true);
                QuestManager.UnlockQuest<SongOfIceAndFire>(true);
				QuestManager.UnlockQuest<StrangeSeas>(true);
            }
            OnNPCLoot?.Invoke(npc);
        }

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor) //Draws the exclamation mark on the NPC when they have a quest
		{
			bool valid = ModContent.GetInstance<SpiritClientConfig>().ShowQuestExclamationMarks && npc.CanTalk; //Check if the NPC talks and if the config allows
			if (valid && ModContent.GetInstance<QuestWorld>().NPCQuestQueue.ContainsKey(npc.type) && ModContent.GetInstance<QuestWorld>().NPCQuestQueue[npc.type].Count > 0)
			{
				Texture2D tex = mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
				float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
			}
		}
	}
}

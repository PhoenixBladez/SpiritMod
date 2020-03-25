using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class OceanSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 14;
            npc.damage = 17;
            npc.defense = 6;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 860f;
            npc.knockBackResist = .45f;
            npc.aiStyle = 1;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || !NPC.downedBoss1)
            {
                return 0f;
            }
            return SpawnCondition.Ocean.Chance * 0.18f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            int d = 207;
            int d1 = 207;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
        }
    }
}

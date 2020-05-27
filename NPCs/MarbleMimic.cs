using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs
{
    public class MarbleMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marble Mimic");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.width = 28;
            npc.height = 24;
            npc.damage = 80;
            npc.defense = 35;
            npc.lifeMax = 550;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 50000f;
            npc.knockBackResist = .30f;
            npc.aiStyle = 25;
            aiType = NPCID.Mimic;
            animationType = NPCID.Mimic;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int d1 = 236;
            for (int k = 0; k < 5; k++)
            {
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
                }
            }
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return (tile == 367) && Main.hardMode && spawnInfo.spawnTileY > Main.rockLayer ? 0.01f : 0f;
        }

        public override void NPCLoot()
        {
            string[] lootTable = { "TatteredShotbow", "GoldenApple", "ZeusLightning", "CircleScimitar" };
            int loot = Main.rand.Next(lootTable.Length);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));
        }
    }
}

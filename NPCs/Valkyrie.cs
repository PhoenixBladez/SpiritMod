using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class Valkyrie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Valkyrie");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Harpy];
        }
        public override void SetDefaults()
        {
            npc.width = 98;
            npc.height = 70;
            npc.damage = 23;
            npc.defense = 15;
            npc.lifeMax = 110;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 560f;
            npc.knockBackResist = .15f;
            npc.aiStyle = 14;
            animationType = NPCID.Harpy;
        }
        public override void AI()
        {
            {
                if (Main.rand.Next(150) == 6) //Fires desert feathers like a shotgun
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 14f;
                    direction.Y *= 14f;

                    int amountOfProjectiles = Main.rand.Next(1, 1);
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-150, 150) * 0.01f;
                        float B = (float)Main.rand.Next(-150, 150) * 0.01f;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ProjectileID.JavelinHostile, 12, 1, Main.myPlayer, 0, 0);
                    }
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
            return  (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.1f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++) ;
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Valkyrie3"), 1f);
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(5) == 1)
            {
                target.AddBuff(BuffID.Silenced, 160);
            }
        }
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MarbleChunk"), 1);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.NPCs.Dungeon;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;

namespace SpiritMod.NPCs.Tides
{
    public class Crocomount : ModNPC
    {
        bool attack = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crocomount");
            Main.npcFrameCount[npc.type] = 11;
        }

        public override void SetDefaults() {
            npc.width = 88;
            npc.height = 77;
            npc.damage = 29;
            npc.defense = 16;
            npc.lifeMax = 600;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath16;
            npc.value = 600f;
            npc.knockBackResist = .35f;
        }


        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crocomount/CrocomountGore4"), 1f);
            }
        }


        int frame = 0;
        int timer = 0;
        //  int shootTimer = 0;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 80) {
                attack = true;
            }
            if(distance > 100) {
                attack = false;
            }
            if(attack) {
                npc.velocity.X = .008f * npc.direction;
                //shootTimer++;
                timer++;
                if(timer >= 5) {
                    frame++;
                    timer = 0;
                }
                if(frame > 10) {
                    frame = 7;
                }
                 if(frame < 7) {
                    frame = 7;
                }
                if(target.position.X > npc.position.X) {
                    npc.direction = 1;
                } else {
                    npc.direction = -1;
                }
            } else {
                //shootTimer = 0;
                npc.aiStyle = 26;
                aiType = NPCID.Skeleton;
                timer++;
                if(timer >= 4) {
                    frame++;
                    timer = 0;
                }
                if(frame > 6) {
                    frame = 0;
                }
            }
            /*if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }*/
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
    }
}

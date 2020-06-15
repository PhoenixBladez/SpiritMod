using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.NPCs.BlueMoon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.BlueMoon
{
    public class GlowToad : ModNPC
    {
        int timer = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Glow Toad");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults() {
            npc.width = 64;
            npc.height = 60;
            npc.damage = 49;
            npc.defense = 14;
            npc.lifeMax = 380;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 2000f;
            npc.knockBackResist = 0.5f;
            // npc.aiStyle = 26;
            // aiType = NPCID.Unicorn;
        }
        public override void HitEffect(int hitDirection, double damage) {
            Main.PlaySound(31, (int)npc.position.X, (int)npc.position.Y);

            for(int k = 0; k < 5; k++)
                Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);

            if(npc.life <= 0) {
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 40;
                npc.height = 48;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 200; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
        }
        bool tongueOut = false;
        bool tongueActive = false;
        int cooldownTimer = 5;
        bool jumping = false;
        public override void AI() {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (Math.Abs(player.position.X - npc.position.X) < 190 && npc.collideY)
            {
                tongueOut = true;
            }
            if (!tongueOut && cooldownTimer > 4)
            {
                timer++;
            }
            if (!npc.collideY)
            {
                 npc.velocity.X *= 1.045f;
            }
            cooldownTimer++;
            if(timer % 90 == 0 && !tongueOut) {
                if (!jumping)
                {
                    jumping = true;
                     cooldownTimer = 0;
                }
                cooldownTimer++;
                 npc.velocity.Y = -7;
                if(player.position.X > npc.position.X) {
                    npc.velocity.X = 14;
                    npc.netUpdate = true;
                } else {
                    npc.velocity.X = -14;
                    npc.netUpdate = true;
                }
            }
            else
            {
                jumping = false;
            } 
            if (!tongueActive)
            {
                if(player.position.X > npc.position.X) {
                    npc.spriteDirection = 0;
                } else {
                    npc.spriteDirection = 1;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return MyWorld.BlueMoon ? 7f : 0f;
        }
        float frameCounter = 0;
        int tongueproj = 0;
        public override void FindFrame(int frameHeight) {
            Player player = Main.player[npc.target];
            if(!npc.collideY) {
                //npc.frameCounter += 0.40f;
                // npc.frameCounter %= 5;
                // int frame = (int)npc.frameCounter;
                npc.frame.Y = 0;
                frameCounter = 0;
            } else if (!tongueOut){
                npc.frame.Y = frameHeight;
                frameCounter = 0f;
            }
            if (tongueOut)
            {
               // timer = 100;
                if (!tongueActive)
                {
                    frameCounter+= 0.11f;
                }
                npc.frame.Y = ((int)(frameCounter % 5) + 1) * frameHeight;
                if (npc.frame.Y / frameHeight == 5 && !tongueActive)
                {
                    npc.TargetClosest(true);
                    tongueActive = true;
                    npc.knockBackResist = 0f;
                    if (npc.spriteDirection == 0)
                    {
                        tongueproj = Projectile.NewProjectile(npc.Center + new Vector2(21, 8), new Vector2(11,0), ModContent.ProjectileType<GlowTongue>(), (int)(npc.damage / 1.7f), 1, player.whoAmI, 1);
                    }
                    else
                    {
                        tongueproj = Projectile.NewProjectile(npc.Center + new Vector2(-18, 8), new Vector2(-11,0), ModContent.ProjectileType<GlowTongue>(), (int)(npc.damage / 1.7f), 1, player.whoAmI, 0);
                    }
                }
                 if (tongueActive)
                {
                    Projectile tongue = Main.projectile[tongueproj]; 
                    if (!tongue.active)
                    {
                        tongueActive = false;
                        tongueOut = false;
                        timer = 80;
                        npc.knockBackResist = 0.5f;
                         if(player.position.X > npc.position.X) {
                            npc.spriteDirection = 0;
                        } else {
                            npc.spriteDirection = 1;
                        }
                    }
                    else
                    {
                        if (tongue.ai[0] == 0)
                        {
                            npc.spriteDirection = 1;
                        }
                        else
                        {
                            npc.spriteDirection = 0;
                        }
                    }
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
        }

        public override void NPCLoot() {
            if(Main.rand.Next(40) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GloomgusStaff>());
        }

    }
}

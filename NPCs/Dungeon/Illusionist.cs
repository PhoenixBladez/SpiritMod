using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
    public class Illusionist : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ghast");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults() {
            npc.width = 40;
            npc.height = 90;

            npc.lifeMax = 155;
            npc.defense = 14;
            npc.damage = 25;

            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.value = 1200f;
            npc.knockBackResist = 0.75f;

            npc.noGravity = true;
            npc.netAlways = true;
            npc.chaseable = true;
            npc.noTileCollide = true;
            npc.lavaImmune = true;
        }

        int frame = 5;
        int timer = 0;
        bool aggroed = false;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 100f;

        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 200) {
                if(!aggroed) {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
                }
                aggroed = true;
            }

            if(!aggroed) {
                if(npc.localAI[0] == 0f) {
                    npc.localAI[0] = npc.Center.Y;
                    npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
                }
                if(npc.Center.Y >= npc.localAI[0]) {
                    npc.localAI[1] = -1f;
                    npc.netUpdate = true;
                }
                if(npc.Center.Y <= npc.localAI[0] - 2f) {
                    npc.localAI[1] = 1f;
                    npc.netUpdate = true;
                }
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.009f * npc.localAI[1], -.25f, .25f);
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 4) {
                    frame = 1;
                }
            } else {
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 3) {
                    frame = 0;
                }
                Player player = Main.player[npc.target];

                if(npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
                    moveSpeed--;

                if(npc.Center.X <= player.Center.X && moveSpeed <= 30)
                    moveSpeed++;

                npc.velocity.X = moveSpeed * 0.08f;

                if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
                {
                    moveSpeedY--;
                    HomeY = 165f;
                }

                if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 20)
                    moveSpeedY++;

                npc.velocity.Y = moveSpeedY * 0.1f;
                if(Main.rand.Next(250) == 1) {
                    HomeY = -25f;
                }

                ++npc.ai[0];
                if(NPC.CountNPCS(ModContent.NPCType<IllusionistSpectre>()) < 3) {
                    if(npc.ai[0] == 240 || npc.ai[0] == 480 || npc.ai[0] == 720) {
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                        Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
                        if(Main.netMode != 1) {
                            NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.Center.Y - 16, ModContent.NPCType<IllusionistSpectre>(), 0, 0, 0, 0, 0, 255);
                        }
                        for(int k = 0; k < 30; k++) {
                            int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 68);
                            Main.dust[dust].velocity *= -1f;
                            Main.dust[dust].noGravity = true;
                            Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                            vector2_1.Normalize();
                            Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                            Main.dust[dust].velocity = vector2_2;
                            vector2_2.Normalize();
                            Vector2 vector2_3 = vector2_2 * 74f;
                            Main.dust[dust].position = npc.Center - vector2_3;
                        }
                    }

                    if(npc.ai[0] >= 230 && npc.ai[0] <= 250 || npc.ai[0] >= 470 && npc.ai[0] <= 485 || npc.ai[0] >= 710 && npc.ai[0] <= 725) {
                        frame = 4;
                    }
                }
                if(npc.ai[0] >= 720) {
                    npc.ai[0] = 0;
                }
            }

            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.122f, .5f, .48f);
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(189, 195, 184);
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) {
            if(!aggroed) {
                Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
            }
            aggroed = true;
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<Illusionist>()) < 1 ? 0.05f : 0f;
        }
        public override void NPCLoot() {
            if(Main.rand.Next(1) == 153) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if(Main.rand.Next(1) == 75) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);
            }
            if(Main.rand.Next(1) == 100) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
            if(Main.rand.Next(4) == 1000) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
            }
            if(Main.rand.Next(20) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<IllusionistEye>());
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            int d = 37;
            int d1 = 180;
            for(int k = 0; k < 30; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Illusionist1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Illusionist2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Illusionist3"), 1f);
            }
        }
    }
}
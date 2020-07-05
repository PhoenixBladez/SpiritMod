using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using Terraria;
using Terraria.ID;
using SpiritMod;
using Terraria.ModLoader;
using SpiritMod.Tide;

namespace SpiritMod.NPCs.Tides
{
    public class KakamoraRider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kakamoran Rider");
            Main.npcFrameCount[npc.type] = 1;
        }

         public override void SetDefaults() {
            npc.width = 42;
            npc.height = 38;
            npc.damage = 18;
            npc.defense = 4;
            aiType = NPCID.SnowFlinx;
            npc.aiStyle = 3;
            npc.lifeMax = 160;
            npc.knockBackResist = .70f;
            npc.value = 200f;
            npc.noTileCollide = false;
             npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        // localai0 : 0 when spawned, 1 when otherNPC spawned. 
        // ai0 = npc number of other NPC
        // ai1 = charge time for gun.
        // ai2 = used for frame??
        // ai3 = 
        bool checkSpawn = false;
        public override void AI()
        {
            int otherNPC = -1;
            Vector2 offsetFromOtherNPC = new Vector2(-15, -18);
            if (npc.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient && !checkSpawn)
            {
				
                npc.localAI[0] = 1f;
                int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Crocomount>(), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                npc.ai[0] = (float)newNPC;
                checkSpawn = true;
                npc.netUpdate = true;
            }
            int otherNPCCheck = (int)npc.ai[0];
            if (Main.npc[otherNPCCheck].active && Main.npc[otherNPCCheck].type == ModContent.NPCType<Crocomount>())
            {
                if (npc.timeLeft < 60)
                {
                    npc.timeLeft = 60;
                }
                otherNPC = otherNPCCheck;
               // offsetFromOtherNPC = Vector2.UnitY * -24f;
            }

            // If otherNPC exists, do this
            if (otherNPC != -1)
            {
                NPC nPC7 = Main.npc[otherNPC];
                npc.velocity = Vector2.Zero;
                npc.position = nPC7.Center;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                npc.position.Y += offsetFromOtherNPC.Y;
                npc.position.X += offsetFromOtherNPC.X * nPC7.direction;
                npc.gfxOffY = nPC7.gfxOffY;
                npc.rotation = 0f;
                npc.direction = nPC7.direction;
                npc.spriteDirection = nPC7.spriteDirection;
                npc.timeLeft = nPC7.timeLeft;
                npc.velocity = nPC7.velocity;
                npc.target = nPC7.target;
                if (npc.ai[1] < 60f)
                {
                    npc.ai[1] += 1f;
                }
                if (npc.justHit)
                {
                    npc.ai[1] = -30f;
                }
                int projectileType = Terraria.ID.ProjectileID.RayGunnerLaser;// 438;
                int projectileDamage = 30;
                float scaleFactor20 = 7f;
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    Vector2 vectorToPlayer = Main.player[npc.target].Center - npc.Center;
                    Vector2 vectorToPlayerNormalized = Vector2.Normalize(vectorToPlayer);
                    float num1547 = vectorToPlayer.Length();
                    float num1548 = 700f;

                    if (num1547 < num1548)
                    {
                        if (npc.ai[1] == 60f && Math.Sign(vectorToPlayer.X) == npc.direction)
                        {
                            npc.ai[1] = -60f;
                            Vector2 center12 = Main.player[npc.target].Center;
                            Vector2 value26 = npc.Center - Vector2.UnitY * 4f;
                            Vector2 vector188 = center12 - value26;
                            vector188.X += (float)Main.rand.Next(-50, 51);
                            vector188.Y += (float)Main.rand.Next(-50, 51);
                            vector188.X *= (float)Main.rand.Next(80, 121) * 0.01f;
                            vector188.Y *= (float)Main.rand.Next(80, 121) * 0.01f;
                            vector188.Normalize();
                            if (float.IsNaN(vector188.X) || float.IsNaN(vector188.Y))
                            {
                                vector188 = -Vector2.UnitY;
                            }
                            vector188 *= scaleFactor20;
                            Projectile.NewProjectile(value26.X, value26.Y, vector188.X, vector188.Y, projectileType, projectileDamage, 0f, Main.myPlayer, 0f, 0f);
                            npc.netUpdate = true;
                        }
                        else
                        {
                            float oldAI2 = npc.ai[2];
                            npc.velocity.X = npc.velocity.X * 0.5f;
                            npc.ai[2] = 3f;
                            if (Math.Abs(vectorToPlayerNormalized.Y) > Math.Abs(vectorToPlayerNormalized.X) * 2f)
                            {
                                if (vectorToPlayerNormalized.Y > 0f)
                                {
                                    npc.ai[2] = 1f;
                                }
                                else
                                {
                                    npc.ai[2] = 5f;
                                }
                            }
                            else if (Math.Abs(vectorToPlayerNormalized.X) > Math.Abs(vectorToPlayerNormalized.Y) * 2f)
                            {
                                npc.ai[2] = 3f;
                            }
                            else if (vectorToPlayerNormalized.Y > 0f)
                            {
                                npc.ai[2] = 2f;
                            }
                            else
                            {
                                npc.ai[2] = 4f;
                            }
                            if (npc.ai[2] != oldAI2)
                            {
                                npc.netUpdate = true;
                            }
                        }
                    }
                }

            }
            else
            {
                // This code is called when Bottom is dead. Top is transformed into a new NPC.
                switch (Main.rand.Next(4))
                {
                    case 0:
                        npc.Transform(ModContent.NPCType<KakamoraRunner>());
                        break;
                    case 1:
                        npc.Transform(ModContent.NPCType<SpearKakamora>());
                        break;
                    case 2:
                        npc.Transform(ModContent.NPCType<SwordKakamora>());
                        break;
                    case 3:
                        npc.Transform(ModContent.NPCType<KakamoraShielder>());
                        break;
                }
                return;
            }
        }
        public override void NPCLoot()
        {
        }
         public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                int d = 207;
                int d1 = 207;
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
                }
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore2"), 1f);
                 Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore3"), 1f);
                if (TideWorld.TheTide && TideWorld.TidePoints < 99)
                {
                    TideWorld.TidePoints += 1;
                }
            }
            else
            {
                 Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
            }
        }
    }
}

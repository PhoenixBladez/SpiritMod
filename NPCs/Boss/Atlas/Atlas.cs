using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Atlas
{
    [AutoloadBossHead]
    public class Atlas : ModNPC
    {
        public static int _type;

        int[] arms = new int[2];
        int timer = 0;
        bool secondStage = false;
        bool thirdStage = false;
        bool lastStage = false;
        int collideTimer = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Atlas");
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 0;
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults() {
            npc.width = 147;
            npc.height = 233;
            npc.scale = 2.0f;
            bossBag = ModContent.ItemType<AtlasBag>();
            npc.damage = 100;
            npc.lifeMax = 41000;
            npc.defense = 48;
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.timeLeft = NPC.activeTime * 30;
            npc.noGravity = true;
            npc.alpha = 255;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath5;
            music = MusicID.Boss4;
        }

        private int Counter;
        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
        public override void AI() {
            bool expertMode = Main.expertMode; //expert mode bool
            Player player = Main.player[npc.target]; //player target
            bool aiChange = (double)npc.life <= (double)npc.lifeMax * 0.75; //ai change to phase 2
            bool aiChange2 = (double)npc.life <= (double)npc.lifeMax * 0.5; //ai change to phase 3
            bool aiChange3 = (double)npc.life <= (double)npc.lifeMax * 0.25; //ai change to phase 4
            bool phaseChange = (double)npc.life <= (double)npc.lifeMax * 0.1; //aggression increase
            player.AddBuff(ModContent.BuffType<UnstableAffliction>(), 2); //buff that causes gravity shit
            int defenseBuff = (int)(65f * (1f - npc.life / npc.lifeMax));
            npc.defense = npc.defDefense + defenseBuff;

            if(npc.ai[0] == 0f) {
                arms[0] = NPC.NewNPC((int)npc.Center.X - 80 - Main.rand.Next(80, 160), (int)npc.position.Y, ModContent.NPCType<AtlasArmLeft>(), npc.whoAmI, npc.whoAmI);
                arms[1] = NPC.NewNPC((int)npc.Center.X + 80 + Main.rand.Next(80, 160), (int)npc.position.Y, ModContent.NPCType<AtlasArmRight>(), npc.whoAmI, npc.whoAmI);
                npc.ai[0] = 1f;
            } else if(npc.ai[0] == 1f) {
                npc.ai[1] += 1f;
                if(npc.ai[1] >= 210f) {
                    npc.alpha -= 4;
                    if(npc.alpha <= 0) {
                        npc.ai[0] = 2f;
                        npc.ai[1] = 0f;
                        npc.alpha = 0;
                        npc.velocity.Y = 14f;
                        npc.netUpdate = true;
                    }
                }
            } else if(npc.ai[0] == 2f) {
                if(npc.alpha == 0) {
                    Vector2 dist = player.Center - npc.Center;
                    Vector2 direction = player.Center - npc.Center;
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                    if(!player.active || player.dead) {
                        npc.TargetClosest(false);
                        npc.velocity.Y = -100f;
                    }

                    #region Dashing mechanics
                    //dash if player is too far away
                    if(Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) > 155) {
                        direction.Normalize();
                        npc.velocity *= 0.99f;
                        if(Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) >= 7f) {
                            int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 1, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                            Main.dust[dust].noGravity = true;
                            dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 1, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                            Main.dust[dust].noGravity = true;
                        }

                        if(Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) < 2f) {
                            if(Main.rand.Next(25) == 1) {
                                direction.X = direction.X * Main.rand.Next(19, 24);
                                direction.Y = direction.Y * Main.rand.Next(19, 24);
                                npc.velocity.X = direction.X;
                                npc.velocity.Y = direction.Y;
                            }
                        }
                    }
                    #endregion

                    #region Flying Movement
                    if(Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 325) {
                        float speed = expertMode ? 21f : 18f; //made more aggressive.  expert mode is more.  dusking base value is 7
                        float acceleration = expertMode ? 0.16f : 0.13f; //made more aggressive.  expert mode is more.  dusking base value is 0.09
                        Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float xDir = player.position.X + (float)(player.width / 2) - vector2.X;
                        float yDir = (float)(player.position.Y + (player.height / 2) - 120) - vector2.Y;
                        float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
                        if(length > 400f) {
                            ++speed;
                            acceleration += 0.08F;
                            if(length > 600f) {
                                ++speed;
                                acceleration += 0.08F;
                                if(length > 800f) {
                                    ++speed;
                                    acceleration += 0.08F;
                                }
                            }
                        }
                        float num10 = speed / length;
                        xDir = xDir * num10;
                        yDir = yDir * num10;
                        if(npc.velocity.X < xDir) {
                            npc.velocity.X = npc.velocity.X + acceleration;
                            if(npc.velocity.X < 0 && xDir > 0)
                                npc.velocity.X = npc.velocity.X + acceleration;
                        } else if(npc.velocity.X > xDir) {
                            npc.velocity.X = npc.velocity.X - acceleration;
                            if(npc.velocity.X > 0 && xDir < 0)
                                npc.velocity.X = npc.velocity.X - acceleration;
                        }
                        if(npc.velocity.Y < yDir) {
                            npc.velocity.Y = npc.velocity.Y + acceleration;
                            if(npc.velocity.Y < 0 && yDir > 0)
                                npc.velocity.Y = npc.velocity.Y + acceleration;
                        } else if(npc.velocity.Y > yDir) {
                            npc.velocity.Y = npc.velocity.Y - acceleration;
                            if(npc.velocity.Y > 0 && yDir < 0)
                                npc.velocity.Y = npc.velocity.Y - acceleration;
                        }
                    }
                    #endregion
                    timer += phaseChange ? 2 : 1; //if below 20% life fire more often
                    int shootThings = expertMode ? 200 : 250; //fire more often in expert mode
                    if(timer > shootThings) {
                        direction.Normalize();
                        direction.X *= 8f;
                        direction.Y *= 8f;
                        int amountOfProjectiles = Main.rand.Next(6, 8);
                        int damageAmount = expertMode ? 54 : 62; //always account for expert damage values
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 92);
                        for(int num621 = 0; num621 < 30; num621++) {
                            int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 2f);
                        }

                        for(int i = 0; i < amountOfProjectiles; ++i) {
                            float A = (float)Main.rand.Next(-250, 250) * 0.01f;
                            float B = (float)Main.rand.Next(-250, 250) * 0.01f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<PrismaticBoltHostile>(), damageAmount, 1, npc.target);
                            timer = 0;
                        }
                    }

                    if(aiChange) {
                        if(secondStage == false) {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 93);
                            float radius = 250;
                            float rot = MathHelper.TwoPi / 5;
                            for(int I = 0; I < 5; I++) {
                                Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
                                NPC.NewNPC((int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye>(), npc.whoAmI, npc.whoAmI, I * rot, radius);
                            }
                            secondStage = true;
                        }
                    }

                    if(aiChange2) {
                        if(thirdStage == false) {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 93);
                            float radius = 400;
                            float rot = MathHelper.TwoPi / 10;
                            for(int I = 0; I < 10; I++) {
                                Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
                                NPC.NewNPC((int)(position.X), (int)(position.Y), mod.NPCType("CobbledEye2"), npc.whoAmI, npc.whoAmI, I * rot, radius);
                            }
                            thirdStage = true;
                        }
                    }

                    if(aiChange3) {
                        if(lastStage == false) {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 93);
                            float radius = 1200;
                            float rot = MathHelper.TwoPi / 41;
                            for(int I = 0; I < 41; I++) {
                                Vector2 position = npc.Center + radius * (I * rot).ToRotationVector2();
                                NPC.NewNPC((int)(position.X), (int)(position.Y), mod.NPCType("CobbledEye3"), npc.whoAmI, npc.whoAmI, I * rot, radius);
                            }
                            lastStage = true;
                        }
                    }
                    for(int index1 = 0; index1 < 6; ++index1) {
                        float x = (npc.Center.X - 2);
                        float xnum2 = (npc.Center.X + 2);
                        float y = (npc.Center.Y - 160);
                        if(npc.direction == -1) {
                            int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 226, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].position.X = x;
                            Main.dust[index2].position.Y = y;
                            Main.dust[index2].scale = .85f;
                            Main.dust[index2].velocity *= 0.02f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].noLight = false;
                        } else if(npc.direction == 1) {
                            int index2 = Dust.NewDust(new Vector2(xnum2, y), 1, 1, 226, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].position.X = xnum2;
                            Main.dust[index2].position.Y = y;
                            Main.dust[index2].scale = .85f;
                            Main.dust[index2].velocity *= 0.02f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].noLight = false;
                        }
                    }
                }
            }

            collideTimer++;
            if(collideTimer == 500) {
                npc.noTileCollide = true;
            }

            npc.TargetClosest(true);
            if(!player.active || player.dead) {
                npc.TargetClosest(false);
                npc.velocity.Y = -100f;
                timer = 0;
            }

            Counter++;
            if(Counter > 400) {
                SpiritMod.tremorTime = 20;
                Counter = 0;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            npc.lifeMax = (int)(npc.lifeMax * 0.85f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.65f);
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 1, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 300;
                npc.height = 500;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 200; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 1, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for(int num623 = 0; num623 < 400; num623++) {
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 1, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 1, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {

            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Atlas/Atlas_Glow"));
        }

        public override bool PreNPCLoot() {
            MyWorld.downedAtlas = true;
            return true;
        }

        public override void NPCLoot() {
            if(Main.expertMode) {
                npc.DropBossBags();
                return;
            }

            npc.DropItem(ModContent.ItemType<ArcaneGeyser>(), Main.rand.Next(32, 44));
            
            int[] lootTable = {
                ModContent.ItemType<KingRock>(),
                ModContent.ItemType<Mountain>(),
                ModContent.ItemType<TitanboundBulwark>(),
                ModContent.ItemType<CragboundStaff>(),
                ModContent.ItemType<QuakeFist>(),
                ModContent.ItemType<Earthshatter>()
            };
            int loot = Main.rand.Next(lootTable.Length);
            npc.DropItem(lootTable[loot]);

            npc.DropItem(ModContent.ItemType<AtlasMask>(), 1f / 7);
            npc.DropItem(ModContent.ItemType<Trophy8>(), 1f / 10);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            if(npc.ai[0] < 2f)
                return false;

            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.GreaterHealingPotion;
        }
    }
}
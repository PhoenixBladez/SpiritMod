using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
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
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SpiritCore
{
    [AutoloadBossHead]
    public class SpiritCore : ModNPC
    {
        public static int _type;

        int timer = 0;
        int timer1 = 0;
        int timer2 = 0;
        int timer3 = 0;
        int timer4 = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 150f;
        bool text = false;
        bool txt = false;
        bool rotationspawns = false;
        bool rotationspawns1 = false;
        bool mirage = false;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ethereal Umbra");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
        }

        public override void SetDefaults() {
            npc.width = 50;
            npc.height = 50;
            npc.damage = 70;
            npc.defense = 22;
            npc.lifeMax = 7800;
            npc.knockBackResist = 0;

            bossBag = ModContent.ItemType<SpiritCoreBag>();
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ethereal_umbra");
            animationType = NPCID.Pixie;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath6;
        }

        private int Counter;
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            npc.lifeMax = (int)(npc.lifeMax * 0.65f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.75f);
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 187, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {

                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 80;
                npc.height = 80;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 200; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for(int num623 = 0; num623 < 400; num623++) {
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 187, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override bool PreAI() {
            bool expertMode = Main.expertMode;
            {
                npc.spriteDirection = npc.direction;
                Player player = Main.player[npc.target];
                if(npc.Center.X >= player.Center.X && moveSpeed >= -53) // flies to players x position
                    moveSpeed--;

                if(npc.Center.X <= player.Center.X && moveSpeed <= 53)
                    moveSpeed++;

                npc.velocity.X = moveSpeed * 0.1f;

                if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
                {
                    moveSpeedY--;
                    HomeY = 150f;
                }

                if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
                    moveSpeedY++;

                npc.velocity.Y = moveSpeedY * 0.1f;
                if(Main.rand.Next(220) == 6)
                    HomeY = -35f;


                if(!rotationspawns) {
                    for(int I = 0; I < 4; I++) {
                        //cos = y, sin = x
                        int GeyserEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 90) * 100)), (int)(npc.Center.Y + (Math.Cos(I * 90) * 100)), ModContent.NPCType<SpiritRotator>(), npc.whoAmI, 0, 0, 0, -1);
                        NPC Eye = Main.npc[GeyserEye];
                        Eye.ai[0] = I * 90;
                        Eye.ai[3] = I * 90;
                        rotationspawns = true;
                    }

                }
                int npcType1 = ModContent.NPCType<SpiritRotator>();
                bool shadow = false;
                bool spirit = false;
                for(int num569 = 0; num569 < 200; num569++) {
                    if((Main.npc[num569].active && Main.npc[num569].type == (npcType1)))
                        spirit = true;
                }
                if(shadow || spirit)
                    npc.dontTakeDamage = true;

                if(spirit) {
                    npc.defense = 22;
                    timer++;
                    {
                        if(timer == 220) {
                            for(int i = 0; i < 6; ++i) {
                                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 92);
                                Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
                                targetDir.Normalize();
                                targetDir *= 15;
                                int damageAmount = expertMode ? 34 : 50;
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CurvingFlame>(), damageAmount, 0.5F, Main.myPlayer);
                                timer = 0;
                            }
                        }
                        timer1++;
                        if(timer1 >= 1000 && timer <= 1400) //Rains red comets
                        {
                            if(Main.rand.Next(8) == 0) {
                                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                                int A = Main.rand.Next(-200, 200) * 6;
                                int B = Main.rand.Next(-200, 200) - 1000;
                                int damageAmount = expertMode ? 21 : 44;

                                Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 14f, ModContent.ProjectileType<SpiritRainHostile>(), damageAmount, 1, Main.myPlayer, 0, 0);
                            }
                        }
                        if(timer1 == 1500)
                            timer1 = 0;
                    }
                }
                if(!spirit) {
                    if(npc.Center.X >= player.Center.X && moveSpeed >= -68) // flies to players x position
                        moveSpeed--;

                    if(npc.Center.X <= player.Center.X && moveSpeed <= 68)
                        moveSpeed++;

                    npc.velocity.X = moveSpeed * 0.1f;

                    if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -44) //Flies to players Y position
                        moveSpeedY--;
                    HomeY = 160f;

                    if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 44)
                        moveSpeedY++;

                    npc.velocity.Y = moveSpeedY * 0.15f;
                    if(Main.rand.Next(220) == 6)
                        HomeY = -25f;

                    npc.defense = 11;
                    if(!mirage) {
                        int Mirage = NPC.NewNPC((int)(npc.Center.X - 100), (int)(npc.Center.Y), ModContent.NPCType<Mirage>(), npc.whoAmI, 0, 0, 0, -1);
                        mirage = true;
                    }
                    if(!rotationspawns1) {
                        for(int I = 0; I < 2; I++) {
                            //cos = y, sin = x
                            int GeyserEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 180) * 100)), (int)(npc.Center.Y + (Math.Cos(I * 180) * 100)), ModContent.NPCType<ShadowRotator>(), npc.whoAmI, 0, 0, 0, -1);
                            NPC Eye = Main.npc[GeyserEye];
                            Eye.ai[0] = I * 180;
                            Eye.ai[3] = I * 180;
                            rotationspawns1 = true;

                        }
                    }
                    timer2++;
                    if(timer2 == 100) {
                        for(int i = 0; i < 6; ++i) {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 92);
                            Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
                            targetDir.Normalize();
                            targetDir *= 15;
                            int damageAmount = expertMode ? 34 : 60;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<ShadowPulse>(), damageAmount, 0.5F, Main.myPlayer);
                            timer2 = 0;
                        }
                    }
                    if(!text) {
                        Main.NewText("You force me to draw power from the Shadows!", 180, 80, 250, true);
                        text = true;
                    }

                }
                int npcType = ModContent.NPCType<ShadowRotator>();
                for(int num569 = 0; num569 < 200; num569++) {
                    if((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
                        spirit = true;
                }
                if(!spirit) {
                    if(!shadow) {
                        npc.defense = 0;
                        if(npc.Center.X >= player.Center.X && moveSpeed >= -90) // flies to players x position
                            moveSpeed--;

                        if(npc.Center.X <= player.Center.X && moveSpeed <= 90)
                            moveSpeed++;

                        npc.velocity.X = moveSpeed * 0.1f;

                        if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -60) //Flies to players Y position
                        {
                            moveSpeedY--;
                            HomeY = 175f;
                        }

                        if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 60)
                            moveSpeedY++;

                        npc.velocity.Y = moveSpeedY * 0.22f;
                        if(Main.rand.Next(219) == 6)
                            HomeY = -25f;

                        if(!txt) {
                            Main.NewText("I will finish you...", 80, 80, 210, true);
                            txt = true;
                        }

                    }
                    if(Main.rand.Next(19) == 0) {
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                        int A = Main.rand.Next(-200, 200) * 6;
                        int B = Main.rand.Next(-200, 200) - 1000;
                        int damageAmount = expertMode ? 18 : 44;

                        Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 14f, ModContent.ProjectileType<SpiritRainHostile>(), damageAmount, 1, Main.myPlayer, 0, 0);
                    }
                    timer3++;
                    if(timer3 == 100) {
                        for(int i = 0; i < 6; ++i) {
                            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 92);
                            Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
                            targetDir.Normalize();
                            targetDir *= 15;
                            int damageAmount = expertMode ? 34 : 50;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CurvingFlame>(), damageAmount, 0.5F, Main.myPlayer);
                            timer3 = 0;
                        }
                    }
                    npc.dontTakeDamage = false;
                }
                if(Main.rand.Next(6) == 1) {
                    int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 173, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                    int dust1 = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 187, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust1].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust1].noGravity = true;

                }
            }
            return true;
        }

        public override void AI() {
            Player player = Main.player[npc.target];
            if(!player.active || player.dead || Main.dayTime) {
                npc.TargetClosest(false);
                npc.velocity.Y = -2000;
            }
        }


        public override bool PreNPCLoot() {
            MyWorld.downedSpiritCore = true;
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot() {
            if(Main.expertMode) {
                npc.DropBossBags();
                return;
            }

            int[] lootTable = { 
                ModContent.ItemType<SummonStaff>(), 
                ModContent.ItemType<ShadowStaff>(), 
                ModContent.ItemType<WispSword>() 
            };
            int loot = Main.rand.Next(lootTable.Length);
            npc.DropItem(lootTable[loot]);

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<SpiritBar>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<StellarBar>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<Items.Material.Rune>(), Main.rand.Next(3, 6));

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<SoulShred>(), Main.rand.Next(5, 9));

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<DuskStone>(), Main.rand.Next(4, 8));

            if(Main.rand.Next(4) == 1)
                npc.DropItem(ModContent.ItemType<SpiritCrystal>(), Main.rand.Next(3, 6));

            npc.DropItem(Items.Armor.Masks.SpiritCoreMask._type, 1f / 7);
            npc.DropItem(Items.Boss.Trophy10._type, 1f / 10);
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
    }
}

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
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class FelDemon : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fel Demon");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 44;
            npc.height = 60;

            npc.lifeMax = 490;
            npc.defense = 28;
            npc.damage = 60;

            npc.HitSound = SoundID.NPCHit36;
            npc.DeathSound = SoundID.NPCDeath6;

            npc.knockBackResist = 0.1f;
            npc.value = 3000f;

            npc.netAlways = true;
            npc.chaseable = true;
            npc.lavaImmune = true;

            Main.npcFrameCount[npc.type] = 4;
        }

        public override bool PreAI() {
            npc.TargetClosest(true);
            npc.velocity.X = npc.velocity.X * 0.93f;
            if(npc.velocity.X > -0.1F && npc.velocity.X < 0.1F)
                npc.velocity.X = 0;
            if(npc.ai[0] == 0)
                npc.ai[0] = 500f;

            if(npc.ai[2] != 0 && npc.ai[3] != 0) {
                // Teleport effects: away.
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                for(int index1 = 0; index1 < 50; ++index1) {
                    int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 75, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                    Main.dust[newDust].noGravity = true;
                }
                npc.position.X = (npc.ai[2] * 16 - (npc.width / 2) + 8);
                npc.position.Y = npc.ai[3] * 16f - npc.height;
                npc.velocity.X = 0.0f;
                npc.velocity.Y = 0.0f;
                npc.ai[2] = 0.0f;
                npc.ai[3] = 0.0f;
                // Teleport effects: arrived.
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                for(int index1 = 0; index1 < 50; ++index1) {
                    int newDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 75, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                    Main.dust[newDust].noGravity = true;
                }
            }

            ++npc.ai[0];

            if(npc.ai[0] == 100 || npc.ai[0] == 200 || npc.ai[0] == 300) {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }

            bool teleport = false;
            for(int i = 0; i < 255; ++i) {
                if(Main.player[i].active && !Main.player[i].dead && (npc.position - Main.player[i].position).Length() < 160) {
                    teleport = true;
                    break;
                }
            }

            // Teleport
            if(npc.ai[0] >= 500 && Main.netMode != 1) {
                teleport = true;
            }

            if(teleport)
                Teleport();

            if(npc.ai[1] > 0) {
                --npc.ai[1];
                if(npc.ai[1] == 15) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                    if(Main.netMode != 1) {
                        NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.Center.Y - 16, ModContent.NPCType<CursedBall>(), 0, 0, 0, 0, 0, 255);
                    }
                }
            }

            if(Main.rand.Next(3) == 0)
                return false;
            Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 75, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
            dust.noGravity = true;
            dust.velocity.X = dust.velocity.X * 0.3f;
            dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

            return false;
        }

        public void Teleport() {
            npc.ai[0] = 1f;
            int num1 = (int)Main.player[npc.target].position.X / 16;
            int num2 = (int)Main.player[npc.target].position.Y / 16;
            int num3 = (int)npc.position.X / 16;
            int num4 = (int)npc.position.Y / 16;
            int num5 = 20;
            int num6 = 0;
            bool flag1 = false;
            if(Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0) {
                num6 = 100;
                flag1 = true;
            }
            while(!flag1 && num6 < 100) {
                ++num6;
                int index1 = Main.rand.Next(num1 - num5, num1 + num5);
                for(int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2) {
                    if((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive()) {
                        bool flag2 = true;
                        if(Main.tile[index1, index2 - 1].lava())
                            flag2 = false;
                        if(flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1)) {
                            npc.ai[1] = 20f;
                            npc.ai[2] = (float)index1;
                            npc.ai[3] = (float)index2;
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            npc.netUpdate = true;
        }

        public override void FindFrame(int frameHeight) {
            int currShootFrame = (int)npc.ai[1];
            if(currShootFrame >= 25)
                npc.frame.Y = frameHeight;
            else if(currShootFrame >= 20)
                npc.frame.Y = frameHeight * 2;
            else if(currShootFrame >= 15)
                npc.frame.Y = frameHeight * 3;
            else if(currShootFrame >= 10)
                npc.frame.Y = frameHeight * 2;
            else if(currShootFrame >= 5)
                npc.frame.Y = frameHeight;
            else
                npc.frame.Y = 0;

            npc.spriteDirection = npc.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.player.ZoneDungeon && NPC.downedPlantBoss ? 0.09f : 0f;
        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Material.CursedFire>(), Main.rand.Next(1) + 1);
            if(Main.rand.Next(14) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FelBinder>(), 1);
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 75, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 100;
                npc.height = 60;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 30; num621++) {
                    int randomDustType = Main.rand.Next(3);
                    if(randomDustType == 0) {
                        randomDustType = 5;
                    } else if(randomDustType == 1) {
                        randomDustType = 36;
                    } else {
                        randomDustType = 32;
                    }
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 75, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for(int num623 = 0; num623 < 50; num623++) {
                    int randomDustType = Main.rand.Next(3);
                    if(randomDustType == 0) {
                        randomDustType = 5;
                    } else if(randomDustType == 1) {
                        randomDustType = 36;
                    } else {
                        randomDustType = 32;
                    }
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 75, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 75, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
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
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
    public class ShellBane : ModNPC
    {
        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shell Bane");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults() {
            npc.width = 26;
            npc.height = 38;
            npc.damage = 55;
            npc.defense = 5;
            npc.lifeMax = 450;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 329f;
            npc.knockBackResist = 0.06f;
            npc.aiStyle = 3;
            aiType = 508;

        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DepthShard>(), 1);
            if(Main.rand.Next(50) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Clauncher>(), 1);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBossAny)
                return 6.2f;

            return 0;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            {
                timer++;
                if(timer < 200) //Fires desert feathers like a shotgun
                {
                    npc.defense = 1000;

                }

                if(timer >= 200) //sets velocity to 0, creates dust
                {
                    npc.velocity.X = 0f;
                    npc.defense = 0;

                    if(Main.rand.Next(2) == 0) {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, 107);
                        Main.dust[dust].scale = 0.9f;
                    }

                }
                if(timer >= 400) {

                    for(int i = 0; i < 8; ++i) {
                        Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
                        targetDir.Normalize();
                        targetDir *= 3;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<ShellBolt>(), 30, 0.5F, Main.myPlayer);
                    }
                    timer = 0;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 107);
            if(npc.life <= 0) {
                int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, 107);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Clampeye"), 1f);
                if(TideWorld.TheTide) {
                    TideWorld.TidePoints2 += 1;
                }
            }
        }
    }
}


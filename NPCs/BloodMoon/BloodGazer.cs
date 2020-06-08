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

namespace SpiritMod.NPCs.BloodMoon
{
    public class BloodGazer : ModNPC
    {
        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blood Gazer");
        }

        public override void SetDefaults() {
            npc.width = 50;
            npc.height = 68;
            npc.damage = 45;
            npc.defense = 13;
            npc.lifeMax = 800;
            npc.knockBackResist = 0.1f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 3;
            Main.npcFrameCount[npc.type] = 4;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath22;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            if(npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
                moveSpeed--;
            else if(npc.Center.X <= player.Center.X && moveSpeed <= 30)
                moveSpeed++;

            npc.velocity.X = moveSpeed * 0.1f;

            if(npc.Center.Y >= player.Center.Y - 50f && moveSpeedY >= -30) //Flies to players Y position
                moveSpeedY--;
            else if(npc.Center.Y <= player.Center.Y - 50f && moveSpeedY <= 30)
                moveSpeedY++;

            npc.velocity.Y = moveSpeedY * 0.1f;
            npc.ai[0] += 4f;
            if(npc.ai[0] > 96f) {
                npc.ai[0] = 0f;
                int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
                int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
                int num184 = 26;
                if(Main.expertMode) {
                    num184 = 18;
                }
                int bloodproj;
                bloodproj = Main.rand.Next(new int[] { ModContent.ProjectileType<GazerEye>(), mod.ProjectileType("GazerEye1") });
                Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 5f, bloodproj, num184, 0f, Main.myPlayer, 0f, 0f);
                return;
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0 || npc.life >= 0) {
                int d = 5;
                for(int k = 0; k < 25; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
                }
            }

            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer3"), 1f);
                int d = 5;
                for(int k = 0; k < 25; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.97f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 1.27f);
                }
                for(int i = 0; i < 16; i++) {
                    int bloodproj;
                    bloodproj = Main.rand.Next(new int[] { mod.ProjectileType("Feeder1"), mod.ProjectileType("Feeder2"), mod.ProjectileType("Feeder3") });
                    float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                    Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, bloodproj, npc.damage / 2, 1, Main.myPlayer, 0, 0);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].velocity *= 4f;
                }
            }
        }
        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.2f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<BloodGazer>()) ? 0.024f : 0f;
        }
    }
}

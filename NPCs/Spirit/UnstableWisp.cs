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
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
    public class UnstableWisp : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Unstable Wisp");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 32;
            npc.lifeMax = 150;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.friendly = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), };
            return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY < Main.rockLayer ? .1f : 0f;
        }

        public override bool PreAI() {
            bool inRange = false;
            Vector2 target = Vector2.Zero;
            float triggerRange = 280f;
            for(int i = 0; i < 255; i++) {
                if(Main.player[i].active && !Main.player[i].dead) {
                    float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
                    float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
                    float distOrth = Math.Abs(npc.position.X + (float)(npc.width / 2) - playerX) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - playerY);
                    if(distOrth < triggerRange) {
                        if(Main.player[i].Hitbox.Intersects(npc.Hitbox)) {
                            npc.life = 0;
                            npc.HitEffect(0, 10.0);
                            npc.checkDead();
                            npc.active = false;
                            return false;
                        }
                        triggerRange = distOrth;
                        target = Main.player[i].Center;
                        inRange = true;
                    }
                }
            }
            if(inRange) {
                Vector2 delta = target - npc.Center;
                delta.Normalize();
                delta *= 0.95f;
                npc.velocity = (npc.velocity * 10f + delta) * (1f / 11f);
                return false;
            }
            if(npc.velocity.Length() > 0.2f) {
                npc.velocity *= 0.98f;
            }
            return false;
        }

        public override bool CheckDead() {
            Vector2 center = npc.Center;
            Terraria.Projectile.NewProjectile(center.X, center.Y, 0f, 0f, mod.ProjectileType("UnstableWisp_Explosion"), 100, 0f, Main.myPlayer);
            return true;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.10000000149011612;
            if((int)npc.frameCounter >= Main.npcFrameCount[npc.type]) {
                npc.frameCounter -= (double)Main.npcFrameCount[npc.type];
            }
            int num = (int)npc.frameCounter;
            npc.frame.Y = num * frameHeight;
            npc.spriteDirection = npc.direction;
        }
    }
}

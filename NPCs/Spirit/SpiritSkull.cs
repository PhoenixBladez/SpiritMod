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
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
    public class SpiritSkull : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Skull");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 40;
            npc.height = 52;
            npc.damage = 35;
            npc.defense = 10;
            npc.knockBackResist = 0.2f;
            npc.lifeMax = 295;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 0.75f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/SpiritSkull_Glow"));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
                int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
                return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY < Main.rockLayer && !spawnInfo.playerSafe && !spawnInfo.invasion ? 4f : 0f;
            }
            return 0f;
        }


        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI() {
            float velMax = 1f;
            float acceleration = 0.011f;
            npc.TargetClosest(true);
            Vector2 center = npc.Center;
            float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
            float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
            float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
            npc.ai[1] += 1f;
            if((double)npc.ai[1] > 600.0) {
                acceleration *= 8f;
                velMax = 4f;
                if((double)npc.ai[1] > 650.0) {
                    npc.ai[1] = 0f;
                }
            } else if((double)distance < 250.0) {
                npc.ai[0] += 0.9f;
                if(npc.ai[0] > 0f) {
                    npc.velocity.Y = npc.velocity.Y + 0.019f;
                } else {
                    npc.velocity.Y = npc.velocity.Y - 0.019f;
                }
                if(npc.ai[0] < -100f || npc.ai[0] > 100f) {
                    npc.velocity.X = npc.velocity.X + 0.019f;
                } else {
                    npc.velocity.X = npc.velocity.X - 0.019f;
                }
                if(npc.ai[0] > 200f) {
                    npc.ai[0] = -200f;
                }
            }
            if((double)distance > 350.0) {
                velMax = 5f;
                acceleration = 0.3f;
            } else if((double)distance > 300.0) {
                velMax = 3f;
                acceleration = 0.2f;
            } else if((double)distance > 250.0) {
                velMax = 1.5f;
                acceleration = 0.1f;
            }
            float stepRatio = velMax / distance;
            float velLimitX = deltaX * stepRatio;
            float velLimitY = deltaY * stepRatio;
            if(Main.player[npc.target].dead) {
                velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
                velLimitY = (float)((double)(-(double)velMax) / 2.0);
            }
            if(npc.velocity.X < velLimitX) {
                npc.velocity.X = npc.velocity.X + acceleration;
            } else if(npc.velocity.X > velLimitX) {
                npc.velocity.X = npc.velocity.X - acceleration;
            }
            if(npc.velocity.Y < velLimitY) {
                npc.velocity.Y = npc.velocity.Y + acceleration;
            } else if(npc.velocity.Y > velLimitY) {
                npc.velocity.Y = npc.velocity.Y - acceleration;
            }
            if((double)velLimitX > 0.0) {
                npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
            }
            if((double)velLimitX < 0.0) {
                npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
            }
            npc.spriteDirection = -npc.direction;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.09f, 0.4f);
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 13);
                Gore.NewGore(npc.position, npc.velocity, 12);
                Gore.NewGore(npc.position, npc.velocity, 11);
            }
        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SpiritOre>(), Main.rand.Next(3) + 2);
        }

    }
}

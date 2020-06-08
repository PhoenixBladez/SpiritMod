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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class SporeWheezer : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spore Wheezer");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults() {
            npc.width = 40;
            npc.height = 60;
            npc.damage = 20;
            npc.defense = 7;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath53;
            npc.value = 260f;
            npc.knockBackResist = .55f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return (tile == TileID.MushroomGrass) && NPC.downedBoss1 && spawnInfo.spawnTileY > Main.rockLayer ? 3f : 0f;

        }
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 42, hitDirection, -1f, 0, default(Color), .61f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
            }
        }

        public override void NPCLoot() {
            int lootcount = Main.rand.Next(1, 3);
            for(int J = 0; J <= lootcount; J++) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Carapace>());
            }
            int lootcount1 = Main.rand.Next(3, 6);
            for(int J = 0; J <= lootcount1; J++) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GlowingMushroom);
            }
            if(Main.rand.Next(15) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<WheezerScale>());
        }

        int frame = 0;
        int timer = 0;
        int shootTimer = 0;
        public override void AI() {
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.1f, 0.15f);
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 200) {
                npc.velocity = Vector2.Zero;
                if(npc.velocity == Vector2.Zero) {
                    npc.velocity.X = .008f * npc.direction;
                    npc.velocity.Y = 12f;
                }
                float num395 = Main.mouseTextColor / 200f - 0.35f;
                num395 *= 0.2f;
                npc.scale = num395 + 0.95f;
                shootTimer++;
                if(shootTimer >= 90) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 95);
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 5f;
                    direction.Y *= 5f;

                    int amountOfProjectiles = Main.rand.Next(2, 4);
                    bool expertMode = Main.expertMode;
                    int damage = expertMode ? 8 : 13;
                    for(int i = 0; i < amountOfProjectiles; ++i) {
                        float A = (float)Main.rand.Next(-50, 50) * 0.02f;
                        float B = (float)Main.rand.Next(-60, -40) * 0.1f;
                        int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<WheezerSporeHostile>(), damage, 1, Main.myPlayer, 0, 0);
                        for(int k = 0; k < 11; k++) {
                            Dust.NewDust(npc.position, npc.width, npc.height, 42, direction.X + A, direction.Y + B, 0, default(Color), .61f);
                        }
                        Main.projectile[p].hostile = true;
                    }
                    shootTimer = 0;
                }
                frame = 0;
            } else {
                npc.scale = 1f;
                shootTimer = 0;
                npc.aiStyle = 3;
                aiType = NPCID.Skeleton;
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 6) {
                    frame = 0;
                }
            }
            if(shootTimer > 120) {
                shootTimer = 120;
            }
            if(shootTimer < 0) {
                shootTimer = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/SporeWheezer_Glow"));
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
    }
}

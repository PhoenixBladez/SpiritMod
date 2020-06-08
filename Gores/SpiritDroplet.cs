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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
    public class SpiritDroplet : ModGore
    {
        public override void OnSpawn(Gore gore) {
            gore.numFrames = 15;
            gore.behindTiles = true;
            gore.timeLeft = Gore.goreTime * 3;
        }

        public override bool Update(Gore gore) {
            if(gore.position.Y < Main.worldSurface * 16.0 + 8.0) {
                gore.alpha = 0;
            } else {
                gore.alpha = 100;
            }

            gore.frameCounter += 1;

            int frameDuration = 4;
            if(gore.frame <= 4) {
                int tileX = (int)(gore.position.X / 16f);
                int tileY = (int)(gore.position.Y / 16f) - 1;
                if(WorldGen.InWorld(tileX, tileY) && !Main.tile[tileX, tileY].active()) {
                    gore.active = false;
                }

                if(gore.frame == 0 || gore.frame == 1 || gore.frame == 2) {
                    frameDuration = 24 + Main.rand.Next(256);
                }

                if(gore.frame == 3) {
                    frameDuration = 24 + Main.rand.Next(96);
                }

                if(gore.frameCounter >= frameDuration) {
                    gore.frameCounter = 0;
                    gore.frame += 1;
                    if(gore.frame == 5) {
                        int droplet = Gore.NewGore(gore.position, gore.velocity, gore.type);
                        Main.gore[droplet].frame = 9;
                        Main.gore[droplet].velocity *= 0f;
                    }
                }
            } else if(gore.frame <= 6) {
                frameDuration = 8;
                if(gore.frameCounter >= frameDuration) {
                    gore.frameCounter = 0;
                    gore.frame += 1;
                    if(gore.frame == 7) {
                        gore.active = false;
                    }
                }
            } else if(gore.frame <= 9) {
                frameDuration = 6;

                gore.velocity.Y += 0.2f;
                gore.velocity.Y = Utils.Clamp(gore.velocity.Y, 0.5f, 12f);

                if(gore.frameCounter >= frameDuration) {
                    gore.frameCounter = 0;
                    gore.frame += 1;
                }

                if(gore.frame > 9) {
                    gore.frame = 7;
                }
            } else {
                gore.velocity.Y += 0.1f;

                if(gore.frameCounter >= frameDuration) {
                    gore.frameCounter = 0;
                    gore.frame += 1;
                }

                gore.velocity *= 0f;
                if(gore.frame > 14) {
                    gore.active = false;
                }
            }

            Vector2 oldVelocity = gore.velocity;
            gore.velocity = Collision.TileCollision(gore.position, gore.velocity, 16, 14);
            if(gore.velocity != oldVelocity) {
                if(gore.frame < 10) {
                    gore.frame = 10;
                    gore.frameCounter = 0;
                    Main.PlaySound(SoundID.Drip, (int)gore.position.X + 8, (int)gore.position.Y + 8, Main.rand.Next(2));
                }
            } else if(Collision.WetCollision(gore.position + gore.velocity, 16, 14)) {
                if(gore.frame < 10) {
                    gore.frame = 10;
                    gore.frameCounter = 0;
                    Main.PlaySound(SoundID.Drip, (int)gore.position.X + 8, (int)gore.position.Y + 8, 2);
                }

                int tileX = (int)(gore.position.X + 8f) / 16;
                int tileY = (int)(gore.position.Y + 14f) / 16;
                if(Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].liquid > 0) {
                    gore.velocity *= 0f;
                    gore.position.Y = tileY * 16 - (Main.tile[tileX, tileY].liquid / 16);
                }
            }

            gore.position += gore.velocity;

            return false;
        }
    }
}
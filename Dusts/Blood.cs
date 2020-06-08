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
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class Blood : ModDust
    {
        private const int stickTime = 80;
        private const float baseScale = 0.1f;
        private const float growRate = (1 - baseScale) / stickTime;

        public override void OnSpawn(Dust dust) {
            dust.noGravity = true;
        }

        public override bool Update(Dust dust) {
            if(dust.customData != null) {
                if(dust.customData is NPC npc) {
                    dust.customData = new BloodAnchor {
                        anchor = npc,
                        oldRotation = npc.rotation,
                        offset = dust.position - ((NPC)dust.customData).Center
                    };

                    dust.scale = baseScale;
                    dust.velocity = Vector2.Zero;
                }

                if(dust.customData is BloodAnchor) {
                    if(Follow(dust, (BloodAnchor)dust.customData)) {
                        return false;
                    }
                }
            }

            if(Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 6) && dust.fadeIn == 0f) {
                dust.velocity = Vector2.Zero;
            } else {
                dust.scale += 0.009f;
            }

            return true;
        }

        private bool Follow(Dust dust, BloodAnchor follow) {
            NPC npc = follow.anchor;
            if(follow.counter++ >= stickTime || !npc.active) {
                if(npc.active) {
                    dust.velocity = npc.velocity + 0.5f * (follow.offset.RotatedBy(npc.rotation) - follow.offset.RotatedBy(follow.oldRotation));
                }

                dust.customData = null;
                dust.noGravity = false;

                return false;
            }

            dust.scale = baseScale + follow.counter * growRate;
            dust.position = npc.Center + follow.offset.RotatedBy(npc.rotation);
            follow.oldRotation = dust.rotation = npc.rotation;

            return true;
        }
    }

    internal class BloodAnchor
    {
        internal int counter;
        internal Vector2 offset;
        internal NPC anchor;
        internal float oldRotation;
    }
}
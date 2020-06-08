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

namespace SpiritMod
{
    class FOVHelper
    {
        public const float POS_X_DIR = 0f;
        public const float NEG_X_DIR = (float)Math.PI;
        public const float POS_Y_DIR = (float)Math.PI / 2f;
        public const float NEG_Y_DIR = (float)Math.PI / -2f;
        public const float UP = NEG_Y_DIR;
        public const float DOWN = POS_Y_DIR;
        public const float LEFT = NEG_X_DIR;
        public const float RIGHT = POS_X_DIR;

        private bool returnDefault = true;
        private bool defaultValue = false;
        private bool overExtended = false;
        Vector2 origin = Vector2.Zero;

        private bool checkAboveLeft = false;
        private bool verticalLeft = false;
        private float slopeLeft = 0f;

        private bool checkAboveRight = false;
        private bool verticalRight = false;
        private float slopeRight = 0f;

        public void AdjustCone(Vector2 center, float fov, float direction) {
            if(fov >= Math.PI) {
                returnDefault = true;
                defaultValue = true;
                return;
            } else if(fov <= 0d) {
                returnDefault = true;
                defaultValue = false;
                return;
            } else {
                returnDefault = false;
                overExtended = fov > MathHelper.PiOver2 ? true : false;
            }

            origin = center;

            float left = direction + fov * .5f;
            float right = direction - fov * .5f;
            left = MathHelper.WrapAngle(left);
            right = MathHelper.WrapAngle(right);
            slopeLeft = (float)Math.Tan(left);
            slopeRight = (float)Math.Tan(right);
            verticalLeft = float.IsNaN(slopeLeft);
            verticalRight = float.IsNaN(slopeRight);

            if(verticalLeft) {
                checkAboveLeft = left > 0f ? true : false;
            } else {
                checkAboveLeft = Math.Abs(left) > MathHelper.PiOver2 ? true : false;
            }

            if(verticalRight) {
                checkAboveRight = right > 0f ? false : true;
            } else {
                checkAboveRight = Math.Abs(right) > MathHelper.PiOver2 ? false : true;
            }
        }

        public bool IsInCone(Vector2 pos) {
            if(returnDefault) {
                return defaultValue;
            }

            float x = pos.X - origin.X;
            float y = pos.Y - origin.Y;

            if(verticalLeft) {
                if(checkAboveLeft) {
                    if(x >= 0f) {
                        if(overExtended) {
                            return true;
                        }
                    } else {
                        if(!overExtended) {
                            return false;
                        }
                    }
                } else {
                    if(x <= 0f) {
                        if(overExtended) {
                            return true;
                        }
                    } else {
                        if(!overExtended) {
                            return false;
                        }
                    }
                }
            } else {
                if(checkAboveLeft) {
                    if(x * slopeLeft <= y) {
                        if(overExtended) {
                            return true;
                        }
                    } else {
                        if(!overExtended) {
                            return false;
                        }
                    }
                } else {
                    if(x * slopeLeft >= y) {
                        if(overExtended) {
                            return true;
                        }
                    } else {
                        if(!overExtended) {
                            return false;
                        }
                    }
                }
            }

            if(verticalRight) {
                return checkAboveRight ? x >= 0f : x <= 0f;
            } else {
                return checkAboveRight ? x * slopeRight <= y : x * slopeRight >= y;
            }
        }
    }
}

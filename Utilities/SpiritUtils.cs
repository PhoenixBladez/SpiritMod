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
using Terraria.Utilities;

namespace SpiritMod
{
    public static class SpiritUtils
    {
        public static MyPlayer GetSpiritPlayer(this Player player) => player.GetModPlayer<MyPlayer>();

        public static float GetDamageBoost(this Player player) {
            float[] damageTypes = new float[] { player.meleeDamage, player.magicDamage, player.rangedDamage, player.thrownDamage, player.minionDamage };
            return damageTypes.Min();
        }

        public static Vector2 NextVec2CircularEven(this UnifiedRandom rand, float halfWidth, float halfHeight) {
            double x = rand.NextDouble();
            double y = rand.NextDouble();
            if(x + y > 1) {
                x = 1 - x;
                y = 1 - y;
            }

            double s = 1 / (x + y);
            if(double.IsNaN(s)) {
                return Vector2.Zero;
            }

            s *= s;
            s = Math.Sqrt(x * x * s + y * y * s);
            s = 1 / s;

            x *= s;
            y *= s;

            double angle = rand.NextDouble() * (2 * Math.PI);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            return new Vector2((float)(x * cos - y * sin) * halfWidth, (float)(x * sin + y * cos) * halfHeight);
        }

        public static bool LeftOf(this Vector2 point, Vector2 check) {
            return check.X * point.Y - check.Y * point.X < 0;
        }

        public static float SideOfNormalize(this Vector2 point, Vector2 check) {
            float length = check.Length();
            length = (check.X * point.Y - check.Y * point.X) / length;
            return float.IsNaN(length) ? 0f : length;
        }

        public static float SideOf(this Vector2 point, Vector2 checkNorm) {
            return checkNorm.X * point.Y - checkNorm.Y * point.X;
        }

        public static Vector2 TurnRight(this Vector2 vec) {
            return new Vector2(-vec.Y, vec.X);
        }

        public static Vector2 TurnLeft(this Vector2 vec) {
            return new Vector2(vec.Y, -vec.X);
        }

        public static bool Nearing(this Vector2 vec, Vector2 target) {
            return 0 < vec.X * target.X + vec.Y * target.Y;
        }
    }
}

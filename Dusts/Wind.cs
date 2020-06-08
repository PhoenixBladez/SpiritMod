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
    public class Wind : ModDust
    {
        public override void OnSpawn(Dust dust) {
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }

        public override bool Update(Dust dust) {
            if(dust.customData is null) {
                dust.active = false;
            }

            if(!(dust.customData is WindAnchor)) {
                return true;
            }

            if(dust.alpha > 253) {
                dust.active = false;
                return false;
            }

            WindAnchor data = (WindAnchor)dust.customData;
            data.anchor += dust.velocity;
            dust.position = data.anchor + (data.offsetDir * data.offset).RotatedBy(dust.rotation);
            dust.rotation += data.turnRate;

            dust.alpha += 5;
            dust.scale *= .98f;
            dust.velocity *= .95f;
            data.offset += 0.4f;
            data.turnRate *= .92f;

            return false;
        }
    }

    internal class WindAnchor
    {
        public float turnRate;
        public float offset;
        public Vector2 offsetDir;
        public Vector2 anchor;

        public WindAnchor(Vector2 origin, Vector2 velocity, Vector2 position) {
            float length = velocity.Length();

            velocity = velocity * (1f / length);
            if(velocity.HasNaNs()) {
                velocity = new Vector2(0, -1);
            }

            turnRate = 0.06f + Main.rand.NextFloat(0.04f);
            turnRate *= length > 4 ? length : 4;

            bool left = (position - origin).LeftOf(velocity);
            if(left) {
                turnRate = -turnRate;
                offsetDir = -velocity.TurnLeft();
            } else {
                offsetDir = -velocity.TurnRight();
            }

            offset = 2 + Main.rand.NextFloat(2);
            anchor = offsetDir * offset;
            anchor += position;
        }

        public WindAnchor(Vector2 origin, Vector2 position) {
            turnRate = 0.06f + Main.rand.NextFloat(0.04f);
            turnRate *= 6;

            bool left = position.X - origin.X < 0;
            if(left) {
                turnRate = -turnRate;
                offsetDir = new Vector2(1, 0);
            } else {
                offsetDir = new Vector2(-1, 0);
            }

            offset = 2 + Main.rand.NextFloat(2);
            anchor = offsetDir * offset;
            anchor += position;
        }
    }
}
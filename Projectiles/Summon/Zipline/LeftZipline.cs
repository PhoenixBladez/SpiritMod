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
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zipline
{
    public class LeftZipline : ModProjectile
    {
        Vector2 direction9 = Vector2.Zero;
        int timer = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Left Zipline");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.alpha = 0;
        }

        bool chain = false;
        int rightValue;
        int distance = 9999;
        bool stuck = false;
        float alphaCounter;
        public override bool PreAI() {
            alphaCounter += 0.04f;
            if(!stuck) {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            }
            projectile.timeLeft = 50;
            rightValue = (int)projectile.ai[1];
            if(rightValue < (double)Main.projectile.Length && rightValue != 0) {
                Projectile other = Main.projectile[rightValue];
                if(other.active) {
                    direction9 = other.Center - projectile.Center;
                    distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
                    chain = true;
                } else {
                    chain = false;
                }
            } else {
                chain = false;
            }
            if(stuck) {
                projectile.velocity = Vector2.Zero;
            }
            return true;
        }
        public override void AI() {
            if(stuck)
                DoDustEffect(projectile.Center, 18f);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            if(chain && distance < 2000 && stuck) {
                Projectile other = Main.projectile[rightValue];
                direction9 = other.Center - projectile.Center;
                direction9.Normalize();
                //	direction9 *= 6;
                ProjectileExtras.DrawChain(projectile.whoAmI, other.Center,
                "SpiritMod/Projectiles/Summon/Zipline/Zipline_Chain", false, 0, true, direction9.X, direction9.Y);
            }

        }
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null) {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .26f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            if(!stuck) {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 52);
            }
            if(oldVelocity.X != projectile.velocity.X) //if its an X axis collision
                {
                if(projectile.velocity.X > 0) {
                    projectile.rotation = 1.57f;
                } else {
                    projectile.rotation = 4.71f;
                }
            }
            if(oldVelocity.Y != projectile.velocity.Y) //if its a Y axis collision
            {
                if(projectile.velocity.Y > 0) {
                    projectile.rotation = 3.14f;
                } else {
                    projectile.rotation = 0f;
                }
            }
            stuck = true;
            return false;
        }
    }
}
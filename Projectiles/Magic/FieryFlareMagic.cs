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

namespace SpiritMod.Projectiles.Magic
{
    public class FieryFlareMagic : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Slag Flare");
        }

        public override void SetDefaults() {
            projectile.width = 2;
            projectile.height = 2;
            aiType = ProjectileID.Flames;
            projectile.alpha = 255;
            projectile.timeLeft = 120;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.extraUpdates = 18;
        }
        int counter;
        public override void AI() {
            Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
            projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 30));
            projectile.rotation += 0.1f;
            for(int i = 0; i < 2; i++) {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 127, projectile.velocity.X * 1.5f, projectile.velocity.Y * 1.5f);
                Main.dust[dust].scale = .9f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn += .25f;
                int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 127, projectile.velocity.X * 1.5f, projectile.velocity.Y * 1.5f);
                Main.dust[dust1].scale = 1.6f;
                Main.dust[dust1].noGravity = true;
                Main.dust[dust].fadeIn += .25f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            for(int j = 0; j < 8; j++) {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 127, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                Main.dust[dust].scale = .85f;
            }
            if(Main.rand.Next(6) == 2)
                target.AddBuff(BuffID.OnFire, 180);
            if(Main.rand.Next(4) == 0) {
                int n = 4;
                int deviation = Main.rand.Next(0, 300);
                for(int i = 0; i < n; i++) {
                    float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 2.5f;
                    perturbedSpeed.Y *= 2.5f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, 504, projectile.damage / 2, 2, projectile.owner);
                }
            }
        }
    }
}

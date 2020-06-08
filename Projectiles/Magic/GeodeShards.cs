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
    public class GeodeShards : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Geode");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.alpha = 255;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 30;
            projectile.light = 0.0f;
        }

        public override void AI() {
            projectile.rotation += 0.1f;
            projectile.velocity *= 0.95f;

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
            int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135);
            int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust1].noGravity = true;
            Main.dust[dust1].velocity *= 0f;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0f;
        }

        public override void Kill(int timeLeft) {
            if(Main.rand.Next(2) == 0) {
                float rotation = (float)(Main.rand.Next(0, 0) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(projectile.Center, velocity,
                    ProjectileID.CrystalShard, 20, projectile.knockBack, projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].velocity *= 4f;
            }
            if(Main.rand.Next(2) == 0) {
                float rotation = (float)(Main.rand.Next(0, 0) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(projectile.Center, velocity,
                    ProjectileID.CrystalShard, 20, projectile.knockBack, projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].velocity *= 4f;
            }
            if(Main.rand.Next(2) == 0) {
                float rotation = (float)(Main.rand.Next(0, 0) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(projectile.Center, velocity,
                    ProjectileID.CrystalShard, (int)(projectile.damage * .75), projectile.knockBack, projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].velocity *= 4f;
            }

            for(int i = 0; i < 5; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(8) == 0) {
                target.AddBuff(BuffID.CursedInferno, 200, true);
                target.AddBuff(BuffID.OnFire, 200, true);
                target.AddBuff(BuffID.Frostburn, 200, true);
            }
        }

    }
}
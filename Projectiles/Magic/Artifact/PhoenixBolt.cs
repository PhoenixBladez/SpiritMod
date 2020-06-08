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
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic.Artifact
{

    public class PhoenixBolt : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phoenix Bolt");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults() {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 500;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
        }

        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 5f) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            if(Main.rand.Next(4) == 0) {
                for(int k = 0; k < 1; k++) {
                    Vector2 value = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, (Math.PI / 16)), (double)Utils.ToRotation(projectile.velocity));
                    int num9 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num9].velocity *= 0.1f;
                    Main.dust[num9].noGravity = true;
                    Main.dust[num9].position = projectile.Center + value * (float)projectile.width / 2f;
                    Main.dust[num9].fadeIn = 0.9f;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(ModContent.BuffType<Buffs.Artifact.Blaze>(), 300);
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 74);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

            for(int i = 0; i < 40; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Dust expr_62_cp_0 = Main.dust[num];
                expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((Main.rand.Next(-50, 51) * .05f) - 1.5f);
                Dust expr_92_cp_0 = Main.dust[num];
                expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((Main.rand.Next(-50, 51) * .05f) - 1.5f);
                if(Main.dust[num].position != projectile.Center) {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
        }

    }
}
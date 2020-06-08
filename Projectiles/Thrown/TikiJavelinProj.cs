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

namespace SpiritMod.Projectiles.Thrown
{
    public class TikiJavelinProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tiki Javelin");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            projectile.timeLeft = 999999;
        }

        float counter = 3;
        int trailcounter = 0;
        Vector2 holdOffset = new Vector2(0, -3);
        public override bool PreAI() {
            Player player = Main.player[projectile.owner];
            if(player.channel) {
                projectile.position = player.position + holdOffset;
                player.velocity.X *= 0.95f;
                if(counter < 13) {
                    counter += 0.1f;
                }
                Vector2 direction = Main.MouseWorld - (projectile.position);
                direction.Normalize();
                direction *= counter;
                projectile.rotation = direction.ToRotation() - 1.57f;
                if(direction.X > 0) {
                    holdOffset.X = -10;
                    player.direction = 1;
                } else {
                    holdOffset.X = 10;
                    player.direction = 0;
                }
                trailcounter++;
                if(trailcounter % 5 == 0)
                    Projectile.NewProjectile(projectile.Center + (direction * 3), direction, mod.ProjectileType("TikiJavelinProj1"), 0, 0, projectile.owner); //predictor trail, please pick a better dust Yuy
            } else {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 1);
                Vector2 direction = Main.MouseWorld - (projectile.position);
                direction.Normalize();
                direction *= counter;
                Projectile.NewProjectile(projectile.position, direction, mod.ProjectileType("TikiJavelinProj2"), (int)(projectile.damage * Math.Sqrt(counter)), projectile.knockBack, projectile.owner);
                projectile.active = false;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 30;
            player.itemAnimation = 30;
            //	player.itemRotation = 0;
            return true;
        }
    }
}

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

namespace SpiritMod.Projectiles.Magic
{
    public class AdamantiteStaffProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Adamantite Blast");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 50;
        }

        int counter = -720;
        public override bool PreAI() {
            int num = 5;
            for(int k = 0; k < 10; k++) {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 60, 0.0f, 0.0f, 0, new Color(), 1.3f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                Main.dust[index2].scale = .5f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            return true;
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3.75f, 3.75f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -3.75f, -3.75f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);

            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 7.5f, 0f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -7.5f, 0f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 7.5f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -7.5f, mod.ProjectileType("AdamantiteStaffProj2"), projectile.damage, 0f, projectile.owner, 0f, 0f);
        }

    }
}

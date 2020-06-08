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
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class Fae2 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fae Blast");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.alpha = 255;
            projectile.timeLeft = 150;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            for(int i = 0; i < 10; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 26, 26, 242, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].position.X = x;
                Main.dust[num].position.Y = y;
                Main.dust[num].velocity *= 0f;
                Main.dust[num].noGravity = true;
            }
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + 10, projectile.velocity.Y + 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X - 10, projectile.velocity.Y - 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X - 10, projectile.velocity.Y + 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + 10, projectile.velocity.Y - 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y + 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y - 10, ModContent.ProjectileType<Fae>(), 25, projectile.knockBack, projectile.owner, 0f, 0f);
        }
    }
}
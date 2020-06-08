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
    public class AquaSphere : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Aqua Sphere");
        }

        public override void SetDefaults() {
            projectile.width = 34;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 120;
            projectile.alpha = 0;
            projectile.tileCollide = false;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
            for(int i = 0; i < 2; i++) {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
                Main.dust[d].noGravity = true;
            }

            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, -4, mod.ProjectileType("AquaSphere2"), projectile.damage / 2, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, -4, mod.ProjectileType("AquaSphere2"), projectile.damage / 2, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, 4, mod.ProjectileType("AquaSphere2"), projectile.damage / 2, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, 4, mod.ProjectileType("AquaSphere2"), projectile.damage / 2, projectile.knockBack, Main.myPlayer);
        }

        public override void AI() {
            projectile.alpha += 3;
            projectile.scale += .011f;
            if(projectile.alpha >= 160) {
                projectile.Kill();
            }


            for(int i = 0; i < 5; i++) {
                Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)projectile.height, (float)projectile.height) * projectile.scale * 1.45f / 2f;
                int index = Dust.NewDust(projectile.Center + vector2, 0, 0, 187, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index].position = projectile.Center + vector2;
                Main.dust[index].velocity = Vector2.Zero;
                Main.dust[index].noGravity = true;
            }
            Lighting.AddLight(projectile.position, 0.1f, 0.2f, 0.3f);
        }

    }
}
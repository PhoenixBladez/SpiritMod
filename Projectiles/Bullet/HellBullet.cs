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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
    public class HellBullet : ModProjectile
    {
        private int Counter = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hell Bullet");
        }

        public override void SetDefaults() {
            projectile.width = 2;
            projectile.height = 16;
            projectile.aiStyle = 1;
            projectile.extraUpdates = 10;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 240;
            aiType = ProjectileID.Bullet;
        }

        public override void AI() {
            for(int i = 0; i < 10; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 2, 2, 6);
                Main.dust[num].alpha = projectile.alpha;
                Main.dust[num].velocity = Vector2.Zero;
                Main.dust[num].noGravity = true;
            }

            Counter++;
            if(Counter % 35 == 1) {
                for(int i = 0; i < 2; ++i) {
                    int randFire = Main.rand.Next(3);
                    int newProj = Projectile.NewProjectile(projectile.Center, new Vector2(0, -4),
                        ProjectileID.GreekFire1 + randFire, projectile.damage / 2, 0, projectile.owner);
                    Main.projectile[newProj].hostile = false;
                    Main.projectile[newProj].friendly = true;
                }
            }
        }

        public override void Kill(int timeLeft) {
            int n = 2;
            int deviation = Main.rand.Next(0, 300);
            for(int z = 0; z < n; z++) {
                float rotation = MathHelper.ToRadians(270 / n * z + deviation);
                Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed *= 5.5f;
                int newProj = Projectile.NewProjectile(projectile.Center, perturbedSpeed,
                    ProjectileID.GreekFire1, 30, 2, projectile.owner);

                Main.projectile[newProj].hostile = false;
                Main.projectile[newProj].friendly = true;
                for(int i = 0; i < 5; i++) {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
                    Main.dust[dust].noGravity = true;
                }
                Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            }
        }


    }
}

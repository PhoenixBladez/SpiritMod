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
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
    public class ToxicFlaskHostile : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Plague Vial");
        }

        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 14;

            projectile.aiStyle = 2;
            projectile.damage = 10;
            projectile.friendly = false;
            projectile.hostile = true;

            projectile.alpha = 0;
        }
        public override bool PreAI() {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach(var proj in list) {
                if(Main.player[projectile.owner].channel) {
                    if(projectile != proj && proj.friendly) {
                        projectile.Kill();
                    }
                }
            }
            return true;
        }
        public override void Kill(int timeLeft) {

            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 107);
            for(int i = 0; i < 30; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, 0f, -2f, 0, default(Color), 1.2f);
                Main.dust[num].noGravity = true;
                Dust expr_62_cp_0 = Main.dust[num];
                expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                Dust expr_92_cp_0 = Main.dust[num];
                expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                if(Main.dust[num].position != projectile.Center) {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
            int n = 2;
            int deviation = Main.rand.Next(0, 300);
            for(int i = 0; i < n; i++) {
                float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= Main.rand.NextFloat(1.5f, 3.5f);
                perturbedSpeed.Y *= Main.rand.NextFloat(1.5f, 3.5f);
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, 480, projectile.damage, 2, projectile.owner);
                Main.projectile[p].hostile = true;
                Main.projectile[p].friendly = false;
            }
        }
    }
}

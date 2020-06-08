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
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class SpiritStar : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Star");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.timeLeft = 150;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }

        public override void AI() {
            for(int i = 0; i < 10; i++) {
                int num = 5;
                for(int k = 0; k < 6; k++) {
                    int index2 = Dust.NewDust(projectile.position, 4, 4, 110, 0.0f, 0.0f, 0, new Color(), 1f);
                    Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
                    Main.dust[index2].scale = .48f;
                    Main.dust[index2].velocity *= 0f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = false;
                }
            }
            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] == 4f) {
                projectile.localAI[0] = 0f;
                for(int j = 0; j < 12; j++) {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 110, 0f, 0f, 160, new Color(), 1f);
                    Main.dust[num8].scale = .68f;
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                }
            }
        }

        public override void Kill(int timeLeft) {
            int n = 2;
            int deviation = Main.rand.Next(0, 180);
            for(int i = 0; i < n; i++) {
                float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 4.5f;
                perturbedSpeed.Y *= 4.5f;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<StarSoul>(), projectile.damage / 3, 2, projectile.owner);
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 5;
            projectile.height = 5;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            for(int num623 = 0; num623 < 25; num623++) {
                int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), .8f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 1f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0)
                target.AddBuff(ModContent.BuffType<StarFracture>(), 200, true);

            projectile.Kill();
        }
    }
}
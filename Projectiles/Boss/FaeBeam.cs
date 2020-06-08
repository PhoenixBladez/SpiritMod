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

namespace SpiritMod.Projectiles.Boss
{
    public class FaeBeam : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fae Beam");
        }

        int target;

        public override void SetDefaults() {
            projectile.hostile = true;
            projectile.magic = true;
            projectile.width = 4;
            projectile.height = 20;
            projectile.timeLeft = 80;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            projectile.ai[0] += 1f;
            if(projectile.ai[0] > 5f) {
                projectile.velocity.Y = projectile.velocity.Y + 0.01f;
                projectile.velocity.X = projectile.velocity.X * 1.0f;
                projectile.alpha -= 23;
                projectile.scale = 0.8f * (255f - (float)projectile.alpha) / 255f;
                if(projectile.alpha < 0)
                    projectile.alpha = 0;
            }
            if(projectile.alpha >= 255 && projectile.ai[0] > 5f) {
                projectile.Kill();
                return;
            }

            if(Main.rand.Next(4) == 0) {
                int num193 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    62, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num193].position = projectile.Center;
                Main.dust[num193].scale += Main.rand.Next(50) * 0.01f;
                Main.dust[num193].noGravity = true;
                Main.dust[num193].velocity.Y -= 2f;
            }
            if(Main.rand.Next(6) == 0) {
                int num194 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    62, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num194].position = projectile.Center;
                Main.dust[num194].scale += 0.3f + Main.rand.Next(50) * 0.01f;
                Main.dust[num194].noGravity = true;
                Main.dust[num194].velocity *= 0.1f;
            }

            if(projectile.localAI[1] == 0f) {
                projectile.localAI[1] = 1f;
                Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0f);
            }
        }

    }
}
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

namespace SpiritMod.Projectiles.Sword
{
    public class WaterMass : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Water Mass");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults() {
            projectile.width = projectile.height = 30;

            projectile.melee = true;
            projectile.friendly = true;

            projectile.penetrate = 6;
        }

        public override bool PreAI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 3) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
            projectile.rotation = projectile.velocity.ToRotation();

            if(projectile.owner == Main.myPlayer && projectile.timeLeft <= 3) {
                projectile.tileCollide = false;
                projectile.ai[1] = 0f;
                projectile.alpha = 255;

                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 128;
                projectile.height = 128;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.knockBack = 8f;

                projectile.ai[0] = 1;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(ModContent.BuffType<TidalWrath>(), 300);

            if(Main.rand.Next(6) == 0 && projectile.timeLeft > 3) {
                projectile.velocity *= 0f;
                projectile.alpha = 255;
                projectile.timeLeft = 3;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if(Main.rand.Next(6) == 0) {
                projectile.velocity *= 0f;
                projectile.alpha = 255;
                projectile.timeLeft = 3;
                return false;
            }
            return true;
        }

        public override void Kill(int timeLeft) {
            if(!projectile.active)
                return;

            if(projectile.ai[0] != 1) {
                for(int i = 0; i < 15; ++i) {
                    int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[newDust].noGravity = true;
                    Main.dust[newDust].velocity *= 3F;
                    newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[newDust].velocity *= 1.5F;
                }
                return;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 22;
            projectile.height = 22;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            for(int num617 = 0; num617 < 20; num617++) {
                int num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 3.5f);
                Main.dust[num618].noGravity = true;
                Main.dust[num618].velocity *= 7f;
                num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num618].velocity *= 3f;
            }
            projectile.active = false;
        }

    }
}

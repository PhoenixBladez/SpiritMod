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

namespace SpiritMod.Projectiles.Magic
{
    public class FloranSpore : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Floran Spore");
        }

        public override void SetDefaults() {
            projectile.width = 30;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 180;
            projectile.alpha = 0;
            projectile.extraUpdates = 1;
        }
        public float counter = -1440;
        bool stopped = false;
        public override void AI() {
            projectile.ai[0]++;
            if(projectile.velocity.Length() >= 0.1) {
                if(projectile.velocity.X > 0)
                    projectile.velocity.X -= 0.2f;
                else if(projectile.velocity.X < 0)
                    projectile.velocity.X += 0.2f;

                if(projectile.velocity.Y > 0)
                    projectile.velocity.Y -= 0.2f;
                else if(projectile.velocity.Y < 0)
                    projectile.velocity.Y += 0.2f;
                if(projectile.velocity.Length() <= 0.1f) {
                    projectile.velocity = Vector2.Zero;
                    stopped = true;
                }
                if(!stopped) {
                    if(Main.rand.Next(5) == 0) {
                        int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 39);
                        Main.dust[d].scale *= 0.42f;
                    }
                    for(int i = 0; i < 6; i++) {
                        float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                        float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                        int num = Dust.NewDust(projectile.Center, 6, 6, 39, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[num].velocity *= .1f;
                        Main.dust[num].scale *= .9f;
                        Main.dust[num].noGravity = true;

                    }
                }
                if(projectile.ai[0] % 2 == 0)
                    projectile.alpha += 3;
                if(projectile.alpha >= 250)
                    projectile.Kill();
            }
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 20; i++) {
                int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 39, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
                Main.dust[d].scale *= 0.42f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<VineTrap>(), 180);
        }

    }
}
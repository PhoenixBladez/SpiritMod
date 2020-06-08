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
    public class DepthBullet : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Depth Round");
        }

        public override void SetDefaults() {
            projectile.width = 2;
            projectile.height = 20;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
            aiType = ProjectileID.Bullet;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(15) == 0) {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
                ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                    delegate {
                        for(int i = 0; i < 40; i++) {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                                15, 0f, -2f, 0, default(Color), 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            if(Main.dust[num].position != projectile.Center)
                                Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    });
            }
        }

        public override bool PreAI() {
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15);
            Main.dust[dust].scale = 1.3f;
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;
            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15);
            Main.dust[dust1].scale = 1.3f;
            Main.dust[dust1].velocity = Vector2.Zero;
            Main.dust[dust1].noGravity = true;
            Main.dust[dust1].noLight = true;

            return false;
        }

    }
}

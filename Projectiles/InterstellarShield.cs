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
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class InterstellarShield : ModProjectile
    {
        public bool shooting = false;
        double dist = 80;
        Vector2 direction = Vector2.Zero;
        int counter = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("InterstellarShield");
        }
        public override void SetDefaults() {
            projectile.penetrate = 600;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.timeLeft = 2;
            projectile.damage = 1;
            projectile.extraUpdates = 1;
            projectile.alpha = 255;
            projectile.width = projectile.height = 24;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;

        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);
            for(int k = 0; k < 15; k++) {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 180, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
        }
        public override void AI() {
            Player player = Main.player[projectile.owner];
            //Factors for calculations
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach(var proj in list) {
                if(proj.hostile && proj.active && proj.timeLeft > 2) {
                    if(proj.damage < 65) {
                        counter++;
                        proj.timeLeft = 2;
                        Main.PlaySound(SoundID.Item93, projectile.position);
                        proj.active = false;
                    } else {
                        counter += 5;
                    }
                }
            }
            if(counter >= 2) {
                projectile.active = false;
                ((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).shieldsLeft -= 1;
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
            }
            Vector2 center = projectile.Center;
            float num8 = (float)player.miscCounter / 60f;
            float num7 = 1.0471975512f * 2;
            for(int i = 0; i < 3; i++) {
                int num6 = Dust.NewDust(center, 0, 0, 180, 0f, 0f, 100, default(Color), 1.3f);
                Main.dust[num6].noGravity = true;
                Main.dust[num6].velocity = Vector2.Zero;
                Main.dust[num6].noLight = true;
                Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
            }

            double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians

            /*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            projectile.ai[1] += .38f;
            if(((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).ShieldCore) {
                projectile.timeLeft = 2;
            }
            projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
            projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;
            direction = player.Center - projectile.Center;
            direction.Normalize();
        }
    }
}
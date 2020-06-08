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

namespace SpiritMod.Projectiles.Sword
{
    public class PrincessSickle : ModProjectile
    {
        int timer = 0;
        bool launch = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Princess Sickle");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.light = 0.5f;
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 210;
            projectile.friendly = true;
            projectile.damage = 90;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
            for(int num623 = 0; num623 < 25; num623++) {
                int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 168, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num622].noGravity = true;
                Main.dust[num622].velocity *= 1.5f;
                Main.dust[num622].scale = 1.5f;
            }
        }

        public override void AI() {
            if(timer < 150) {
                projectile.velocity *= 0.97f;
            }
            timer++;
            Vector2 targetPos = projectile.Center;
            float targetDist = 1000f;
            bool targetAcquired = false;

            //loop through first 200 NPCs in Main.npc
            //this loop finds the closest valid target NPC within the range of targetDist pixels
            for(int i = 0; i < 200; i++) {
                if(Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)) {
                    float dist = projectile.Distance(Main.npc[i].Center);
                    if(dist < targetDist) {
                        targetDist = dist;
                        targetPos = Main.npc[i].Center;
                        targetAcquired = true;
                    }
                }
            }

            //change trajectory to home in on target
            projectile.rotation += 0.2f;
            if(timer > 100) {
                projectile.rotation += 0.2f;
                int num622 = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X, projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height, 168, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num622].noGravity = true;
                Main.dust[num622].scale = 1.5f;

            }
            if(targetAcquired && timer > 150 && !launch) {
                float homingSpeedFactor = 25f;
                Vector2 homingVect = targetPos - projectile.Center;
                homingVect.Normalize();
                homingVect *= homingSpeedFactor;

                projectile.velocity = homingVect;
                launch = true;
            }


            Vector3 RGB = new Vector3(1.5f, 0f, 1f);
            float multiplier = 1;
            float max = 2.25f;
            float min = 1.0f;
            RGB *= multiplier;
            if(RGB.X > max) {
                multiplier = 0.5f;
            }
            if(RGB.X < min) {
                multiplier = 1.5f;
            }
            Lighting.AddLight(projectile.position, RGB.X, RGB.Y, RGB.Z);
        }

        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
        //    for (int k = 0; k < projectile.oldPos.Length; k++)
        //    {
        //        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
        //        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
        //        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return true;
        //}
    }
}
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

namespace SpiritMod.Projectiles.Hostile
{
    public class MadHat : ModProjectile
    {
        int timer = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mad Hat");
            Main.projFrames[base.projectile.type] = 5;
        }

        public override void SetDefaults() {
            projectile.hostile = true;
            projectile.width = 28;
            projectile.height = 18;
            projectile.timeLeft = 225;
            projectile.light = 0.5f;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.penetrate = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            projectile.Kill();
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            for(int k = 0; k < 15; k++) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 6) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            Vector3 RGB = new Vector3(0f, 0.5f, 1.5f);
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

            timer++;
            int range = 650;   //How many tiles away the projectile targets NPCs

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            foreach(Player player in Main.player) {
                //if npc is a valid target (active, not friendly, and not a critter)
                if(player.active) {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(player.Center);
                    if(dist / 16 < range) {
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = player.whoAmI;
                        }
                    }
                }
            }

            if(timer < 125) {
                projectile.velocity.Y *= 0.98f;
                if(timer > 100) {
                    int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                    Main.dust[dust].noGravity = true;
                }
            }
            int index = (int)projectile.ai[1];
            if(timer == 125 && index >= 0 && index < Main.maxPlayers && Main.player[index].active) {
                Vector2 direction9 = Main.player[(int)projectile.ai[1]].Center - projectile.Center;
                direction9.Normalize();
                direction9.X *= 15f;
                direction9.Y *= 15f;
                projectile.velocity = direction9;
            }
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
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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
    public class CoilRocket : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Coiled Rocket");
        }

        public override void SetDefaults() {
            projectile.width = 18;       //projectile width
            projectile.height = 24;  //projectile height
            projectile.friendly = false;      //make that the projectile will not damage you
            projectile.hostile = true;        // 
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
            projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
        }
        int numHits;
        public override bool PreAI() {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach(var proj in list) {
                if(projectile != proj && proj.friendly) {
                    numHits++;
                    if(numHits >= 3) {
                        projectile.Kill();
                    }
                }
            }
            return true;
        }
        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] == 16f) {
                projectile.localAI[0] = 0f;
                for(int j = 0; j < 12; j++) {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 226, 0f, 0f, 160, new Color(), 1f);
                    Main.dust[num8].scale = .48f;
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                }
            }
            float num = 1f - (float)projectile.alpha / 255f;
            num *= projectile.scale;

            float num1 = 6f;
            float num2 = 3f;
            float num3 = 16f;
            num1 = 9f;
            num2 = 5.5f;
            if(projectile.timeLeft > 30 && projectile.alpha > 0)
                projectile.alpha -= 25;
            if(projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                projectile.alpha = 128;
            if(projectile.alpha < 0)
                projectile.alpha = 0;

            if(++projectile.frameCounter > 4) {
                projectile.frameCounter = 0;
                if(++projectile.frame >= 4)
                    projectile.frame = 0;
            }
            float num4 = 0.5f;
            if(projectile.timeLeft < 120)
                num4 = 1.1f;
            if(projectile.timeLeft < 60)
                num4 = 1.6f;

            ++projectile.ai[1];
            double num5 = (double)projectile.ai[1] / 180.0;

            int index1 = (int)projectile.ai[0];
            if(index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead) {
                if(projectile.Distance(Main.player[index1].Center) <= num3)
                    return;
                Vector2 unitY = projectile.DirectionTo(Main.player[index1].Center);
                if(unitY.HasNaNs())
                    unitY = Vector2.UnitY;
                projectile.velocity = (projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
            } else {
                if(projectile.timeLeft > 30)
                    projectile.timeLeft = 30;
                if(projectile.ai[0] == -1f)
                    return;
                projectile.ai[0] = -1f;
                projectile.netUpdate = true;
            }
            int num1222 = 5;
            for(int k = 0; k < 3; k++) {
                int index2 = Dust.NewDust(projectile.position, 1, 1, 6, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].position = projectile.Center - projectile.velocity / num1222 * (float)k;
                Main.dust[index2].scale = .95f;
                Main.dust[index2].velocity *= 0f;
                Main.dust[index2].noGravity = true;
                Main.dust[index2].noLight = false;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(BuffID.OnFire, 300);
            projectile.Kill();
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            for(int i = 0; i < 40; i++) {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 1.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                if(Main.dust[num].position != projectile.Center) {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
        }

    }
}

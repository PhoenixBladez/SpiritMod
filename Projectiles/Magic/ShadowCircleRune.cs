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
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.Dusking;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class ShadowCircleRune : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadow Circle Rune");
        }

        public override void SetDefaults() {
            projectile.width = 86;
            projectile.height = 80;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.aiStyle = -1;
            projectile.timeLeft = 3600;
            projectile.sentry = true;
        }

        public override bool PreAI() {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Shadowflame, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

            projectile.rotation += 0.05f * projectile.direction;
            return true;
        }

        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 30) {
                projectile.frameCounter = 0;
                float num = 8000f;
                int num2 = -1;
                for(int i = 0; i < 200; i++) {
                    float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
                    if(num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(projectile, false)) {
                        num2 = i;
                        num = num3;
                    }
                }

                if(num2 != -1) {
                    bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
                    if(flag) {
                        Vector2 value = Main.npc[num2].Center - projectile.Center;
                        float num4 = 9f;
                        float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
                        if(num5 > num4)
                            num5 = num4 / num5;

                        value *= num5;
                        int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<NPCs.Boss.Dusking.CrystalShadow>(), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
                        Main.projectile[p].friendly = true;
                        Main.projectile[p].hostile = false;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(2) == 0)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }

    }
}

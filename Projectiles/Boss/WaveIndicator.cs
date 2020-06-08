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
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
    public class WaveIndicator : ModProjectile
    {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crystal Fragment");
        }

        public override void SetDefaults() {
            projectile.aiStyle = -1;
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.timeLeft = 280;
        }

        public override void AI() {
            int num1 = ModContent.NPCType<AncientFlyer>();
            float num2 = 210f;
            float x = 0.08f;
            float y = 0.1f;
            int Damage = 0;
            float num3 = 0.0f;
            bool flag1 = true;
            bool flag2 = false;
            bool flag3 = false;
            if((double)projectile.ai[0] < (double)num2) {
                bool flag4 = true;
                int index1 = (int)projectile.ai[1];
                if(Main.npc[index1].active && Main.npc[index1].type == num1) {
                    if(!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                } else {
                    projectile.ai[0] = num2;
                    flag4 = false;
                }
                if(flag4 && !flag2) {
                    projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                }
            }
            for(int i = 0; i < 5; i++) {
                if(projectile.width == 8) {
                    float x1 = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y1 = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                    int num = Dust.NewDust(new Vector2(x1, y1), 2, 2, 172);
                    Main.dust[num].velocity = Vector2.Zero;
                    Main.dust[num].noGravity = true;
                }
            }

        }
        public override void Kill(int timeLeft) {
            if(Main.rand.Next(2) == 0) {
                int d = 172;
                for(int k = 0; k < 6; k++) {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
                }

                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
                Dust.NewDust(projectile.position, projectile.width, projectile.height, d, 1.5f * Main.rand.Next(-2, 2), -2.5f, 0, Color.Green, 0.7f);
                projectile.velocity *= 0f;
                projectile.width = 40;
                projectile.knockBack = 0;
            }
        }
    }
}

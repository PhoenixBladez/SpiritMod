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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
    public class MoltenGold : ModProjectile
    {
        private int DamageAdditive;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Molten Gold");
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
            projectile.width = 25;
            projectile.height = 25;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.hostile = true;
            projectile.penetrate = 1;
        }
        int counter = -1440;
        public override void AI() {
            counter++;
            if(counter >= 1440) {
                counter = -1440;
            }
            for(int i = 0; i < 10; i++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                int num = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= .1f;
                Main.dust[num].scale *= .9f;
                Main.dust[num].noGravity = true;

            }
            for(int f = 0; f < 10; f++) {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)f;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)f;

                int num = Dust.NewDust(projectile.Center - new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, DustID.GoldCoin, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= .1f;
                Main.dust[num].scale *= .9f;
                Main.dust[num].noGravity = true;

            }
            for(int j = 0; j < 6; j++) {

                int num2 = Dust.NewDust(projectile.Center, 6, 6, 244, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num2].velocity *= 0f;
                Main.dust[num2].scale *= .6f;
                Main.dust[num2].noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(BuffID.OnFire, 240, true);
        }
    }
}

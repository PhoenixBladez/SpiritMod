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
using SpiritMod.NPCs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class AngelWrath : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Angel's Wrath");
        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {
            GNPC info = npc.GetGlobalNPC<GNPC>();
            if(info.angelWrathStacks < 5) {
                info.angelWrathStacks++;
            }
            return true;
        }
        int lightnum;
        public override void Update(NPC npc, ref int buffIndex) {
            GNPC info = npc.GetGlobalNPC<GNPC>();

            {
                int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 112);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].scale *= .35f * info.angelWrathStacks;
                Main.dust[dust].noGravity = true;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 34f;
                Main.dust[dust].position = npc.Center - vector2_3;
            }
            if(info.angelWrathStacks <= 1) {
                lightnum = 1;
            }
            if(info.angelWrathStacks <= 2 || info.angelWrathStacks <= 3) {
                lightnum = 2;
            }
            if(info.angelWrathStacks >= 4) {
                lightnum = 3;
            }
            if(npc.buffTime[buffIndex] == 1) {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 9);
                for(int k = 0; k < lightnum; k++) {
                    float num12 = Main.rand.Next(-10, 10);
                    float num13 = 100;
                    if((double)num13 < 0.0) num13 *= -1f;
                    if((double)num13 < 20.0) num13 = 20f;
                    float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                    float num15 = 10 / num14;
                    float num16 = num12 * num15;
                    float num17 = num13 * num15;
                    float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.15f;  //this defines the projectile X position speed and randomnes
                    float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.15f;  //this defines the projectile Y position speed and randomnes
                    int proj = Projectile.NewProjectile(npc.position.X, npc.position.Y + Main.rand.Next(-400, -380), SpeedX, SpeedY, ModContent.ProjectileType<AngelLightStar>(), 65, 0, Main.myPlayer, 0.0f, 1);
                    info.angelWrathStacks = 0;
                }
            }
        }
    }
}

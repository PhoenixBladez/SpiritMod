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
    public class SunFlower : ModNPC
    {
        int timer = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Solar Flower");
        }

        public override void SetDefaults() {
            npc.width = 38;
            npc.height = 26;
            npc.damage = 18;
            npc.defense = 0;
            npc.lifeMax = 60;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
            npc.knockBackResist = .1f;
            npc.aiStyle = 0;
            aiType = NPCID.BoundGoblin;
            animationType = NPCID.BoundGoblin;
        }

        public override void AI() {
            Player player = Main.player[npc.target];
            npc.velocity *= 0.95f;
            float num801 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
            float num802 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
            float num803 = (float)Math.Atan2((double)num802, (double)num801) + MathHelper.PiOver2;
            if(num803 < 0f)
                num803 += (2 * MathHelper.Pi);
            else if(num803 > (2 * MathHelper.Pi))
                num803 -= (2 * MathHelper.Pi);

            float num804 = 0.1f;
            if(npc.rotation < num803) {
                if(num803 - npc.rotation > MathHelper.Pi)
                    npc.rotation -= num804;
                else
                    npc.rotation += num804;
            } else if(npc.rotation > num803) {
                if(npc.rotation - num803 > MathHelper.Pi)
                    npc.rotation += num804;
                else
                    npc.rotation -= num804;
            }

            if(npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
                npc.rotation = num803;

            if(npc.rotation < 0f)
                npc.rotation += 2 * MathHelper.Pi;
            else if(npc.rotation > 2 * MathHelper.Pi)
                npc.rotation -= 6.283f;

            if(npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
                npc.rotation = num803;

            timer++;
            bool expertMode = Main.expertMode;
            if(timer == 100 || timer == 300 || timer == 600) {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 91);
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 5f;
                direction.Y *= 5f;

                int amountOfProjectiles = 1;
                for(int i = 0; i < amountOfProjectiles; ++i) {
                    float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                    float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                    int damage = expertMode ? 6 : 12;
                    Projectile.NewProjectile(npc.Center, direction,
                        ModContent.ProjectileType<SolarBeamHostile>(), damage, 0, Main.myPlayer);
                }
            } else if(timer == 700)
                timer = 0;
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 30;
                npc.height = 30;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 20; num621++) {
                    int num622 = Dust.NewDust(npc.position, npc.width, npc.height,
                        3, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
        }

    }
}
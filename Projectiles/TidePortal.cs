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
using SpiritMod.Tide.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class TidePortal : ModProjectile
    {
        bool start = true;
        // USE THIS DUST: 261

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tidal Portal");
        }

        public override void SetDefaults() {
            projectile.width = projectile.height = 360;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;

            projectile.penetrate = -1;

            projectile.timeLeft = 700;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
            NPC parent = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Rylheian>())];
            Player player = Main.player[parent.target];
            Vector2 direction8 = player.Center - projectile.Center;
            direction8.Normalize();
            direction8.X *= 22f;
            direction8.Y *= 22f;
            {
                Vector2 dir = player.Center - projectile.Center;
                dir.Normalize();
                dir *= 14;
                int spiritdude = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, ModContent.NPCType<Tentacle>(), parent.target, 0, 0, 0, -1);
                NPC Spirits = Main.npc[spiritdude];
                Spirits.ai[0] = dir.X;
                Spirits.ai[1] = dir.Y;
            }
        }

        public override bool PreAI() {
            if(start) {
                for(int num621 = 0; num621 < 55; num621++) {
                    int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0f, 0f, 206, default(Color), 2f);

                }

                projectile.ai[1] = projectile.ai[0];
                start = false;
            }
            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0f, 0f, 206, default(Color), 2f);
            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0f, 0f, 206, default(Color), 2f);
            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0f, 0f, 206, default(Color), 2f);
            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0f, 0f, 206, default(Color), 2f);
            projectile.rotation = projectile.rotation + 3f;

            //Making player variable "p" set as the projectile's owner
            float lowestDist = float.MaxValue;
            NPC parent = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Rylheian>())];
            Player player = Main.player[parent.target]; // 
            if((projectile.ai[1] / 2) % 75 == 1) {
                Vector2 dir = player.Center - projectile.Center;
                dir.Normalize();
                dir *= 14;
                int spiritdude = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, ModContent.NPCType<Tentacle>(), parent.target, 0, 0, 0, -1);
                NPC Spirits = Main.npc[spiritdude];
                Spirits.ai[0] = dir.X;
                Spirits.ai[1] = dir.Y;
            }

            //Factors for calculations
            double deg = (double)projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 500; //Distance away from the player

            /*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
    		/distance for the desired distance away from the player minus the projectile's width   /
    		/and height divided by two so the center of the projectile is at the right place.     */
            projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width / 2;
            projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            projectile.ai[1] += 2f;

            return false;
        }


    }
}

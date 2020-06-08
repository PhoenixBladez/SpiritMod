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

namespace SpiritMod.Projectiles.Summon
{
    public class Primavore : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Primavore");
            Main.projFrames[base.projectile.type] = 7;
        }

        public override void SetDefaults() {
            projectile.width = 84;
            projectile.height = 76;
            projectile.timeLeft = 3000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.minionSlots = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void AI() {


            /*projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }*/

            projectile.velocity.Y = 5;
            //CONFIG INFO
            int range = 50;   //How many tiles away the projectile targets NPCs
            float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
            int shootSpeed = 50;

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            for(int i = 0; i < 200; ++i) {
                NPC npc = Main.npc[i];
                //if npc is a valid target (active, not friendly, and not a critter)
                if(npc.active && npc.CanBeChasedBy(projectile)) {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(npc.Center);
                    if(dist / 16 < range) {
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = npc.whoAmI;
                        }
                    }
                }
            }
            NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
                                                                         //firing
            projectile.ai[0]++;
            #region frame
            Vector2 direction2 = target.Center - projectile.Center;
            float actualangle = ((float)Math.Atan(direction2.X / direction2.Y) * (float)(180.0 / Math.PI)) + 90f;
            // string SlopeText = actualangle.ToString();
            //Main.NewText(SlopeText, Color.Orange.R, Color.Orange.G, Color.Orange.B);
            if(actualangle < 11.5f)
                projectile.frame = 0;
            else if(actualangle < 22.5f)
                projectile.frame = 1;
            else if(actualangle < 67.5f)
                projectile.frame = 2;
            else if(actualangle < 112.5f)
                projectile.frame = 3;
            else if(actualangle < 157.5f)
                projectile.frame = 4;
            else if(actualangle < 168.5f)
                projectile.frame = 5;
            else
                projectile.frame = 6;
            #endregion

            if(projectile.ai[0] % shootSpeed == 4 && target.active && projectile.Distance(target.Center) / 16 < range) {
                Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
                Vector2 direction = target.Center - ShootArea;
                direction.Normalize();
                direction.X *= shootVelocity;
                direction.Y *= shootVelocity;

                int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, 483, projectile.damage, 0, Main.myPlayer);
                Main.PlaySound(2, projectile.Center, 5);
            }
        }

    }
}
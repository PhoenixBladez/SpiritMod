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

namespace SpiritMod.Projectiles.Magic
{
    public class FrostFlake : ModProjectile
    {
        int target;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Frost Flake");

            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults() {
            projectile.width = projectile.height = 16;

            projectile.penetrate = 6;

            projectile.magic = true;
            projectile.friendly = true;
            ProjectileID.Sets.Homing[projectile.type] = true;

            projectile.timeLeft = 300;
        }

        public override bool PreAI() {
            if(projectile.ai[0] == 0) {
                projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
                projectile.ai[0] = 1;
            } else {
                projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

                if(projectile.ai[1] == 0 && Main.netMode != 1) {
                    target = -1;
                    float distance = 320;
                    for(int k = 0; k < 200; k++) {
                        if(Main.npc[k].active && Main.npc[k].CanBeChasedBy(projectile, false) && Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[k].Center, 1, 1)) {
                            Vector2 center = Main.npc[k].Center;
                            float currentDistance = Vector2.Distance(center, projectile.Center);
                            if(currentDistance < distance || target == -1) {
                                distance = currentDistance;
                                target = k;
                            }
                        }
                    }

                    if(target != -1) {
                        projectile.ai[1] = 1;
                        projectile.netUpdate = true;
                    }
                } else {
                    NPC targetNPC = Main.npc[this.target];
                    if(!targetNPC.active || !targetNPC.CanBeChasedBy(projectile, false) || !Collision.CanHitLine(projectile.Center, 1, 1, targetNPC.Center, 1, 1)) {
                        this.target = -1;
                        projectile.ai[1] = 0;
                        projectile.netUpdate = true;
                    } else {
                        float currentRot = projectile.velocity.ToRotation();
                        Vector2 direction = targetNPC.Center - projectile.Center;
                        float targetAngle = direction.ToRotation();
                        if(direction == Vector2.Zero)
                            targetAngle = currentRot;

                        float desiredRot = currentRot.AngleLerp(targetAngle, 0.04f);
                        projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot, default(Vector2));
                    }
                }
            }

            if(projectile.timeLeft <= 30)
                projectile.Opacity -= 0.032F;
            return false;
        }

        public override void AI() {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
        }

        public override void SendExtraAI(System.IO.BinaryWriter writer) {
            writer.Write(this.target);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader) {
            this.target = reader.Read();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.Frostburn, 200, true);
        }

    }
}

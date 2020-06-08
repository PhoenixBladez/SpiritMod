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

namespace SpiritMod.Projectiles
{
    public class Flay : ModProjectile
    {
        int target;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mind Sizzler");
        }

        public override void SetDefaults() {
            projectile.hostile = true;
            projectile.magic = true;
            projectile.width = 22;
            projectile.height = 30;
            projectile.timeLeft = 60;
            projectile.friendly = false;
            projectile.aiStyle = 1;
            projectile.tileCollide = false;
        }

        public override bool PreAI() {
            projectile.rotation = projectile.velocity.ToRotation() + 1.57F;

            if(projectile.ai[0] == 0 && Main.netMode != 1) {
                target = -1;
                float distance = 2000f;
                for(int k = 0; k < 255; k++) {
                    if(Main.player[k].active && !Main.player[k].dead) {
                        Vector2 center = Main.player[k].Center;
                        float currentDistance = Vector2.Distance(center, projectile.Center);
                        if(currentDistance < distance || target == -1) {
                            distance = currentDistance;
                            target = k;
                        }
                    }
                }
                if(target != -1) {
                    projectile.ai[0] = 1;
                    projectile.netUpdate = true;
                }
            } else {
                Player targetPlayer = Main.player[this.target];
                if(!targetPlayer.active || targetPlayer.dead) {
                    this.target = -1;
                    projectile.ai[0] = 0;
                    projectile.netUpdate = true;
                } else {
                    float currentRot = projectile.velocity.ToRotation();
                    Vector2 direction = targetPlayer.Center - projectile.Center;
                    float targetAngle = direction.ToRotation();
                    if(direction == Vector2.Zero) {
                        targetAngle = currentRot;
                    }

                    float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
                    projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot, default(Vector2));
                }
            }

            if(projectile.timeLeft <= 60)
                projectile.alpha -= 4;

            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 135, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].scale = 0.5f;

            return false;
        }

    }
}
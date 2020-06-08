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
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
    public class CoreCrusher : ModProjectile
    {
        int timer;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Core Crusher");
        }

        public override void SetDefaults() {
            projectile.width = 28;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.aiStyle = 15;
        }


        public override bool PreAI() {
            ProjectileExtras.FlailAI(projectile.whoAmI);
            timer++;
            if(timer >= 60) {
                float lowestDist = float.MaxValue;
                for(int i = 0; i < 200; ++i) {
                    NPC npc = Main.npc[i];
                    //if npc is a valid target (active, not friendly, and not a critter)
                    if(npc.active && npc.CanBeChasedBy(projectile)) {
                        //if npc is within 50 blocks
                        float dist = projectile.Distance(npc.Center);
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = npc.whoAmI;
                        }
                    }
                }

                NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC());
                Vector2 direction = target.Center - projectile.Center;
                direction.Normalize();
                direction.X *= 8f;
                direction.Y *= 8f;
                int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, 85, projectile.damage, 1, projectile.owner, 0, 0);
                Projectile newProj2 = Main.projectile[proj2];
                newProj2.friendly = true;
                newProj2.hostile = false;
                newProj2.timeLeft = 30;
                timer = 0;
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(24, 240);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
                "SpiritMod/Projectiles/Flail/CoreCrusher_Chain");
            ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
            return false;
        }

    }
}
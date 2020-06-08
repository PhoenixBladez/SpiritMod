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

namespace SpiritMod.Projectiles.DonatorItems
{
    public class ElectricityBolt : ModProjectile
    {
        NPC[] hit = new NPC[8];
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Electricity Bolt");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.melee = true;
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 8;
            projectile.alpha = 255;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.extraUpdates = 7;
        }



        private int Mode {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        private NPC Target {
            get { return Main.npc[(int)projectile.ai[1]]; }
            set { projectile.ai[1] = value.whoAmI; }
        }

        private Vector2 Origin {
            get { return new Vector2(projectile.localAI[0], projectile.localAI[1]); }
            set {
                projectile.localAI[0] = value.X;
                projectile.localAI[1] = value.Y;
            }
        }

        public override void AI() {
            if(Mode == 0) {
                Origin = projectile.position;
                Mode = 1;
            } else {
                if(Mode == 2) {
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                }
                Trail(Origin, projectile.position);
                Origin = projectile.position;
            }
        }

        private void Trail(Vector2 from, Vector2 to) {
            float distance = Vector2.Distance(from, to);
            float step = 1 / distance;
            for(float w = 0; w < distance; w += 4) {
                Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 226, Vector2.Zero).noGravity = true;
            }
        }

        private bool CanTarget(NPC target) {
            foreach(var npc in hit)
                if(target == npc)
                    return false;
            return true;
        }

        private NPC TargetNext(NPC current) {
            float range = 16 * 14;
            range *= range;
            NPC target = null;
            var center = projectile.Center;
            for(int i = 0; i < 200; ++i) {
                NPC npc = Main.npc[i];
                //if npc is a valid target (active, not friendly, and not a critter)
                if(npc != current && npc.active && npc.CanBeChasedBy(null) && CanTarget(npc)) {
                    float dist = Vector2.DistanceSquared(center, npc.Center);
                    if(dist < range) {
                        range = dist;
                        target = npc;
                    }
                }
            }
            return target;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.velocity = Vector2.Zero;
            hit[projectile.penetrate - 1] = target;

            target = TargetNext(target);
            if(target != null)
                projectile.Center = target.Center;
            else
                projectile.Kill();
        }
    }
}
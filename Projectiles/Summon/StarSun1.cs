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

namespace SpiritMod.Projectiles.Summon
{
    public class StarSun1 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Orion's Star");
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.minion = true;
            projectile.width = 20;
            projectile.timeLeft = 150;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.minionSlots = 1;
        }

        public override void AI() {
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 226, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = 1.3f;
            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
            Main.dust[dust1].noGravity = true;
            Main.dust[dust1].scale = 1.3f;
            //CONFIG INFO
            int range = 59;   //How many tiles away the projectile targets NPCs
            float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
            int shootSpeed = 60;

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
            if(projectile.ai[0] % shootSpeed == 10 && target.active && projectile.Distance(target.Center) / 16 < range) {

                for(int I = 0; I < 2; I++) {
                    Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
                    Vector2 direction = target.Center - ShootArea;
                    direction.Normalize();
                    direction.X *= shootVelocity;
                    direction.Y *= shootVelocity;
                    int proj2 = Projectile.NewProjectile(projectile.Center.X + (Main.rand.Next(3, 4)), projectile.Center.Y + (Main.rand.Next(3, 4)), direction.X, direction.Y, ModContent.ProjectileType<StarBeam>(), projectile.damage, 0, Main.myPlayer);
                }
            }
        }

    }
}

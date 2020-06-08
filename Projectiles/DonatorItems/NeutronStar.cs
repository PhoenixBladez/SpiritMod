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
    public class NeutronStar : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Neutron Star");
            Main.projFrames[base.projectile.type] = 10;
        }

        public override void SetDefaults() {
            projectile.width = 80;
            projectile.height = 112;
            projectile.timeLeft = 3000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.minionSlots = 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

        public override void AI() {
            //projectile.velocity.Y = 5;
            //CONFIG INFO
            int range = 25;   //How many tiles away the projectile targets NPCs
            int animSpeed = -4;  //how many game frames per frame :P note: firing anims are twice as fast currently
            float shootVelocity = 7f; //magnitude of the shoot vector (speed of arrows shot)

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            foreach(NPC npc in Main.npc) {
                //if npc is a valid target (active, not friendly, and not a critter)
                if(npc.active && !npc.friendly && npc.catchItem == 0) {
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
            if(projectile.frame < 9) {
                //do nuffin... until target in range
                if(target.active && projectile.Distance(target.Center) / 16 < range) {
                    projectile.frameCounter++;
                    //proceed if rotated in the right direction
                    if((float)projectile.frameCounter >= 6f) {
                        projectile.frame++;
                        projectile.frameCounter = 0;
                    }
                    //proceed if rotated in the right direction

                } else {
                    projectile.frameCounter++;
                    if((float)projectile.frameCounter >= 6f) {
                        projectile.frame = ((projectile.frame + 1) % 5) + 2;
                        projectile.frameCounter = 0;
                    }
                }
            }
            //firing
            else if(projectile.frame == 9) {
                projectile.frameCounter++;
                //fire!!
                if(projectile.frameCounter % animSpeed == 0) {
                    //spawn the arrow centered on the bow (this code aligns the centers :3)
                    Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y);
                    Vector2 direction = target.Center - ShootArea;
                    direction.Normalize();
                    direction.X *= shootVelocity;
                    direction.Y *= shootVelocity;
                    int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, 580, projectile.damage, .5f, projectile.owner, direction.ToRotation(), Main.rand.Next(100));
                    Projectile newProj2 = Main.projectile[proj2];
                    newProj2.friendly = true;
                    newProj2.hostile = false;
                    projectile.frame = 1;


                    Main.PlaySound(SoundLoader.customSoundType, projectile.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/Thunder"));

                    projectile.frame++;
                }
            }

            //finish firing anim, revert back to 0
            if(projectile.frame == 10) {
                projectile.frame = 1;
                projectile.frameCounter = 0;
            }
        }

    }
}
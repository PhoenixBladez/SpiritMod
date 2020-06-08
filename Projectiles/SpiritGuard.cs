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
    public class SpiritGuard : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Guard");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults() {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = -1;
            projectile.timeLeft = 700;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override bool PreAI() {
            projectile.tileCollide = false;
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f);
            Main.dust[dust].scale = .5f;
            Main.dust[dust].noGravity = true;
            return true;
        }

        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 4) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }

            Player player = Main.player[projectile.owner];
            projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 40 : -65), player.position.Y);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)

            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] >= 10f) {
                projectile.localAI[0] = 0f;
                int num416 = 0;
                int num417 = 0;
                float num418 = 0f;
                int num419 = projectile.type;
                for(int num420 = 0; num420 < 1000; num420++) {
                    if(Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
                        num416++;
                        if(Main.projectile[num420].ai[1] > num418) {
                            num417 = num420;
                            num418 = Main.projectile[num420].ai[1];
                        }
                    }
                }
                if(num416 > 1) {
                    Main.projectile[num417].netUpdate = true;
                    Main.projectile[num417].ai[1] = 36000f;
                    return;
                }
            }

            int range = 80;   //How many tiles away the projectile targets NPCs
            float shootVelocity = 5f; //magnitude of the shoot vector (speed of arrows shot)
            int shootSpeed = 80;

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
            if(player.statLifeMax >= player.statLifeMax2 / 2) {//firing
                projectile.ai[0]++;
                if(projectile.ai[0] % shootSpeed == 2 && target.active && projectile.Distance(target.Center) / 16 < range) {
                    Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y);
                    Vector2 direction = target.Center - ShootArea;
                    direction.Normalize();
                    direction.X *= shootVelocity;
                    direction.Y *= shootVelocity;
                    int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<StarSoul>(), 45, 1, Main.myPlayer);

                }
            } else {
                projectile.ai[0]++;
                if(projectile.ai[0] % shootSpeed == 1 && target.active && projectile.Distance(target.Center) / 16 < range) {
                    Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y);
                    Vector2 direction = target.Center - ShootArea;
                    direction.Normalize();
                    direction.X *= shootVelocity;
                    direction.Y *= shootVelocity;
                    int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<StarSoul>(), 55, 2, Main.myPlayer);

                }
            }

        }
    }
}

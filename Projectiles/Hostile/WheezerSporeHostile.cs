using System.Linq;
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


namespace SpiritMod.Projectiles.Hostile
{
    public class WheezerSporeHostile : ModProjectile
    {
        int moveSpeed = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spore");
        }

        public override void SetDefaults() {
            projectile.hostile = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.timeLeft = 1000;
            ;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.penetrate = 1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            projectile.Kill();
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
            for(int k = 0; k < 15; k++) {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 42);
                Main.dust[dust].scale = .61f;
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool PreAI() {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach(var proj in list) {
                if(projectile != proj && proj.friendly) {
                    projectile.Kill();
                }
            }
            return true;
        }
        public override void AI() {
            int range = 650;   //How many tiles away the projectile targets NPCs
                               //int targetingMax = 20; //how many frames allowed to target nearest instead of shooting
                               //float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
            foreach(Player player in Main.player) {
                //if npc is a valid target (active, not friendly, and not a critter)
                if(player.active) {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(player.Center);
                    if(dist / 16 < range) {
                        //if npc is closer than closest found npc
                        if(dist < lowestDist) {
                            lowestDist = dist;

                            //target this npc
                            projectile.ai[1] = player.whoAmI;
                        }
                    }
                }
            }

            Player target = (Main.player[(int)projectile.ai[1]] ?? new Player());
            if(target.active && projectile.Distance(target.Center) / 16 < range && projectile.timeLeft < 945) {
                if(projectile.Center.X >= target.Center.X && moveSpeed >= -30) // flies to players x position
                {
                    moveSpeed--;
                }

                if(projectile.Center.X <= target.Center.X && moveSpeed <= 30) {
                    moveSpeed++;
                }

                projectile.velocity.X = moveSpeed * 0.1f;
                projectile.velocity.Y = 1.4f;
            }
        }
    }
}
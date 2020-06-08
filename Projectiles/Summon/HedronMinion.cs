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
    public class HedronMinion : ModProjectile
    {
        float localaione = 0;
        float localaizero = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hedron");
            Main.projFrames[base.projectile.type] = 8;
        }

        public override void SetDefaults() {
            projectile.width = 22;
            projectile.height = 46;
            projectile.timeLeft = 10000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = 12;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.minionSlots = 0;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate {
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<SpiritBoom>(), (int)(projectile.damage), 0, Main.myPlayer);
                    for(int i = 0; i < 15; i++) {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
                    }
                });
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void AI() {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 4) {
                projectile.frame++;
                projectile.frameCounter = 0;
                if(projectile.frame >= 4)
                    projectile.frame = 0;
            }

            if(localaizero == 0f) {
                localaizero = projectile.Center.Y;
                projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
            }
            if(projectile.Center.Y >= localaizero) {
                localaione = -1f;
                projectile.netUpdate = true;
            }
            if(projectile.Center.Y <= localaizero - 25f) {
                localaione = 1f;
                projectile.netUpdate = true;
            }
            projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.05f * localaione, -2f, 2f);
        }

    }
}
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
    public class DrakomireFlame : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Drakomire Flame");
        }

        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft = 60;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.hide = true;
        }

        public override bool PreAI() {
            if(projectile.velocity.Y < 8f)
                projectile.velocity.Y += 0.1f;


            Vector2 next = projectile.position + projectile.velocity;
            Tile inside = Main.tile[((int)projectile.position.X + (projectile.width >> 1)) >> 4, ((int)projectile.position.Y + (projectile.height >> 1)) >> 4];
            if(inside.active() && Main.tileSolid[inside.type]) {
                projectile.position.Y -= 16f;
                return false;
            }

            if(Collision.WetCollision(next, projectile.width, projectile.height)) {
                if(Main.player[projectile.owner].waterWalk) {
                    projectile.velocity.Y = 0f;
                } else {
                    projectile.timeLeft = 0;
                }
            }

            int num = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height, 6, (float)Main.rand.Next(-3, 4), (float)Main.rand.Next(-3, 4), 100, default(Color), 1f);
            Dust dust = Main.dust[num];
            dust.position.X = dust.position.X - 2f;
            dust.position.Y = dust.position.Y + 2f;
            dust.scale += (float)Main.rand.Next(50) * 0.01f;
            dust.noGravity = true;
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(24, 60);
        }
    }
}

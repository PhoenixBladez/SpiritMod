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
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
    class SpectreBullet : ModProjectile
    {
        public static int _type;

        public override string Texture => SpiritMod.EMPTY_TEXTURE;

        public const float MAX_ANGLE_CHANGE = (float)Math.PI / 12;
        public const float ACCELERATION = 0.5f;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spectre Bullet");
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.height = 6;
            projectile.width = 6;
            projectile.alpha = 255;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
        }

        public override void AI() {
            if(projectile.ai[1] == 0) {
                projectile.ai[0] = -1;
                projectile.ai[1] = projectile.velocity.Length();
            }

            bool chasing = true;
            NPC target = null;
            if(projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxNPCs) {
                target = ProjectileExtras.FindCheapestNPC(projectile.Center, projectile.velocity, ACCELERATION, MAX_ANGLE_CHANGE);
            } else {
                target = Main.npc[(int)projectile.ai[0]];
                if(!target.active || !target.CanBeChasedBy()) {
                    target = ProjectileExtras.FindCheapestNPC(projectile.Center, projectile.velocity, ACCELERATION, MAX_ANGLE_CHANGE);
                }
            }

            if(target == null) {
                chasing = false;
                projectile.ai[0] = -1f;
            } else {
                projectile.ai[0] = (float)target.whoAmI;
                ProjectileExtras.HomingAI(this, target, projectile.ai[1], ACCELERATION);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(!target.chaseable || target.lifeMax <= 5 || target.dontTakeDamage || target.friendly || target.immortal)
                return;

            if(Main.rand.Next(100) <= 35) {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, 305, 0, 0f, projectile.owner, projectile.owner, 1);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            int max = (int)(projectile.ai[1] * .4f);
            Vector2 offset = projectile.velocity * ((projectile.extraUpdates + 1f) / max);
            float deviation = projectile.ai[1] * .125f;
            float vX = projectile.velocity.X * (projectile.extraUpdates + 1);
            float vY = projectile.velocity.Y * (projectile.extraUpdates + 1);
            for(int i = 0; i < max; i++) {
                Vector2 position = projectile.Center - offset * i;
                int dust = Dust.NewDust(position, 0, 0, 187, vX * .25f - vY * .08f, vY * .25f + vX * .08f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 0.9f;
                dust = Dust.NewDust(position, 0, 0, 187, vX * .25f + vY * .08f, vY * .25f - vX * .08f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 0.9f;
            }
        }
    }
}

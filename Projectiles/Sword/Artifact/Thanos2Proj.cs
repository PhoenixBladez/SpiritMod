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
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword.Artifact
{
    public class Thanos2Proj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Thanos Afterimage");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = 20;
            projectile.height = 22;
            projectile.aiStyle = 113;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
            projectile.extraUpdates = 1;
            projectile.light = 0;
            aiType = ProjectileID.Shuriken;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            projectile.penetrate--;
            if(projectile.penetrate <= 0)
                projectile.Kill();
            else {
                aiType = ProjectileID.Shuriken;
                if(projectile.velocity.X != oldVelocity.X) {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if(projectile.velocity.Y != oldVelocity.Y) {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                projectile.velocity *= 0.75f;
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(15) == 0)
                target.AddBuff(ModContent.BuffType<Crystallize>(), 240, true);
        }

        public override void AI() {
            projectile.tileCollide = true;
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, ModContent.DustType<Dusts.Crystal>(), 0f, 0f);
            Main.dust[dust2].scale = 1.0f;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0f;
        }

        public override void Kill(int timeLeft) {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 0);
            int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.Crystal>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[newDust].scale = 2f;
            int newDust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.Crystal>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[newDust1].scale = 2f;
            int newDust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.Crystal>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[newDust2].scale = 2f;
            int newDust3 = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.Crystal>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[newDust3].scale = 2f;
        }

    }
}

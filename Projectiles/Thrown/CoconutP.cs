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
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
    public class CoconutP : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Coconut");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        private bool cracky = false;
        public override void SetDefaults() {
            projectile.width = 24;
            projectile.height = 24;
            //projectile.aiStyle = 8;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 1;
            projectile.light = 0;
            //		aiType = ProjectileID.ThrowingKnife;
        }
        public override void AI() {
            projectile.rotation += .2f;
            projectile.velocity.X *= .98f;
            if(projectile.velocity.Y < 0) {
                projectile.velocity.Y *= 0.95f;
            }
            if(!cracky)
                projectile.velocity.Y += 0.2f;
            if(projectile.velocity.Y > 7f && !cracky) {
                projectile.damage = (int)(projectile.damage * 2.85f);
                cracky = true;
            }
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 7);
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 2);
            Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 2);
            if(cracky) {
                Vector2 GoreVel = projectile.velocity;
                GoreVel.X = 2f;
                GoreVel.Y *= -0.2f;
                Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 1);
                Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutGore1"), 1f);
                GoreVel.X = -2f;
                Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutGore2"), 1f);
            } else {
                Vector2 GoreVel = projectile.velocity;
                if(projectile.velocity.X > 0) {
                    GoreVel.X = 2f;
                } else {
                    GoreVel.X = 0f;
                }
                GoreVel.Y *= -0.2f;
                int g = Gore.NewGore(projectile.position, GoreVel, mod.GetGoreSlot("Gores/Coconut/CoconutGore"), 1f);
                Main.gore[g].timeLeft = 40;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(cracky) {
                projectile.position.Y -= 20;
                crit = true;
                target.AddBuff(BuffID.Confused, 200);
            }
        }
        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
        //    for (int k = 0; k < projectile.oldPos.Length; k++)
        //    {
        //        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
        //        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
        //        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return true;
        //}

    }
}
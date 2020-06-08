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
using SpiritMod.Items.Weapon.Thrown;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
    class MechKnifeProj : ModProjectile
    {
        private int lastFrame = 0;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mechanical Scrap");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            projectile.penetrate = 1;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            aiType = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.StrikeNPC(projectile.damage, 0f, 0, crit);

            if(Main.rand.Next(2) == 0)
                target.AddBuff(BuffID.OnFire, 300, true);
        }

        public override void Kill(int timeLeft) {
            if(Main.rand.Next(0, 4) == 0)
                Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ModContent.ItemType<MechKnife>(), 1, false, 0, false, false);

            for(int i = 0; i < 5; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }

        public override void AI() {
            if(projectile.ai[0] == 0) {
                projectile.ai[0] = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
            } else if(lastFrame > 0) {
                lastFrame++;
                if(lastFrame > 4)
                    projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            //Pre-compute some values to improve performance.
            Texture2D texture = Main.projectileTexture[projectile.type];
            float divisor = 1f / (float)projectile.oldPos.Length;
            Color preColor = projectile.GetAlpha(lightColor) * divisor;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            drawOrigin += new Vector2(0f, projectile.gfxOffY);

            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] + drawOrigin;
                drawPos -= Main.screenPosition;
                Color color = preColor * (float)(projectile.oldPos.Length - k);
                spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            lastFrame = 1;
            projectile.tileCollide = false;
            return false;
        }

    }
}

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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class PossessedDagger : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Possessed Dagger");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.width = 6;
            projectile.height = 12;

            projectile.magic = true;
            projectile.friendly = true;
            projectile.timeLeft = 600;

            projectile.penetrate = -1;
        }
        bool strike = false;
        public override bool PreAI() {
            Lighting.AddLight(projectile.Center, 0.230f, .035f, .06f);
            strike = true;
            if(projectile.ai[0] == 0)
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            else {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num996 = 15;
                bool flag52 = false;
                bool flag53 = false;
                projectile.localAI[0] += 1f;
                if(projectile.localAI[0] % 30f == 0f)
                    flag53 = true;

                int num997 = (int)projectile.ai[1];
                if(projectile.localAI[0] >= (float)(60 * num996))
                    flag52 = true;
                else if(num997 < 0 || num997 >= 200)
                    flag52 = true;
                else if(Main.npc[num997].active && !Main.npc[num997].dontTakeDamage) {
                    projectile.Center = Main.npc[num997].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if(flag53) {
                        Main.npc[num997].HitEffect(0, 1.0);
                    }
                } else
                    flag52 = true;

                if(flag52)
                    projectile.Kill();
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.ai[0] = 1f;
            projectile.ai[1] = (float)target.whoAmI;
            target.AddBuff(ModContent.BuffType<BCorrupt>(), projectile.timeLeft);
            if(strike) {
                target.StrikeNPC(10, 0f, 0, crit);
            }
            projectile.velocity = (target.Center - projectile.Center) * 0.75f;
            projectile.netUpdate = true;
            projectile.damage = 0;

            int num31 = 3;
            Point[] array2 = new Point[num31];
            int num32 = 0;

            for(int n = 0; n < 1000; n++) {
                if(n != projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI) {
                    array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
                    if(num32 >= array2.Length)
                        break;
                }
            }

            if(num32 >= array2.Length) {
                int num33 = 0;
                for(int num34 = 1; num34 < array2.Length; num34++) {
                    if(array2[num34].Y < array2[num33].Y)
                        num33 = num34;
                }
                Main.projectile[array2[num33].X].Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void Kill(int timeLeft) {
            for(int i = 0; i < 5; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 5);
            }
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
        }

    }
}

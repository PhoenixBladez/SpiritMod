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
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
    public class GraniteMaceProj : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Unstable Colonnade");
        }

        public override void SetDefaults() {
            projectile.width = 40;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
        }

        public override bool PreAI() {
            Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10;

            Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 0, default(Color), .31f)];
            newDust.position = position;
            newDust.velocity = projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
            newDust.position += projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2));
            newDust.fadeIn = 0.5f;
            newDust.noGravity = true;
            newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 0, default(Color), .31f)];
            newDust.position = position;
            newDust.velocity = projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + projectile.velocity / 4;
            newDust.position += projectile.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
            newDust.fadeIn = 0.5F;
            newDust.noGravity = true;
            ProjectileExtras.FlailAI(projectile.whoAmI);
            return false;
        }
        public static void DrawChain(int index, Vector2 to, string chainPath) {
            Texture2D texture = ModContent.GetTexture(chainPath);
            Projectile projectile = Main.projectile[index];
            Vector2 vector = projectile.Center;
            Rectangle? sourceRectangle = null;
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num = (float)texture.Height;
            Vector2 vector2 = to - vector;
            float rotation = (float)Math.Atan2((double)vector2.Y, (double)vector2.X) - 1.57f;
            bool flag = true;
            if(float.IsNaN(vector.X) && float.IsNaN(vector.Y)) {
                flag = false;
            }
            if(float.IsNaN(vector2.X) && float.IsNaN(vector2.Y)) {
                flag = false;
            }

            while(flag) {
                if((double)vector2.Length() < (double)num + 1.0) {
                    flag = false;
                } else {
                    Vector2 value = vector2;
                    value.Normalize();
                    vector += value * num;
                    vector2 = to - vector;
                    Color color = Color.White;
                    color = Color.White;
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
                "SpiritMod/Projectiles/Flail/GraniteMace_Chain");

            ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(target.life <= 0) {
                if(projectile.friendly && !projectile.hostile) {
                    ProjectileExtras.Explode(projectile.whoAmI, 30, 30,
                    delegate {
                    });

                }
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                {
                    for(int i = 0; i < 20; i++) {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if(Main.dust[num].position != projectile.Center)
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
                int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y,
                    0, 0, mod.ProjectileType("GraniteSpike1"), projectile.damage / 2, projectile.knockBack, projectile.owner);
                Main.projectile[proj].timeLeft = 2;
            }
        }

    }
}

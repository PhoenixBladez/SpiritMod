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
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
    public class InfernalBeam : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Infernal Beam");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults() {
            projectile.width = 18;
            projectile.height = 18;
            projectile.alpha = 255;

            projectile.penetrate = -1;

            projectile.friendly = true;
            projectile.tileCollide = false;
            //projectile.updatedNPCImmunity = true;
        }

        public override bool PreAI() {
            Player player = Main.player[projectile.owner];

            if(projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            if(player.active && !player.dead && player.channel && !player.CCed && !player.noItems) {
                projectile.Center = player.Center;
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
                projectile.timeLeft = 2;

                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = (float)Math.Atan2((projectile.velocity.Y * projectile.direction), (projectile.velocity.X * projectile.direction));

                projectile.ai[0]++;
                if(projectile.ai[0] >= 10) {
                    if(!player.CheckMana(player.inventory[player.selectedItem].mana, true, false)) {
                        projectile.Kill();
                        return false;
                    }
                    projectile.ai[0] = 0;
                }
            } else {
                projectile.Kill();
                return false;
            }

            if(projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            float rot = projectile.velocity.ToRotation();
            projectile.rotation = rot + 1.57F;
            projectile.velocity = rot.ToRotationVector2();
            int num811 = 2;
            float scaleFactor7 = 0f;
            Vector2 value37 = projectile.Center;

            float[] array3 = new float[num811];
            int num812 = 0;
            while((float)num812 < num811) {
                float num813 = (float)num812 / (num811 - 1);
                Vector2 value38 = value37 + projectile.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * (num813 - 0.5f) * scaleFactor7 * projectile.scale;
                int num814 = (int)value38.X / 16;
                int num815 = (int)value38.Y / 16;
                Vector2 vector69 = value38 + projectile.velocity * 16f * 150f;
                int num816 = (int)vector69.X / 16;
                int num817 = (int)vector69.Y / 16;
                Tuple<int, int> tuple;
                float num818;
                if(!Collision.TupleHitLine(num814, num815, num816, num817, 0, 0, new List<Tuple<int, int>>(), out tuple)) {
                    num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;
                } else if(tuple.Item1 == num816 && tuple.Item2 == num817) {
                    num818 = 2400f;
                } else {
                    num818 = new Vector2((float)Math.Abs(num814 - tuple.Item1), (float)Math.Abs(num815 - tuple.Item2)).Length() * 16f;
                }
                array3[num812] = num818;
                num812++;
            }

            float num819 = 0f;
            for(int num820 = 0; num820 < array3.Length; num820++) {
                num819 += array3[num820];
            }
            num819 /= num811;
            float amount = 0.5f;
            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num819, amount);

            Vector2 vector72 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
            /*for (int num826 = 0; num826 < 2; num826++)
            {
                float num827 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num828 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector73 = new Vector2((float)Math.Cos((double)num827) * num828, (float)Math.Sin((double)num827) * num828);
                int num829 = Dust.NewDust(vector72, 0, 0, 229, vector73.X, vector73.Y, 0, default(Color), 1f);
                Main.dust[num829].noGravity = true;
                Main.dust[num829].scale = 1.7f;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 value40 = projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
                int num830 = Dust.NewDust(vector72 + value40 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num830].velocity *= 0.5f;
                Main.dust[num830].velocity.Y = -Math.Abs(Main.dust[num830].velocity.Y);
            }*/
            DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            //Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            float tmp = 0f;
            if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 30f * projectile.scale, ref tmp)) {
                return true;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            //projectile.npcImmune[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor) {
            if(projectile.velocity == Vector2.Zero)
                return false;

            Texture2D tex2 = Main.projectileTexture[projectile.type];
            float num210 = projectile.localAI[1];
            Microsoft.Xna.Framework.Color c_ = new Microsoft.Xna.Framework.Color(255, 255, 255, 127);
            Vector2 value20 = projectile.Center.Floor();
            num210 -= projectile.scale * 10.5f;
            Vector2 vector41 = new Vector2(projectile.scale);
            DelegateMethods.f_1 = 1f;
            DelegateMethods.c_1 = c_;
            DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));
            DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw));

            return false;
        }

    }
}

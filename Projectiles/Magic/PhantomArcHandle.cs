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

namespace SpiritMod.Projectiles.Magic
{
    public class PhantomArcHandle : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Phantom Arc");
        }

        public override void SetDefaults() {
            projectile.width = 14;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
        }
        float alphaCounter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition), null, new Color((int)(2.5f * sineAdd), (int)(5.5f * sineAdd), (int)(6f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
        }
        public override bool PreAI() {
            alphaCounter += 0.04f;
            Player player = Main.player[projectile.owner];
            float num = MathHelper.PiOver2;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);

            float num26 = 30f;
            if(projectile.ai[0] > 120f)
                num26 = 5f;
            else if(projectile.ai[0] > 90f)
                num26 = 15f;

            projectile.damage = (int)(player.inventory[player.selectedItem].damage * player.magicDamage);
            projectile.ai[0]++;
            projectile.ai[1]++;
            bool flag9 = false;
            if(projectile.ai[0] % num26 == 0f)
                flag9 = true;

            int num27 = 10;
            bool flag10 = false;
            if(projectile.ai[0] % num26 == 0f)
                flag10 = true;

            if(projectile.ai[1] >= 1f) {
                projectile.ai[1] = 0f;
                flag10 = true;
                if(Main.myPlayer == projectile.owner) {
                    float scaleFactor5 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                    Vector2 value12 = vector;
                    Vector2 value13 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - value12;
                    if(player.gravDir == -1f)
                        value13.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - value12.Y;

                    Vector2 vector11 = Vector2.Normalize(value13);
                    if(vector11.HasNaNs())
                        vector11 = -Vector2.UnitY;

                    vector11 = Vector2.Normalize(Vector2.Lerp(vector11, Vector2.Normalize(projectile.velocity), 0.92f));
                    vector11 *= scaleFactor5;
                    if(vector11.X != projectile.velocity.X || vector11.Y != projectile.velocity.Y)
                        projectile.netUpdate = true;

                    projectile.velocity = vector11;
                }
            }

            int num28 = (projectile.ai[0] < 120f) ? 4 : 1;
            if(projectile.soundDelay <= 0) {
                projectile.soundDelay = num27;
                projectile.soundDelay *= 2;
                if(projectile.ai[0] != 1f)
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 15);

            }

            if(flag10 && Main.myPlayer == projectile.owner) {
                bool flag11 = !flag9 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
                bool flag12 = player.channel && flag11 && !player.noItems && !player.CCed;
                if(flag12) {
                    if(projectile.ai[0] == 1f) {
                        Vector2 center3 = projectile.Center;
                        Vector2 vector12 = Vector2.Normalize(projectile.velocity);
                        if(vector12.HasNaNs())
                            vector12 = -Vector2.UnitY;

                        int num29 = projectile.damage;
                        Projectile.NewProjectile(center3.X, center3.Y, vector12.X, vector12.Y, ModContent.ProjectileType<PhantomArc>(),
                               num29, projectile.knockBack, projectile.owner, 0, projectile.whoAmI);
                        projectile.netUpdate = true;
                    }
                } else {
                    projectile.Kill();
                }
            }

            if(projectile.localAI[0] >= 120) {
                projectile.Kill();
                return false;
            }

            projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f;
            projectile.rotation = projectile.velocity.ToRotation() + num;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
            return false;
        }

    }
}

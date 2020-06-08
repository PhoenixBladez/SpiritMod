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

namespace SpiritMod.Projectiles
{
    public class CoilMine : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Coil Mine");
        }
        public override void SetDefaults() {
            ///for reasons, I have to put a comment here.
            aiType = ProjectileID.StickyGrenade;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 180;
            projectile.width = 10;
            projectile.CloneDefaults(ProjectileID.StickyGrenade);
            projectile.height = 10;
            projectile.alpha = 255;
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 50;
            projectile.height = 50;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            for(int num621 = 0; num621 < 20; num621++) {
                int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num622].velocity *= 3f;
                if(Main.rand.Next(2) == 0) {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for(int num623 = 0; num623 < 35; num623++) {
                int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num624].velocity *= 2f;
            }

            for(int num625 = 0; num625 < 3; num625++) {
                float scaleFactor10 = 0.33f;
                if(num625 == 1) {
                    scaleFactor10 = 0.66f;
                }
                if(num625 == 2) {
                    scaleFactor10 = 1f;
                }
                int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13C3C_cp_0 = Main.gore[num626];
                expr_13C3C_cp_0.velocity.X = expr_13C3C_cp_0.velocity.X + 1f;
                Gore expr_13C5C_cp_0 = Main.gore[num626];
                expr_13C5C_cp_0.velocity.Y = expr_13C5C_cp_0.velocity.Y - 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13CFF_cp_0 = Main.gore[num626];
                expr_13CFF_cp_0.velocity.X = expr_13CFF_cp_0.velocity.X - 1f;
                Gore expr_13D1F_cp_0 = Main.gore[num626];
                expr_13D1F_cp_0.velocity.Y = expr_13D1F_cp_0.velocity.Y - 1f;
            }
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.Kill();
        }

        public override void AI() {
            int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 226, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 226, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].velocity *= 0f;
            Main.dust[dust2].scale = 0.9f;
            Main.dust[dust].scale = 0.9f;

            projectile.ai[1] += 1f;
            if(projectile.ai[1] >= 7200f) {
                projectile.alpha += 5;
                if(projectile.alpha > 255) {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }

            projectile.localAI[0] += 1f;
            if(projectile.localAI[0] >= 10f) {
                projectile.localAI[0] = 0f;
                int num416 = 0;
                int num417 = 0;
                float num418 = 0f;
                int num419 = projectile.type;
                for(int num420 = 0; num420 < 1000; num420++) {
                    if(Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
                        num416++;
                        if(Main.projectile[num420].ai[1] > num418) {
                            num417 = num420;
                            num418 = Main.projectile[num420].ai[1];
                        }
                    }
                    if(num416 > 2) {
                        Main.projectile[num417].netUpdate = true;
                        Main.projectile[num417].ai[1] = 36000f;
                        return;
                    }
                }
            }
        }

    }
}
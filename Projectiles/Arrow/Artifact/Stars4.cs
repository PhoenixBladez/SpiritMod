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

namespace SpiritMod.Projectiles.Arrow.Artifact
{
    class Stars4 : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Burning Star");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.timeLeft = 500;
            projectile.height = 30;
            projectile.width = 10;
            projectile.scale = 1.15f;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
        }

        public override void Kill(int timeLeft) {
            Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-80, 80),
                projectile.Center.Y - 1000 + Main.rand.Next(-50, 50),
                0, Main.rand.Next(18, 28),
                mod.ProjectileType("Stars3"), projectile.damage / 4 * 3, projectile.knockBack, Main.myPlayer);
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f,
                ModContent.ProjectileType<Fire>(), projectile.damage / 2 * 3, projectile.knockBack, projectile.owner);
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 50;
            projectile.height = 50;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            for(int num621 = 0; num621 < 20; num621++) {
                int num622 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    244, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num622].velocity *= 3f;
                if(Main.rand.Next(2) == 0) {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }

            for(int num623 = 0; num623 < 35; num623++) {
                int num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    244, 0f, 0f, 100, default(Color), 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    244, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num624].velocity *= 2f;
            }

            for(int num625 = 0; num625 < 3; num625++) {
                float scaleFactor10 = 0.33f;
                if(num625 == 1)
                    scaleFactor10 = 0.66f;
                else if(num625 == 2)
                    scaleFactor10 = 1f;

                int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X += 1f;
                Main.gore[num626].velocity.Y += 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X -= 1f;
                Main.gore[num626].velocity.Y += 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X += 1f;
                Main.gore[num626].velocity.Y -= 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Main.gore[num626].velocity.X -= 1f;
                Main.gore[num626].velocity.Y -= 1f;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
            int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
            Main.dust[dust1].noGravity = true;
            Main.dust[dust2].noGravity = true;
            Main.dust[dust1].velocity = Vector2.Zero;
            Main.dust[dust2].velocity = Vector2.Zero;
            Main.dust[dust2].scale = 0.6f;
            Main.dust[dust1].scale = 0.6f;
            Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);

            projectile.frameCounter++;
            if((float)projectile.frameCounter >= 3f) {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            projectile.Kill();
            target.AddBuff(BuffID.OnFire, 180);

            MyPlayer mp = Main.player[projectile.owner].GetSpiritPlayer();
            if(mp.MoonSongBlossom && Main.rand.Next(10) == 0) {
                Projectile.NewProjectile(target.Center.X, target.Center.Y - 100, 0f, 0f,
                    ModContent.ProjectileType<Moon>(), projectile.damage / 3 * 2, 4, projectile.owner);
            }
        }

    }
}

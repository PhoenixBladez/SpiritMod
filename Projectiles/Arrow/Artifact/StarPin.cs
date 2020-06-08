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
    public class StarPin : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Star Pin");
        }

        public override void SetDefaults() {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 240;
            projectile.ranged = true;
            projectile.aiStyle = 1;
            projectile.CloneDefaults(ProjectileID.Bullet);
        }

        public override void Kill(int timeLeft) {
            for(int i = 0; i < 2; i++) {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 244);
            }

            if(Main.rand.Next(4) == 1) {
                Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

                Projectile.NewProjectile(projectile.position, new Vector2(0, -5), ModContent.ProjectileType<StarEnergyBolt>(), projectile.damage / 3, 0, Main.myPlayer);

                Projectile.NewProjectile(projectile.position, new Vector2(6, -2), ModContent.ProjectileType<StarEnergyBolt>(), projectile.damage / 3, 0, Main.myPlayer);
                Projectile.NewProjectile(projectile.position, new Vector2(-6, -2), ModContent.ProjectileType<StarEnergyBolt>(), projectile.damage / 3, 0, Main.myPlayer);

                Projectile.NewProjectile(projectile.position, new Vector2(3, 5), ModContent.ProjectileType<StarEnergyBolt>(), projectile.damage / 3, 0, Main.myPlayer);
                Projectile.NewProjectile(projectile.position, new Vector2(-3, 5), ModContent.ProjectileType<StarEnergyBolt>(), projectile.damage / 3, 0, Main.myPlayer);
            }
        }

        public override void AI() {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            MyPlayer mp = Main.player[projectile.owner].GetSpiritPlayer();
            if(mp.MoonSongBlossom) {
                int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173);
                int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173);
                Main.dust[dust1].noGravity = true;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust1].velocity = Vector2.Zero;
                Main.dust[dust2].velocity = Vector2.Zero;
                Main.dust[dust2].scale = 0.6f;
                Main.dust[dust1].scale = 0.6f;
                Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);
            }

            int dust3 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
            int dust4 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
            Main.dust[dust3].noGravity = true;
            Main.dust[dust4].noGravity = true;
            Main.dust[dust3].velocity = Vector2.Zero;
            Main.dust[dust4].velocity = Vector2.Zero;
            Main.dust[dust4].scale = 0.6f;
            Main.dust[dust3].scale = 0.6f;
            Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(BuffID.OnFire, 300);
        }

    }
}

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
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
    public class MeteorShardHostile1 : ModProjectile
    {

        private int DamageAdditive;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Meteor Shard");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults() {
            projectile.aiStyle = -1;
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.timeLeft = 300;
        }

        public override void AI() {
            projectile.rotation += .3f;
            int num1 = ModContent.NPCType<Mineroid>();
            if(!Main.npc[(int)projectile.ai[1]].active) {
                projectile.timeLeft = 0;
                projectile.active = false;
            }
            float num2 = 60f;
            float x = 0.5f;
            float y = 0.25f;
            int Damage = 0;
            float num3 = 0.0f;
            bool flag1 = true;
            bool flag2 = false;
            bool flag3 = false;
            if((double)projectile.ai[0] < (double)num2) {
                bool flag4 = true;
                int index1 = (int)projectile.ai[1];
                if(Main.npc[index1].active && Main.npc[index1].type == num1) {
                    if(!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                } else {
                    projectile.ai[0] = num2;
                    flag4 = false;
                }
                if(flag4 && !flag2) {
                    projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                }
            }

        }
        public override void Kill(int timeLeft) {
            bool expertMode = Main.expertMode;
            {
                float maxDistance = 500f; // max distance to search for a player
                int index = -1;
                for(int i = 0; i < Main.maxPlayers; i++) {
                    Player target = Main.player[i];
                    if(!target.active || target.dead) {
                        continue;
                    }
                    float curDistance = projectile.Distance(target.Center);
                    if(curDistance < maxDistance) {
                        index = i;
                        maxDistance = curDistance;
                    }
                }
                if(index != -1) {
                    Player player = Main.player[index];
                    Vector2 direction = Main.player[index].Center - projectile.Center;
                    direction.Normalize();
                    direction *= 5f;
                    int amountOfProjectiles = 1;
                    for(int i = 0; i < amountOfProjectiles; ++i) {
                        float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                        Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 89);
                        Projectile.NewProjectile(projectile.Center, direction, mod.ProjectileType("MeteorShardHostile2"), projectile.damage, 0, Main.myPlayer);
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for(int k = 0; k < projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}

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
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SpiritCore
{
    public class SpiritMinionHostile : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Exploding Soul");
        }

        public override void SetDefaults() {
            npc.width = npc.height = 40;
            Main.npcFrameCount[npc.type] = 4;
            npc.lifeMax = 50;
            npc.damage = 40;
            npc.knockBackResist = 0;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.friendly = false;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public override bool PreAI() {
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

            npc.TargetClosest(true);
            float speed = 9f;
            float acceleration = 0.4f;
            Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5F, npc.position.Y + npc.height * 0.5F);
            float xDir = Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5F) - vector2.X;
            float yDir = Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5F) - vector2.Y;
            float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

            float num10 = speed / length;
            xDir = xDir * num10;
            yDir = yDir * num10;
            if(npc.velocity.X < xDir) {
                npc.velocity.X = npc.velocity.X + acceleration;
                if(npc.velocity.X < 0 && xDir > 0)
                    npc.velocity.X = npc.velocity.X + acceleration;
            } else if(npc.velocity.X > xDir) {
                npc.velocity.X = npc.velocity.X - acceleration;
                if(npc.velocity.X > 0 && xDir < 0)
                    npc.velocity.X = npc.velocity.X - acceleration;
            }
            if(npc.velocity.Y < yDir) {
                npc.velocity.Y = npc.velocity.Y + acceleration;
                if(npc.velocity.Y < 0 && yDir > 0)
                    npc.velocity.Y = npc.velocity.Y + acceleration;
            } else if(npc.velocity.Y > yDir) {
                npc.velocity.Y = npc.velocity.Y - acceleration;
                if(npc.velocity.Y > 0 && yDir < 0)
                    npc.velocity.Y = npc.velocity.Y - acceleration;
            }
            for(int i = 0; i < 255; i++) {
                if(Main.player[i].Hitbox.Intersects(npc.Hitbox)) {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.checkDead();
                    npc.active = false;
                    return false;
                }
            }
            return false;
        }

        public override void AI() {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
        }

        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

        public override bool CheckDead() {
            Vector2 center = npc.Center;
            Terraria.Projectile.NewProjectile(center.X, center.Y, 0f, 0f, mod.ProjectileType("UnstableWisp_Explosion"), 40, 0f, Main.myPlayer);
            return true;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter++;
            if(npc.frameCounter >= 4) {
                npc.frame.Y = (npc.frame.Y + frameHeight) % (Main.npcFrameCount[npc.type] * frameHeight);
                npc.frameCounter = 0;
            }
        }
    }
}

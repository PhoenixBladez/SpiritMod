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

namespace SpiritMod.NPCs
{
    public class GraniteCore : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crackling Core");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 24;
            npc.height = 32;
            npc.damage = 30;
            npc.defense = 4;
            npc.noGravity = true;
            npc.lifeMax = 18;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 360f;
            npc.knockBackResist = .3f;
            npc.aiStyle = 44;
            aiType = NPCID.FlyingAntlion;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void HitEffect(int hitDirection, double damage) {
            int d = 226;
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
            }
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            npc.localAI[0] += 1f;
            if(npc.localAI[0] == 12f) {
                npc.localAI[0] = 0f;
                for(int j = 0; j < 12; j++) {
                    Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
                    vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default(Vector2));
                    int num8 = Dust.NewDust(npc.Center, 0, 0, 226, 0f, 0f, 160, default(Color), 1f);
                    Main.dust[num8].scale = 0.9f;
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = npc.Center + vector2;
                    Main.dust[num8].velocity = npc.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
                }
            }
        }
        public override bool CheckDead() {
            for(int i = 0; i < 20; i++) {
                int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .25f;
                if(Main.dust[num].position != npc.Center)
                    Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
            }
            for(int i = 0; i < 4; i++) {
                float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
                    velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, 1, Main.myPlayer, 0, 0);
                Main.projectile[proj].friendly = false;
                Main.projectile[proj].hostile = true;
                Main.projectile[proj].velocity *= 4f;
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GraniteChunk>(), 1);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            {
                target.AddBuff(BuffID.Confused, 160);
            }
        }
    }
}

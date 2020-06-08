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
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs
{
    public class GraniteSlime : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Granite Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults() {
            npc.width = 16;
            npc.height = 12;
            npc.damage = 22;
            npc.defense = 8;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.lifeMax = 85;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath43;
            npc.value = 200f;
            npc.knockBackResist = .25f;
            npc.aiStyle = 1;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/GraniteSlime_Glow"));
        }
        bool jump;
        public override void AI() {
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            bool expertMode = Main.expertMode;
            npc.direction = npc.spriteDirection;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.12f, 0.29f, .42f);
            if(!npc.collideY) {
                jump = true;
            }
            if(npc.collideY && jump && Main.rand.Next(3) == 0) {
                for(int i = 0; i < 20; i++) {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if(Main.dust[num].position != npc.Center)
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                }
                jump = false;
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 110));
                int damage = expertMode ? 11 : 21;
                if(distance < 92) {
                    target.AddBuff(BuffID.Confused, 180);
                    target.AddBuff(BuffID.Shine, 180);
                }
                for(int i = 0; i < 2; i++) {
                    float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                    Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
                        velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, 1, Main.myPlayer, 0, 0);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].velocity *= 4f;
                }
            }
        }
        public override void NPCLoot() {
            if(Main.rand.Next(2) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GraniteChunk>(), 1);
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return (tile == 368) && NPC.downedBoss2 && spawnInfo.spawnTileY > Main.rockLayer ? 0.1f : 0f;

        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                int d = 226;
                for(int k = 0; k < 20; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
                    Dust.NewDust(npc.position, npc.width, npc.height, 240, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
                }
            }
            if(Main.netMode != 1 && npc.life <= 0 && Main.rand.Next(3) == 0) {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                {
                    for(int i = 0; i < 20; i++) {
                        int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if(Main.dust[num].position != npc.Center)
                            Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                    Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
                    NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<GraniteCore>());
                }
            }
        }
    }
}

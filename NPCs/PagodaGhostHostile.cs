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
    public class PagodaGhostHostile : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Disturbed Yurei");
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            if(NPC.downedBoss2) {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 25;
                npc.noGravity = true;
                npc.defense = 8;
                npc.lifeMax = 80;
            }
            if(NPC.downedBoss3) {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 30;
                npc.noGravity = true;
                npc.defense = 11;
                npc.lifeMax = 130;
            } else {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 23;
                npc.noGravity = true;
                npc.defense = 4;
                npc.lifeMax = 50;
            }
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 120f;
            npc.knockBackResist = .1f;
            npc.noTileCollide = true;
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
            int d1 = 91;
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 117, new Color(0, 255, 142), .6f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                for(int i = 0; i < 40; i++) {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 91, 0f, -2f, 117, new Color(0, 255, 142), .6f);
                    Main.dust[num].noGravity = true;
                    Dust expr_62_cp_0 = Main.dust[num];
                    expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    Dust expr_92_cp_0 = Main.dust[num];
                    expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    if(Main.dust[num].position != npc.Center) {
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
            }
        }

        public override void AI() {
            Player player = Main.player[npc.target];
            npc.alpha += 1;
            if(npc.alpha >= 180) {

                int angle = Main.rand.Next(360);
                int distX = (int)(Math.Sin(angle * (Math.PI / 180)) * 90);
                int distY = (int)(Math.Cos(angle * (Math.PI / 180)) * 160);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                npc.position.X = player.position.X + distX;
                npc.position.Y = player.position.Y + distY;
                Gore.NewGore(npc.position, npc.velocity, 99);
                npc.alpha = 0;
                Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
            npc.spriteDirection = npc.direction;
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(4) == 0) {
                target.AddBuff(BuffID.Cursed, 180);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for(int k = 0; k < npc.oldPos.Length; k++) {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
    }
}

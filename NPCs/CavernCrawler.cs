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
    public class CavernCrawler : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cavern Crawler");
            Main.npcFrameCount[npc.type] = 16;
            NPCID.Sets.TrailCacheLength[npc.type] = 5;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 46;
            npc.height = 28;
            npc.damage = 22;
            npc.defense = 9;
            npc.lifeMax = 40;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 860f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(spawnInfo.playerSafe && !NPC.downedBoss1) {
                return 0f;
            }
            return SpawnCondition.Underground.Chance * 0.095f;
        }
        public override void NPCLoot() {
            if(Main.rand.Next(100) <= 4) {

                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ModContent.ItemType<CrawlerockStaff>()));
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ModContent.ItemType<Carapace>()), Main.rand.Next(2) + 1);
        }
        int frame = 0;
        int timer = 0;
        bool trailbehind;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 320) {
                {
                    aiType = NPCID.Unicorn;
                    npc.aiStyle = 26;
                    npc.knockBackResist = 0.15f;
                    trailbehind = true;
                }
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame < 14) {
                    frame = 14;
                }
                if(frame >= 16) {
                    frame = 14;
                }
            } else {
                trailbehind = false;
                aiType = NPCID.Snail;
                npc.aiStyle = 3;
                npc.knockBackResist = 0.75f;
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 13) {
                    frame = 1;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if(trailbehind) {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                for(int k = 0; k < npc.oldPos.Length; k++) {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .61f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler3"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler4"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler5"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler6"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler7"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Crawler8"));
            }
        }
    }
}

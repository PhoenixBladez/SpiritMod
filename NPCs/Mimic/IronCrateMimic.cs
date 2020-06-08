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

namespace SpiritMod.NPCs.Mimic
{
    public class IronCrateMimic : ModNPC
    {
        bool jump = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Iron Crate Mimic");
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }
        public override void SetDefaults() {
            npc.width = 48;
            npc.height = 42;
            npc.damage = 17;
            npc.defense = 15;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 360f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 25;
            aiType = 85;
        }
        int frame = 2;
        int timer = 0;
        int mimictimer = 0;
        public override void AI() {
            mimictimer++;
            if(mimictimer <= 80) {
                frame = 0;
                mimictimer = 81;
            }
            Player target = Main.player[npc.target];
            {
                timer++;
                if(timer == 4) {
                    frame++;
                    timer = 0;
                }
                if(frame == 4) {
                    frame = 1;
                }
            }
            if(npc.collideY && jump && npc.velocity.Y > 0) {
                if(Main.rand.Next(4) == 0) {
                    jump = false;
                    for(int i = 0; i < 20; i++) {
                        int dust = Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 191, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
            if(!npc.collideY)
                jump = true;

        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 720) {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                for(int k = 0; k < npc.oldPos.Length; k++) {
                    var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                int a = Gore.NewGore(npc.position, npc.velocity / 6, 220);
                int a1 = Gore.NewGore(npc.position, npc.velocity / 6, 221);
                int a2 = Gore.NewGore(npc.position, npc.velocity / 6, 222);
            }
            if(npc.life <= 0 || npc.life >= 0) {
                int d = 8;
                for(int k = 0; k < 10; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
                }

                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .77f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .47f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .57f);
            }
        }
        public override void NPCLoot() {
            if(Main.rand.Next(13) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3200);
            }
            if(Main.rand.Next(13) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3201);
            }
            if(Main.rand.Next(15) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 2501);
            }
            if(Main.rand.Next(10) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 2608);
            }
            {
                int bars;
                bars = Main.rand.Next(new int[] { 19, 20, 21, 22, 703, 704, 705, 706 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bars, Main.rand.Next(6, 14));
            }
            {
                int potions;
                potions = Main.rand.Next(new int[] { 288, 296, 304, 305, 2322, 2323, 2324, 2327 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, potions, Main.rand.Next(1, 3));
            }
            {
                int Gpotions;
                Gpotions = Main.rand.Next(new int[] { 188, 189 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Gpotions, Main.rand.Next(5, 15));
            }
            if(Main.rand.Next(3) == 2) {

                int bait;
                bait = Main.rand.Next(new int[] { 2675 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bait, Main.rand.Next(3, 7));

            }
            {
                int bait;
                bait = Main.rand.Next(new int[] { 2676 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bait, Main.rand.Next(3, 7));
            }
            if(Main.rand.Next(5) == 2) {
                int coins;
                coins = Main.rand.Next(new int[] { 73 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, coins, Main.rand.Next(3, 7));
            }
        }
    }
}

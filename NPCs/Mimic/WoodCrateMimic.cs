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

namespace SpiritMod.NPCs.Mimic
{
    public class WoodCrateMimic : ModNPC
    {
        bool jump = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wooden Crate Mimic");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults() {
            npc.width = 48;
            npc.height = 42;
            npc.damage = 9;
            npc.defense = 4;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 360f;
            npc.knockBackResist = 0.34f;
            npc.aiStyle = 3;
            aiType = 508;
        }
        int frame = 2;
        int timer = 0;
        int mimictimer = 0;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            if(npc.wet) {
                npc.noGravity = true;
            } else {
                npc.noGravity = false;
            }
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
                if(Main.rand.Next(8) == 0) {
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
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                int a = Gore.NewGore(npc.position, npc.velocity / 6, 220);
                int a1 = Gore.NewGore(npc.position, npc.velocity / 6, 221);
                int a2 = Gore.NewGore(npc.position, npc.velocity / 6, 222);
            }
            if(npc.life <= 0 || npc.life >= 0) {
                int d = 7;
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
            if(Main.rand.Next(20) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3200);
            }
            if(Main.rand.Next(20) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3201);
            }
            if(Main.rand.Next(23) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 997);
            }
            if(Main.rand.Next(18) == 1) {
                int loot;
                loot = Main.rand.Next(new int[] { 285, 946, 953, 3068, 3084 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, loot);
            }
            if(Main.rand.Next(5) == 4) {
                int ores;
                ores = Main.rand.Next(new int[] { 11, 12, 13, 14, 699, 700, 701, 702 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ores, Main.rand.Next(8, 20));
            } else {
                int bars;
                bars = Main.rand.Next(new int[] { 19, 20, 21, 22, 703, 704, 705, 706 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bars, Main.rand.Next(2, 7));
            }
            if(Main.rand.Next(3) == 2) {
                int potions;
                potions = Main.rand.Next(new int[] { 288, 290, 292, 304, 298, 2322, 2323, 291, 2329 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, potions, Main.rand.Next(1, 3));
            }
            {
                int Gpotions;
                Gpotions = Main.rand.Next(new int[] { 28, 110 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Gpotions, Main.rand.Next(5, 15));
            }
            if(Main.rand.Next(3) == 2) {

                int bait;
                bait = Main.rand.Next(new int[] { 2675 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bait, Main.rand.Next(3, 7));

            } else {
                int bait;
                bait = Main.rand.Next(new int[] { 2674 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, bait, Main.rand.Next(3, 7));
            }
            if(Main.rand.Next(5) == 2) {
                int coins;
                coins = Main.rand.Next(new int[] { 73 });
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, coins, Main.rand.Next(1, 5));
            }
        }
    }
}

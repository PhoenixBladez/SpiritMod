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

namespace SpiritMod.NPCs.Dungeon
{
    public class PlagueDoctor : ModNPC
    {
        bool attack = false;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dark Alchemist");
            Main.npcFrameCount[npc.type] = 12;
        }

        public override void SetDefaults() {
            npc.width = 34;
            npc.height = 48;
            npc.damage = 29;
            npc.defense = 16;
            npc.lifeMax = 140;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 1000f;
            npc.knockBackResist = .35f;
        }


        public override void HitEffect(int hitDirection, double damage) {
            int d = 37;
            int d1 = 75;
            for(int k = 0; k < 30; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PDoctor3"), 1f);
            }
        }

        public override void NPCLoot() {
            if(Main.rand.Next(1) == 153) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if(Main.rand.Next(1) == 75) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);
            }
            if(Main.rand.Next(1) == 100) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
            if(Main.rand.Next(4) == 1000) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
            }
            int lootamt = Main.rand.Next(30, 60);
            for(int J = 0; J <= lootamt; J++) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PlagueVial>());
            }
            string[] lootTable = { "PlagueDoctorCowl", "PlagueDoctorRobe", "PlagueDoctorLegs" };
            if(Main.rand.Next(6) == 0) {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
            }
        }

        int frame = 0;
        int timer = 0;
        //  int shootTimer = 0;
        public override void AI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 200) {
                attack = true;
            }
            if(distance > 250) {
                attack = false;
            }
            if(attack) {
                npc.velocity.X = .008f * npc.direction;
                //shootTimer++;
                if(frame == 3 && timer == 0) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 106);
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 7f;
                    direction.Y *= 7f;
                    float A = (float)Main.rand.Next(-50, 50) * 0.02f;
                    float B = (float)Main.rand.Next(-50, 50) * 0.02f;
                    int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<ToxicFlaskHostile>(), 13, 1, Main.myPlayer, 0, 0);
                    for(int k = 0; k < 11; k++) {
                        Dust.NewDust(npc.position, npc.width, npc.height, 75, (float)direction.X + A, (float)direction.Y + B, 0, default(Color), .61f);
                    }
                    Main.projectile[p].hostile = true;
                    //  shootTimer = 0;
                    timer++;
                }
                timer++;
                if(timer >= 12) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 5) {
                    frame = 0;
                }
                if(target.position.X > npc.position.X) {
                    npc.direction = 1;
                } else {
                    npc.direction = -1;
                }
            } else {
                //shootTimer = 0;
                npc.aiStyle = 3;
                aiType = NPCID.Skeleton;
                timer++;
                if(timer >= 4) {
                    frame++;
                    timer = 0;
                }
                if(frame >= 11) {
                    frame = 5;
                }
                if(frame < 5) {
                    frame = 5;
                }
            }
            /*if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }*/
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<PlagueDoctor>()) < 1 ? 0.05f : 0f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Dungeon/PlagueDoctor_Glow"));
        }
        public override void FindFrame(int frameHeight) {
            npc.frame.Y = frameHeight * frame;
        }
    }
}

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
    public class CrystalDrifter : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crystal Drifter");
            Main.npcFrameCount[npc.type] = 1;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 44;
            npc.height = 88;
            npc.damage = 27;
            npc.defense = 17;
            npc.lifeMax = 200;
            npc.HitSound = SoundID.NPCDeath15;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 200f;
            npc.knockBackResist = 0f;
            npc.alpha = 100;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = 22;
            npc.aiStyle = -1;
        }
        int timer = 0;
        public override bool PreAI() {
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            MyPlayer modPlayer = target.GetSpiritPlayer();
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if(distance < 500) {
                {
                    target.AddBuff(BuffID.WindPushed, 90);
                    modPlayer.windEffect2 = true;
                }
            }
            timer++;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .5f, .36f, .14f);
            npc.spriteDirection = npc.direction;
            float num1 = 5f;
            float moveSpeed = 0.08f;
            npc.TargetClosest(true);
            Vector2 vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(0.0f, Main.rand.NextFloat(-200f, -150f));
            float num2 = vector2_1.Length();
            Vector2 desiredVelocity;
            if((double)num2 < 20.0)
                desiredVelocity = npc.velocity;
            else if((double)num2 < 40.0) {
                vector2_1.Normalize();
                desiredVelocity = vector2_1 * (num1 * 0.35f);
            } else if((double)num2 < 80.0) {
                vector2_1.Normalize();
                desiredVelocity = vector2_1 * (num1 * 0.65f);
            } else {
                vector2_1.Normalize();
                desiredVelocity = vector2_1 * num1;
            }
            npc.SimpleFlyMovement(desiredVelocity, moveSpeed);
            npc.rotation = npc.velocity.X * 0.1f;
            if(timer >= 90) {
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
                bool expertMode = Main.expertMode;
                int damage = expertMode ? 12 : 18;
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, ModContent.ProjectileType<FrostOrbiterHostile>(), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].hostile = true;
                timer = 0;
            }
            return false;
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
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.player.ZoneOverworldHeight && spawnInfo.player.ZoneSnow && Main.raining && !NPC.AnyNPCs(ModContent.NPCType<CrystalDrifter>()) && NPC.downedBoss3 ? 0.09f : 0f;
        }


        public override void HitEffect(int hitDirection, double damage) {
            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 51);
            for(int k = 0; k < 20; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 68, hitDirection * 2f, -1f, 0, default(Color), 1f);
            }

            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter1"), .5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter2"), .5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter3"), .5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter4"), .5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Drifter/Drifter5"), .5f);
                Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 41);
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 30;
                npc.height = 30;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 20; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for(int num623 = 0; num623 < 40; num623++) {
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 68, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(2) == 1)
                target.AddBuff(BuffID.Frostburn, 150);
        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CryoliteOre>(), Main.rand.Next(4, 9) + 1);
        }
    }
}
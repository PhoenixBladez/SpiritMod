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
    public class FallenAngel : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fallen Angel");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FlyingFish];
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 44;
            npc.height = 60;
            npc.damage = 50;
            npc.defense = 31;
            npc.lifeMax = 3200;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 60f;
            npc.knockBackResist = 0.03f;
            npc.aiStyle = 44;
            npc.noGravity = true;
            npc.noTileCollide = true;
            aiType = NPCID.FlyingFish;
            animationType = NPCID.FlyingFish;
            npc.stepSpeed = 2f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.sky && Main.hardMode && NPC.AnyNPCs(ModContent.NPCType<FallenAngel>()) ? 0.013f : 0f;
        }
        int aiTimer;
        float alphaCounter;
        public override void AI() {
            alphaCounter += 0.04f;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.091f, 0.24f, .24f);
            npc.rotation = npc.velocity.X * .009f;
            aiTimer++;
            if(aiTimer == 100 || aiTimer == 240 || aiTimer == 360 || aiTimer == 620) {
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
                direction.X = direction.X * Main.rand.Next(8, 10);
                direction.Y = direction.Y * Main.rand.Next(8, 10);
                npc.velocity.X = direction.X;
                npc.velocity.Y = direction.Y;
                npc.velocity *= 0.96f;
            }
            if(aiTimer >= 680) {
                Main.PlaySound(2, npc.Center, 109);
                for(int i = 0; i < 5; i++) {
                    for(int k = 0; k < 2; k++) {
                        int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GoldCoin);
                        Main.dust[dust].velocity *= -1f;
                        Main.dust[dust].noGravity = true;
                        Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                        vector2_1.Normalize();
                        Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                        Main.dust[dust].velocity = vector2_2;
                        vector2_2.Normalize();
                        Vector2 vector2_3 = vector2_2 * 34f;
                        Main.dust[dust].position = npc.Center - vector2_3;
                    }
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ModContent.ProjectileType<ShootingStarHostile>(), 30, 1, Main.myPlayer, 0, 0);
                }
                aiTimer = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);

            {
                float sineAdd = (float)Math.Sin(alphaCounter) + 3;
                Vector2 drawPos1 = npc.Center - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
            }
            if(npc.velocity != Vector2.Zero) {
                for(int k = 0; k < npc.oldPos.Length; k++) {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(drawColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/FallenAngel_Glow"));
        }
        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StarPiece>(), Main.rand.Next(1, 3) + 1);

            if(Main.rand.Next(100) == 20) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.FallenAngel>());
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
            }
            for(int k = 0; k < 2; k++) {
                int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.GoldCoin);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 34f;
                Main.dust[dust].position = npc.Center - vector2_3;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(6) == 0)
                target.AddBuff(BuffID.Cursed, 300);
        }
    }
}

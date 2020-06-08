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
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
    public class Hedron : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hedron");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 32;
            npc.lifeMax = 230;
            npc.damage = 44;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.friendly = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Spirit/Hedron_Glow"));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
                int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<SpiritGrass>(), };
                return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY < Main.rockLayer ? 1.4f : 0f;
            }
            return 0f;
        }

        public override bool PreAI() {
            if(npc.localAI[0] == 0f) {
                npc.localAI[0] = npc.Center.Y;
                npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
            }
            if(npc.Center.Y >= npc.localAI[0]) {
                npc.localAI[1] = -1f;
                npc.netUpdate = true;
            }
            if(npc.Center.Y <= npc.localAI[0] - 40f) {
                npc.localAI[1] = 1f;
                npc.netUpdate = true;
            }
            npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.05f * npc.localAI[1], -2f, 2f);
            npc.ai[0] += 1f;
            npc.netUpdate = true;
            if(npc.ai[0] >= 90f) {
                bool hasTarget = false;
                Vector2 target = Vector2.Zero;
                float targetRange = 640f;//1600 was too much
                for(int i = 0; i < 255; i++) {
                    if(Main.player[i].active && !Main.player[i].dead) {
                        float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
                        float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
                        float distOrth = Math.Abs(npc.position.X + (float)(npc.width / 2) - playerX) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - playerY);
                        if(distOrth < targetRange) {
                            targetRange = distOrth;
                            target = Main.player[i].Center;
                            hasTarget = true;
                        }
                    }
                }
                if(hasTarget) {
                    Vector2 delta = target - npc.Center;
                    delta.Normalize();
                    delta *= 6f;
                    int slot = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, delta.X, delta.Y, ModContent.ProjectileType<HedronBeam>(), 32, 1f, Main.myPlayer, 0f, 0f);
                    Main.projectile[slot].tileCollide = false;
                    Main.projectile[slot].netUpdate = true;
                }
                npc.ai[0] = 0f;
                npc.netUpdate = true;
            }
            return true;
        }

        public override void AI() {
            if(Main.rand.Next(4) == 0)
                Dust.NewDust(npc.position, npc.width, npc.height, 187);
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, 13);
                Gore.NewGore(npc.position, npc.velocity, 12);
                Gore.NewGore(npc.position, npc.velocity, 11);
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.10000000149011612;
            npc.frameCounter %= (double)Main.npcFrameCount[npc.type];
            int num = (int)npc.frameCounter;
            npc.frame.Y = num * frameHeight;
            npc.spriteDirection = npc.direction;
        }

        public override void NPCLoot() {
            if(Main.rand.Next(25) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HedronStaff>());
        }
    }
}

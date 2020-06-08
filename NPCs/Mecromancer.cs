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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    [AutoloadBossHead]
    public class Mecromancer : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mechromancer");
            Main.npcFrameCount[npc.type] = 17;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 150f;
        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 48;
            npc.damage = 20;
            npc.defense = 8;
            npc.lifeMax = 310;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 6760f;
            npc.knockBackResist = 0.1f;
            npc.noTileCollide = false;
            if(!flying) {
                animationType = 471;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(!NPC.downedBoss2 || NPC.AnyNPCs(ModContent.NPCType<Mecromancer>())) {
                return 0f;
            }
            return SpawnCondition.GoblinArmy.Chance * 0.08666f;
        }

        public override void NPCLoot() {
            if(Main.invasionType == 1) {
                Main.invasionSize -= 5;
                if(Main.invasionSize < 0) {
                    Main.invasionSize = 0;
                }
                if(Main.netMode != 1) {
                    Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 4, 0);
                }
                if(Main.netMode == 2) {
                    NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, (float)Main.invasionProgressIcon, 0f, 0, 0, 0);
                }
            }
            if(Main.rand.Next(3) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<KnocbackGun>());
            }
            if(Main.rand.Next(25) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RocketBoots);
            }
            string[] lootTable = { "CoiledMask", "CoiledChestplate", "CoiledLeggings" };
            int loot = Main.rand.Next(lootTable.Length);
            {
                npc.DropItem(mod.ItemType(lootTable[loot]));
            }
            int Techs = Main.rand.Next(2, 5);
            for(int J = 0; J <= Techs; J++) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TechDrive>());
            }
        }
        int timer;
        bool flying;
        public override void AI() {
            if(Main.rand.Next(250) == 2) {
                Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7);
            }
            timer++;
            if(timer == 100 || timer == 300) {
                Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7);
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 92);
                npc.TargetClosest();
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                float ai = Main.rand.Next(100);
                direction.Normalize();
                int MechBat = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
                int MechBat1 = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
                if(Main.rand.Next(3) == 0) {
                    int MechBat2 = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
                }
            }
            if(timer > 420 && timer < 840) {
                npc.noTileCollide = true;
                if(Main.rand.Next(40) == 0) {
                    int p = Terraria.Projectile.NewProjectile(npc.position.X, npc.position.Y + 40, 0, 1, ProjectileID.GreekFire1, (int)((npc.damage * .5)), 0);
                }
                Player player = Main.player[npc.target];
                if(Main.rand.Next(20) == 0) {
                    Main.PlaySound(SoundID.Item13, npc.position);
                }
                npc.noGravity = true;
                if(npc.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
                    moveSpeed--;

                if(npc.Center.X <= player.Center.X && moveSpeed <= 40)
                    moveSpeed++;

                npc.velocity.X = moveSpeed * 0.1f;

                if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
                {
                    moveSpeedY--;
                    HomeY = 185f;
                }

                if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
                    moveSpeedY++;
                npc.spriteDirection = npc.direction;
                npc.velocity.Y = moveSpeedY * 0.1f;
                flying = true;
                int type5 = 6;
                float scale4 = 1.95f;
                int alpha4 = 100;
                int num222 = npc.height;
                int num220 = Dust.NewDust(new Vector2(npc.Center.X + 2f, npc.position.Y + (float)num222 - 10f), 8, 8, 6, 0f, 0f, alpha4, default(Color), scale4);
                Main.dust[num220].noGravity = true;
                Main.dust[num220].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - npc.velocity.X * 0.3f;
                Main.dust[num220].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -npc.velocity.Y * 0.3f;
                int num221 = Dust.NewDust(new Vector2(npc.Center.X - 3f, npc.position.Y + (float)num222 - 10f), 8, 8, 6, 0f, 0f, alpha4, default(Color), scale4);
                Main.dust[num221].noGravity = true;
                Main.dust[num221].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - npc.velocity.X * 0.3f;
                Main.dust[num221].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -npc.velocity.Y * 0.3f;
            } else {
                npc.noTileCollide = false;
                flying = false;
                npc.rotation = 0f;
                npc.noGravity = false;
                npc.aiStyle = 3;
                aiType = NPCID.GoblinThief;
            }
            bool expertMode = Main.expertMode;
            if(timer >= 840) {
                int damage = expertMode ? 13 : 22;
                timer = 0;
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));
                for(int f = 0; f < 4; f++) {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 2f;
                    direction.Y *= 2f;

                    int amountOfProjectiles = Main.rand.Next(1, 2);
                    for(int i = 0; i < amountOfProjectiles; ++i) {
                        float A = (float)Main.rand.Next(-50, 50) * 0.23f;
                        float B = (float)Main.rand.Next(-50, 50) * 0.23f;
                        Projectile.NewProjectile(npc.Center.X + (Main.rand.Next(-50, 50)), npc.Center.Y + (Main.rand.Next(-50, 50)), direction.X + A, direction.Y + B, ModContent.ProjectileType<CoilRocket>(), damage, 1, Main.myPlayer, 0, 0);

                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) {
            if(flying) {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
                {
                    Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                    for(int k = 0; k < npc.oldPos.Length; k++) {
                        Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                        Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                        spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                    }
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech2"), 1f);
            }
        }
        public override void FindFrame(int frameHeight) {
            if(flying) {
                npc.frame.Y = frameHeight * 10;
            }
        }
    }
}

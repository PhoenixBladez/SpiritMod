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
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
    [AutoloadBossHead]
    public class InfernonSkull : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Infernus Skull");
        }

        public override void SetDefaults() {
            npc.width = 100;
            npc.height = 80;

            npc.damage = 0;
            npc.lifeMax = 10;
            Main.npcFrameCount[npc.type] = 4;
            npc.alpha = 255;

            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
        }

        public override bool PreAI() {
            if(!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != ModContent.NPCType<Infernon>())
                npc.ai[0] = -1;

            if(npc.ai[0] == -1) {
                npc.alpha += 3;
                if(npc.alpha > 255)
                    npc.active = false;
            } else if(npc.ai[0] == 0) {
                npc.ai[1]++;
                if(npc.ai[1] >= 60) {
                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                }
            } else if(npc.ai[0] == 1) {
                if(npc.ai[1] == 0) {
                    /* npc.TargetClosest(false);
                     Vector2 spinningpoint = Main.player[npc.target].Center - npc.Center;
                     spinningpoint.Normalize();
                     float dir = -1f;
                     if ((double)spinningpoint.X < 0.0)
                         dir = 1f;
                     Vector2 pos = Utils.RotatedBy(spinningpoint, -dir * 6.28318548202515 / 6.0, new Vector2());
                     Projectile.NewProjectile(npc.Center.X, npc.Center.Y, pos.X, pos.Y, mod.ProjectileType("InfernonSkull_Laser"), 5, 0.0f, Main.myPlayer, (dir * 6.28F / 540), npc.whoAmI);
                     npc.netUpdate = true;*/
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(1, 0) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int i;
                    for(i = 0; i < 4; i++) {
                        offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                        Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
                        Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
                        npc.netUpdate = true;
                    }
                    bool expertMode = Main.expertMode;
                    {
                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 33);
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        direction.X *= 12f;
                        direction.Y *= 12f;

                        int amountOfProjectiles = 2;
                        for(int z = 0; z < amountOfProjectiles; ++z) {
                            float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                            float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                            int damage = expertMode ? 20 : 24;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SunBlast>(), damage, 1, Main.myPlayer, 0, 0);

                        }
                    }

                }

                npc.ai[1]++;
                if(npc.ai[1] >= 120) {
                    npc.ai[0] = 2;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                }
            } else if(npc.ai[0] == 2) {
                if(npc.ai[1] == 0) {
                    npc.alpha += 3;
                    if(npc.alpha > 255) {
                        // Teleport.
                        NPC target = Main.npc[(int)npc.ai[3]];
                        Vector2 newPos = target.Center + new Vector2(Main.rand.Next(-200, 201), Main.rand.Next(-200, 201));
                        npc.Center = newPos;

                        npc.ai[1] = 1;
                    }
                } else {
                    npc.alpha -= 3;

                    if(npc.alpha <= 0) {
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                    }
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight) {
            if(npc.ai[0] == 0)
                npc.frame.Y = 0;
            else if(npc.ai[0] == 1)
                npc.frame.Y = frameHeight;
            else if(npc.ai[0] == 2) {
                if(npc.alpha >= 0 && npc.alpha < 100)
                    npc.frame.Y = 0;
                else if(npc.alpha >= 100 && npc.alpha < 175)
                    npc.frame.Y = frameHeight * 2;
                else if(npc.alpha >= 175 && npc.alpha < 255)
                    npc.frame.Y = frameHeight * 3;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Infernon/InfernonSkull_Glow"));
        }
    }
}
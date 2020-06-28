using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Pets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class CaptiveMask : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Captive Mask");
            Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 2;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 34;
            npc.height = 32;
            npc.damage = 20;
            npc.defense = 7;
            npc.knockBackResist = 0.2f;
            npc.lifeMax = 58;
            npc.value = 100f;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public override bool PreAI() {
            float velMax = 1.3f;
            float acceleration = 0.011f;
            npc.TargetClosest(true);
            Vector2 center = npc.Center;
            float deltaX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - center.X;
            float deltaY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - center.Y;
            float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
            npc.ai[1] += 1f;
            if((double)npc.ai[1] > 300.0) {
                acceleration *= 8.25f;
                velMax = 4.5f;
                if((double)npc.ai[1] > 330.0) {
                    npc.ai[1] = 0f;
                }
            } else if((double)distance < 250.0) {
                npc.ai[0] += 0.9f;
                if(npc.ai[0] > 0f) {
                    npc.velocity.Y = npc.velocity.Y + 0.019f;
                } else {
                    npc.velocity.Y = npc.velocity.Y - 0.019f;
                }
                if(npc.ai[0] < -100f || npc.ai[0] > 100f) {
                    npc.velocity.X = npc.velocity.X + 0.019f;
                } else {
                    npc.velocity.X = npc.velocity.X - 0.019f;
                }
                if(npc.ai[0] > 200f) {
                    npc.ai[0] = -200f;
                }
            }
            if((double)distance > 350.0) {
                velMax = 5f;
                acceleration = 0.3f;
            } else if((double)distance > 300.0) {
                velMax = 3f;
                acceleration = 0.2f;
            } else if((double)distance > 250.0) {
                velMax = 1.5f;
                acceleration = 0.3f;
            }
            float stepRatio = velMax / distance;
            float velLimitX = deltaX * stepRatio;
            float velLimitY = deltaY * stepRatio;
            if(Main.player[npc.target].dead) {
                velLimitX = (float)((double)((float)npc.direction * velMax) / 2.0);
                velLimitY = (float)((double)(-(double)velMax) / 2.0);
            }
            if(npc.velocity.X < velLimitX) {
                npc.velocity.X = npc.velocity.X + acceleration;
            } else if(npc.velocity.X > velLimitX) {
                npc.velocity.X = npc.velocity.X - acceleration;
            }
            if(npc.velocity.Y < velLimitY) {
                npc.velocity.Y = npc.velocity.Y + acceleration;
            } else if(npc.velocity.Y > velLimitY) {
                npc.velocity.Y = npc.velocity.Y - acceleration;
            }
            if((double)velLimitX > 0.0) {
                npc.spriteDirection = 1;
                npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
            }
            if((double)velLimitX < 0.0) {
                npc.spriteDirection = -1;
                npc.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0 || npc.life >= 0) {
                Main.PlaySound(3, npc.Center, 3);
                int d = 5;
                for(int k = 0; k < 30; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.27f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .87f);
                }

                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .27f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.87f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .47f);
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.87f);
            }
            if(npc.life <= 0) {
                {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mime/MaskGore1"), Main.rand.NextFloat(.3f, 1.1f));
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mime/MaskGore1"), Main.rand.NextFloat(.3f, 1.1f));
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mime/MaskGore1"), Main.rand.NextFloat(.3f, 1.1f));
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mime/MaskGore1"), Main.rand.NextFloat(.3f, 1.1f));
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (Main.rand.NextBool(4))
            target.AddBuff(BuffID.Obstructed, 60);
        }
        public override void NPCLoot() {
            if(Main.rand.Next(10) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TatteredScript>());

            if(Main.rand.Next(2) == 1) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MimeMask>(), 1);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            if(spawnInfo.playerSafe) {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.007f;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
    }
}

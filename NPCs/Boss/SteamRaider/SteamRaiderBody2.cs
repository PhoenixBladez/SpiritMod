using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Boss;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
    public class SteamRaiderBody2 : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Starplate Voyager");
        }

        public override void SetDefaults() {
            npc.damage = 35; //70
            npc.npcSlots = 5f;
            npc.width = 24; //324
            npc.height = 24; //216
            npc.defense = 19;
            npc.lifeMax = 6500; //250000
            npc.aiStyle = 6; //new
            Main.npcFrameCount[npc.type] = 1; //new
            aiType = -1; //new
            animationType = 10; //new
            npc.knockBackResist = 0f;
            npc.alpha = 255;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.netAlways = true;
            for(int k = 0; k < npc.buffImmune.Length; k++) {
                npc.buffImmune[k] = true;
            }
            music = MusicID.Boss3;
            npc.dontCountMe = true;
        }
        bool exposed;
        int timer;
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
            return false;
        }
        public override void AI() {
            if(Main.netMode != 1) {
                npc.localAI[1] += 1;
                if(npc.localAI[1] == (float)Main.rand.Next(100, 600)) {
                    if(!exposed) {
                        exposed = true;
                    }
                }
                if(npc.localAI[1] >= 601) {
                    npc.localAI[1] = 0f;
                }
                npc.localAI[2] += 1;
                if(npc.localAI[2] == (float)Main.rand.Next(100, 600)) {
                    if(exposed) {
                        exposed = false;
                    }
                }
                if(npc.localAI[2] >= 601) {
                    npc.localAI[2] = 0f;
                }
            }
            if(exposed) {
                npc.defense = 19;
                 npc.dontTakeDamage = false;
            } else {
                npc.defense = 9999;
                 npc.dontTakeDamage = true;
            }
            Player player = Main.player[npc.target];
            bool expertMode = Main.expertMode;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.075f, 0.25f);
            if(Main.netMode != 1) {
                npc.localAI[0] += Main.rand.Next(3);
                if(npc.localAI[0] >= (float)Main.rand.Next(1000, 7500)) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 9);
                    npc.localAI[0] = 0f;
                    npc.TargetClosest(true);
                    if(Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)) {
                        float num941 = 1f; //speed
                        Vector2 vector104 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)(npc.height / 2));
                        float num942 = player.position.X + (float)player.width * 0.5f - vector104.X + (float)Main.rand.Next(-20, 21);
                        float num943 = player.position.Y + (float)player.height * 0.5f - vector104.Y + (float)Main.rand.Next(-20, 21);
                        float num944 = (float)Math.Sqrt((double)(num942 * num942 + num943 * num943));
                        num944 = num941 / num944;
                        num942 *= num944;
                        num943 *= num944;
                        num942 += (float)Main.rand.Next(-10, 11) * 0.0125f;
                        num943 += (float)Main.rand.Next(-10, 11) * 0.0125f;
                        int num945 = expertMode ? 13 : 25;
                        int num946 = ModContent.ProjectileType<Starshock>();
                        vector104.X += num942 * 4f;
                        vector104.Y += num943 * 4;
                        int num947 = Projectile.NewProjectile(vector104.X, vector104.Y, num942, num943, num946, num945, 0f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num947].timeLeft = 350;
                        npc.netUpdate = true;
                    }
                }
            }
            if(!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].life <= Main.npc[(int)npc.ai[1]].lifeMax * .25) {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }
            if(Main.npc[(int)npc.ai[1]].alpha < 128) {
                if(npc.alpha != 0) {
                    for(int num934 = 0; num934 < 2; num934++) {
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 226, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = true;
                    }
                }
                npc.alpha -= 42;
                if(npc.alpha < 0) {
                    npc.alpha = 0;
                }
            }
        }

        public override bool CheckActive() {
            return false;
        }

        public override bool PreNPCLoot() {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {

            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            if(exposed) {
                Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
                int r1 = (int)color1.R;
                drawOrigin.Y += 34f;
                drawOrigin.Y += 8f;
                --drawOrigin.X;
                Vector2 position1 = npc.Bottom - Main.screenPosition;
                Texture2D texture2D2 = Main.glowMaskTexture[239];
                float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
                float num12 = num11;
                if((double)num12 > 0.5)
                    num12 = 1f - num11;
                if((double)num12 < 0.0)
                    num12 = 0.0f;
                float num13 = (float)(((double)num11 + 0.5) % 1.0);
                float num14 = num13;
                if((double)num14 > 0.5)
                    num14 = 1f - num13;
                if((double)num14 < 0.0)
                    num14 = 0.0f;
                Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
                drawOrigin = r2.Size() / 2f;
                Vector2 position3 = position1 + new Vector2(0.0f, -20f);
                Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(84, 207, 255) * 1.6f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * 0.5f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num15 = 1f + num11 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * 0.5f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num16 = 1f + num13 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * 0.5f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                Texture2D texture2D3 = Main.extraTexture[89];
                Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
                drawOrigin = r3.Size() / 2f;
                Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
                float num17 = 1f + num13 * 0.75f;
                GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/SteamRaiderBody2_Glow"));

            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 226, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);
                for (int num623 = 0; num623 < 20; num623++)
                {
                    int dust1 = Dust.NewDust(npc.Center, npc.width, npc.height, 226);

                    Main.dust[dust1].velocity *= -1f;
                    Main.dust[dust1].noGravity = true;
                    Main.dust[dust1].scale *= .8f;
                    Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust1].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 104f;
                    Main.dust[dust1].position = (npc.Center) - vector2_3;
                }
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 12;
                direction.Y *= -12f;

                int amountOfProjectiles = Main.rand.Next(1, 2);
                for(int i = 0; i < amountOfProjectiles; ++i) {
                    float A = (float)Main.rand.Next(-150, 150) * 0.01f;
                    float B = (float)Main.rand.Next(-80, 0) * 0.0f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SteamBodyFallingProj>(), 15, 1, Main.myPlayer, 0, 0);
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if(projectile.penetrate <= -1) {
                damage /= 3;
            } else if(projectile.penetrate >= 7) {
                damage /= 3;
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.65f);
        }
    }
}
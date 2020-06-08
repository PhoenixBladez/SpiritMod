using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
    public class LaserBase : ModNPC
    {
        Vector2 direction9 = Vector2.Zero;
        private bool shooting;
        private int timer = 0;
        private bool inblock = true;
        Vector2 target = Vector2.Zero;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Laser Launcher");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults() {
            npc.width = 56;
            npc.height = 46;
            npc.damage = 0;
            npc.defense = 12;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.lifeMax = 65;
            npc.HitSound = SoundID.NPCHit4;
            npc.value = 160f;
            npc.knockBackResist = .16f;
            npc.noGravity = true;
            npc.dontCountMe = true;
        }

        
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            if(npc.alpha != 255) {
                GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/LaserBase_Glow"));
            }
        }

        public override bool PreAI() {
            npc.TargetClosest(true);
            Vector2 center = npc.Center;
            Player player = Main.player[npc.target];

            float num5 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
            float num6 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
            float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
            if(!(timer >= 100 && timer <= 130)) {
                if(num7 < 0f) {
                    num7 += 6.283f;
                } else if((double)num7 > 6.283) {
                    num7 -= 6.283f;
                }
            }
            npc.spriteDirection = npc.direction;
            timer++;
            if(timer >= 170) {
                npc.active = false;
            } else {
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
            }
            if(timer <= 75) {
                direction9 = player.Center - npc.Center;
                direction9.Normalize();
            }
            if(timer >= 60 && timer <= 130 & timer % 2 == 0) {
                {
                    int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 226);
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].scale *= .8f;
                    Main.dust[dust].noGravity = true;
                    Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-80, 81), (float)Main.rand.Next(-80, 81));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = npc.Center - vector2_3;
                }
            }
            if(npc.alpha != 255) {
                if(Main.rand.NextFloat() < 0.5f) {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = new Vector2(npc.Center.X - 10, npc.Center.Y);
                    dust = Terraria.Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
                }
                if(Main.rand.NextFloat() < 0.5f) {
                    Dust dust;
                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                    Vector2 position = new Vector2(npc.Center.X + 10, npc.Center.Y);
                    dust = Terraria.Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
                }
                if(timer == 130) //change to frame related later
                {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 91);
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, ModContent.ProjectileType<StarLaser>(), 90, 1, Main.myPlayer);
                }
                if (timer < 130 && timer > 75 && timer % 3 == 0)
                {
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, ModContent.ProjectileType<StarLaserTrace>(), 27, 1, Main.myPlayer);
                }
                npc.rotation = direction9.ToRotation() - 1.57f;
            }
            return false;
        }
    }
}

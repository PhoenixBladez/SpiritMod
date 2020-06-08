using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
    public class ArcadeProbe : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Starfarer");
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
            npc.dontCountMe = true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) {
            if(npc.alpha != 255) {
                GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/SteamRaider/LaserBase_Glow"));
            }
        }
        int lifeSpan = 250;
        int distAbove = 150;
        int fireRate = Main.rand.Next(27,41);
        public override bool PreAI() {
            npc.TargetClosest(true);
             if(lifeSpan <= 0) {
                    npc.life = 0;
                    npc.active = false;
                }
            Player player = Main.player[npc.target];
                 if (lifeSpan % 250 == 0)
                    {
                        distAbove = 375;
                        if (Main.rand.Next(2) == 0)
                        {
                            npc.position.X = player.Center.X - Main.rand.Next(300,500);
                            npc.position.Y = player.Center.Y - distAbove;
                            npc.velocity.X = 3f;
                        }
                        else
                        {
                            npc.position.X = player.Center.X + Main.rand.Next(300,500);
                            npc.position.Y = player.Center.Y - distAbove;
                            npc.velocity.X = -3f;
                        }
                         npc.rotation = 0f;
                    }
                    npc.velocity.Y = 0;
                      if (lifeSpan % fireRate == 0)
                      {
                          Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ProjectileID.RayGunnerLaser, 19, 1, Main.myPlayer, 0, 0);
                      }
             lifeSpan--;
            return false;
        }
    }
}

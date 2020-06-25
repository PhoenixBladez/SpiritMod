using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
    public class VileWaspProjectile : ModProjectile
    {

        private int DamageAdditive;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Pesterfly Hatchling");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults() {
            projectile.aiStyle = -1;
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.timeLeft = 900;
        }

        public override void AI() {
            projectile.frameCounter++;
            projectile.spriteDirection = -projectile.direction;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
            int num1 = ModContent.NPCType<Vilemoth>();
            if(!Main.npc[(int)projectile.ai[1]].active) {
                projectile.velocity.X = -4 * Main.npc[(int)projectile.ai[1]].spriteDirection;
                projectile.velocity.Y = -3;
            }
            float num2 = 120f;
            float x = 0.85f;
            float y = 0.35f;
            int Damage = 0;
            float num3 = 0.0f;
            bool flag1 = true;
            bool flag2 = false;
            bool flag3 = false;
            if((double)projectile.ai[0] < (double)num2) {
                bool flag4 = true;
                int index1 = (int)projectile.ai[1];
                if(Main.npc[index1].active && Main.npc[index1].type == num1) {
                    if(!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        projectile.position = projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                } else {
                    projectile.ai[0] = num2;
                    flag4 = false;
                }
                if(flag4 && !flag2) {
                    projectile.velocity = projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                }
            }
			if (projectile.timeLeft <= 120)
            {
				if (NPC.CountNPCS(ModContent.NPCType<VileWasp>()) < 3 && Main.npc[(int)projectile.ai[1]].active)
                {
                    projectile.Kill();
                    if (Main.netMode != 1)
                    {
                        NPC.NewNPC((int)projectile.position.X, (int)projectile.position.Y, ModContent.NPCType<VileWasp>());
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
        }      
    }
}

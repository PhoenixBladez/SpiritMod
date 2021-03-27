using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;

namespace SpiritMod.Projectiles.Summon.MoonjellySummon
{
	public class LunazoaOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunazoa");
            Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 16;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.hostile = false;
            projectile.minion = true;
            projectile.penetrate = 1;
            projectile.hide = false;
			projectile.timeLeft = 300;
		}
        float alphaCounter;
        public override void AI()
        {
            alphaCounter += .04f;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            projectile.frameCounter++;
            projectile.spriteDirection = -projectile.direction;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                projectile.frameCounter = 0;
            }
            int num1 = ModContent.ProjectileType<MoonjellySummon>();
			float num2 = 60f;
			float x = 0.2f;
			float y = 0.2f;
			bool flag2 = false;
			if ((double)projectile.ai[0] < (double)num2) {
				bool flag4 = true;
				int index1 = (int)projectile.ai[1];
				if (Main.projectile[index1].active && Main.projectile[index1].type == num1) {
					if (!flag2 && Main.projectile[index1].oldPos[1] != Vector2.Zero)
						projectile.position = projectile.position + Main.projectile[index1].position - Main.projectile[index1].oldPos[1];
				}
				else {
					projectile.ai[0] = num2;
					flag4 = false;
                    projectile.Kill();
				}
				if (flag4 && !flag2) {
                    projectile.velocity += new Vector2((float)Math.Sign(Main.projectile[index1].Center.X - projectile.Center.X), (float)Math.Sign(Main.projectile[index1].Center.Y - projectile.Center.Y)) * new Vector2(x, y);
                    if (projectile.velocity.Length() > 4f)
                    {
                        projectile.velocity *= 4f / projectile.velocity.Length();
                    }
                }
			}

        }
        public override void Kill(int timeLeft)
        {
            for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
            {
                NPC npc = Main.npc[npcFinder];
                if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly)
                {
                    //if npc is within 50 blocks
                    float dist = projectile.Distance(npc.Center);
                    if (dist / 16 < 30)
                    {
                        if (!Main.npc[npcFinder].friendly && !Main.npc[npcFinder].townNPC && Main.npc[npcFinder].active)
                        {
                            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 110);
                            Vector2 direction = Main.npc[npcFinder].Center - projectile.Center;
                            direction.Normalize();
                            direction *= 12f;
                            Projectile p = Projectile.NewProjectileDirect(projectile.Center, direction,
                            ModContent.ProjectileType<JellyfishOrbiter_Friendly>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                            p.friendly = true;
                            p.hostile = false;
                            p.minion = true;
							p.netUpdate = true;
							p.scale = projectile.scale;
                            break;
                        }
                    }
                }
            }
		    for (int k = 0; k < 10; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(3.28f) * Main.rand.NextFloat(5), 0, default, Main.rand.NextFloat(.4f, .8f));
                d.noGravity = true;
            }
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                Color color1 = Color.White * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                spriteBatch.Draw(mod.GetTexture("Projectiles/Summon/MoonjellySummon/LunazoaOrbiter_Glow"), drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color1, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
                spriteBatch.Draw(mod.GetTexture("Projectiles/Summon/MoonjellySummon/LunazoaOrbiter_Glow"), drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color1, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);


                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);

            }
            return false;
        }
	}
}

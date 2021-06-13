
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using System.Linq;
using SpiritMod.Utilities;

namespace SpiritMod.Projectiles.Bullet
{
	public class ConfluxPellet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun Pellet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.height = 6;
			projectile.width = 6;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public bool bounce = false;
        int numBounce = 3;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 mouse = Main.MouseWorld;
			if (bounce)
            {
                projectile.velocity.Y += 0.1F;
                projectile.velocity.X *= 1.005F;
                projectile.ai[0] += .0205f;
                for (int i = 0; i < 4; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                    int num = Dust.NewDust(projectile.position, 6, 6, 272, 0f, 0f, 0, default(Color), .65f);
                    Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;

                    //Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                    Main.dust[num].velocity = projectile.velocity;
                    Main.dust[num].scale = MathHelper.Clamp(projectile.ai[0], .015f, 1.25f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = (float)(100 + projectile.owner);

                }
                if (Main.rand.NextBool(16))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                        float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                        int num = Dust.NewDust(projectile.position, 6, 6, 272, 0f, 0f, 0, default(Color), .65f);
                        Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;
    
                        //Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(18, Main.LocalPlayer);
                        Main.dust[num].velocity *= projectile.velocity/2;
                        Main.dust[num].scale = MathHelper.Clamp(projectile.ai[0], .015f, projectile.ai[0] / 3);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].fadeIn = (float)(100 + projectile.owner);
                    }
                }
            }
			else
            {
                for (int i = 0; i < 3; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                    int num = Dust.NewDust(new Vector2(x, y), 2, 2, 272);

                    Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;
                    Main.dust[num].alpha = projectile.alpha;
                    Main.dust[num].velocity = projectile.velocity;
                    Main.dust[num].noGravity = true;
                    Main.dust[num].fadeIn = 0.4684f;
                    Main.dust[num].scale *= .1235f;
                }

            }
        }
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
				Color color = new Color(240, 199, 255) * 0.65f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                float scale = projectile.scale;
                Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Bullet/ConfluxPellet");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                   
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

		public override void Kill(int timeLeft) => Main.PlaySound(SoundID.NPCHit, projectile.position, 3);

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (float numBounce = 0.0f; (double)numBounce < 10; ++numBounce)
            {
                int dustIndex = Dust.NewDust(projectile.Center, 2, 2, 272, 0f, 0f, 0, default, .56f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity = Vector2.Normalize(projectile.Center.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
            }
            bounce = true;
            projectile.ai[0] = .1f;
            numBounce--;
            if (numBounce <= 0)
                projectile.Kill();
            else
            {
				float maxdist = 500; //the maximum distance in pixels to check for an npc
				var targets = Main.npc.Where(x => x.CanBeChasedBy(this) && x != null && x.active && projectile.Distance(x.Center) < maxdist && Collision.CanHit(projectile.Center, 0, 0, x.Center, 0, 0)); //look through each npc, and find the ones that meet the given requirements
				if (targets.Any())
				{ //only run this code if there are any npcs that fulfill the above requirements
					NPC finaltarget = null;
					foreach (NPC npc in targets)
					{ //loop through each npc that meets the above requirements, and find the closest by decreasing the maximum distance each time a closer npc is found
						if (projectile.Distance(npc.Center) <= maxdist)
						{
							maxdist = projectile.Distance(npc.Center);
							finaltarget = npc;
						}
					}
					if (finaltarget != null) //only run this code if the final target isn't null, to prevent any possible null reference errors
						projectile.velocity = projectile.GetArcVel(finaltarget.Center, 0.1f, maxXvel: 8);
				}
				else 
					projectile.Bounce(oldVelocity, 0.75f);
            }
            return false;
        }
    }
}

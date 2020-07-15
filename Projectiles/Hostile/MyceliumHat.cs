
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Projectiles.Hostile
{
	public class MyceliumHat : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycelial Botanist");
			Main.projFrames[base.projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

		public override void SetDefaults()
		{
            projectile.width = 32;
			projectile.height = 16;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.melee = true;
            projectile.ignoreWater = true;
        }
		//  bool comingHome = false;
	
		public override bool PreAI()
		{

			if (projectile.soundDelay > 8)
            {
                projectile.soundDelay = 0;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }
			projectile.frameCounter++;
			if(projectile.frameCounter >= 6) {
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (!initialized)
			{
				initialized = true;
				Xint = projectile.velocity.X;
				Yint = projectile.velocity.Y;
			}
           // projectile.velocity.Y += .0625f;
			if(projectile.Hitbox.Intersects(npc.Hitbox) && projectile.timeLeft < 855) {
				projectile.active = false;
			}
			if(projectile.timeLeft < 854) {
				Vector2 direction9 = npc.Center - projectile.position;
				projectile.velocity = projectile.velocity.RotatedBy(direction9.ToRotation() - projectile.velocity.ToRotation());
				if (projectile.velocity.Length() < 0.5f)
				{
					direction9.Normalize();
					projectile.velocity = direction9 * 0.8f;
				}
				if (Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y)) < 18)
				{
					projectile.velocity *= 1.075f;
				}
				projectile.tileCollide = false;
			} else {
				projectile.velocity -= new Vector2(Xint, Yint) / 45f;
			}
			return false;
		}
        //projectile.ai[0]: X speed initial
        //projectile.ai[1]: y speed initial
		float Xint = 0;
		float Yint = 0;
		bool initialized = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, 199, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .85f;
                if (Main.dust[num].position != target.Center)
                    Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 5f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                var effects = projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) / 2);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			int amountOfProjectiles = Main.rand.Next(3, 5);
					bool expertMode = Main.expertMode;
					int damage = expertMode ? 8 : 13;
					for(int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-60, -40) * 0.1f;
						//int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, A, B, ModContent.ProjectileType<MyceliumSporeHostile>(), damage, 1);
						for(int k = 0; k < 11; k++) {
							Dust.NewDust(projectile.position, projectile.width, projectile.height, 42, A, B, 0, default(Color), .61f);
						}
						//Main.projectile[p].hostile = true;
					}
            return true;
        }
		public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, projectile.Center);
        }
	}
}

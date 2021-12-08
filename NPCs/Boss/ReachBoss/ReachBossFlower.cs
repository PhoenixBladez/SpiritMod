using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class ReachBossFlower : ModProjectile, ITrailProjectile
	{
		int target;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowflower");
		}

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(projectile, new GradientTrail(new Color(182, 66, 245) * .95f, new Color(91, 21, 150) * .7f), new RoundCap(), new DefaultTrailPosition(), 150f, 60f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = -1;
            projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.scale = 0.1f;
			projectile.width = 28;
			projectile.height = 28;
		}
        bool pulseTrail;
		public override void AI()
		{		
        	Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), 0.201f, 0.110f, 0.226f);
			projectile.ai[1]++;
			projectile.width = 28;
			projectile.height = 28;

			if (projectile.ai[1] < 99)
            {
                projectile.tileCollide = false;
            }
            else
            {
                projectile.tileCollide = true;
            }
			if (projectile.ai[1] < 60)
			{
				projectile.velocity *= .98f;
				projectile.scale = MathHelper.Lerp(projectile.scale, 1f, 0.015f);
			}
			else
			{
                pulseTrail = true;
                projectile.rotation = projectile.velocity.X * .06f;
				if (projectile.ai[1] > 60 && projectile.ai[1] < 99)
				{
					if (projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
						target = -1;
						float distance = 2000f;
						for (int k = 0; k < 255; k++) {
							if (Main.player[k].active && !Main.player[k].dead) {
								Vector2 center = Main.player[k].Center;
								float currentDistance = Vector2.Distance(center, projectile.Center);
								if (currentDistance < distance || target == -1) {
									distance = currentDistance;
									target = k;
								}
							}
						}
						if (target != -1) {
							projectile.ai[0] = 1;
							projectile.netUpdate = true;
						}
					}
					else if (target >= 0 && target < Main.maxPlayers) {
						Player targetPlayer = Main.player[target];
						if (!targetPlayer.active || targetPlayer.dead) {
							target = -1;
							projectile.ai[0] = 0;
							projectile.netUpdate = true;
						}
						else {
							float currentRot = projectile.velocity.ToRotation();
							Vector2 direction = targetPlayer.Center - projectile.Center;
							float targetAngle = direction.ToRotation();
							if (direction == Vector2.Zero)
								targetAngle = currentRot;

							float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
							projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot);
						}
					}
					projectile.velocity *= 1.058f;					
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Grass, (int)projectile.position.X, (int)projectile.position.Y);
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			for (int index = 0; index < 8; ++index)
			{
				int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.AmethystBolt, 0.0f, 0.0f, 0, Color.Purple, 1f);
				Main.dust[i].noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, (projectile.height / Main.projFrames[projectile.type]) * 0.5f);

        	float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
            if (pulseTrail)
			{
				SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 vector33 = new Vector2(projectile.Center.X, projectile.Center.Y) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity;
				Color color29 = new Color(127 - projectile.alpha, 127 - projectile.alpha, 127 - projectile.alpha, 0).MultiplyRGBA(Color.Orchid);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = projectile.GetAlpha(color28);
					color28 *= 1f - num107;
					Vector2 vector29 = new Vector2(projectile.Center.X, projectile.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity * (float)num103;
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/ReachBoss/ReachBossFlower"), vector29, new Microsoft.Xna.Framework.Rectangle?(Main.projectileTexture[projectile.type].Frame(1, Main.projFrames[projectile.type], 0, projectile.frame)), color28, projectile.rotation, drawOrigin, projectile.scale, spriteEffects3, 0f);
				}
			}
		}
	}
}

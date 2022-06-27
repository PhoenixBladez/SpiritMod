using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
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

		public void DoTrailCreation(TrailManager tManager) => tManager.CreateTrail(Projectile, new GradientTrail(new Color(182, 66, 245) * .95f, new Color(91, 21, 150) * .7f), new RoundCap(), new DefaultTrailPosition(), 150f, 60f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_1").Value, 0.01f, 1f, 1f));

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.scale = 0.1f;
			Projectile.width = 28;
			Projectile.height = 28;
		}
        bool pulseTrail;
		public override void AI()
		{		
        	Lighting.AddLight((int)((Projectile.position.X + (float)(Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (float)(Projectile.height / 2)) / 16f), 0.201f, 0.110f, 0.226f);
			Projectile.ai[1]++;
			Projectile.width = 28;
			Projectile.height = 28;

			if (Projectile.ai[1] < 99)
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
			if (Projectile.ai[1] < 60)
			{
				Projectile.velocity *= .98f;
				Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.015f);
			}
			else
			{
                pulseTrail = true;
                Projectile.rotation = Projectile.velocity.X * .06f;
				if (Projectile.ai[1] > 60 && Projectile.ai[1] < 99)
				{
					if (Projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
						target = -1;
						float distance = 2000f;
						for (int k = 0; k < 255; k++) {
							if (Main.player[k].active && !Main.player[k].dead) {
								Vector2 center = Main.player[k].Center;
								float currentDistance = Vector2.Distance(center, Projectile.Center);
								if (currentDistance < distance || target == -1) {
									distance = currentDistance;
									target = k;
								}
							}
						}
						if (target != -1) {
							Projectile.ai[0] = 1;
							Projectile.netUpdate = true;
						}
					}
					else if (target >= 0 && target < Main.maxPlayers) {
						Player targetPlayer = Main.player[target];
						if (!targetPlayer.active || targetPlayer.dead) {
							target = -1;
							Projectile.ai[0] = 0;
							Projectile.netUpdate = true;
						}
						else {
							float currentRot = Projectile.velocity.ToRotation();
							Vector2 direction = targetPlayer.Center - Projectile.Center;
							float targetAngle = direction.ToRotation();
							if (direction == Vector2.Zero)
								targetAngle = currentRot;

							float desiredRot = currentRot.AngleLerp(targetAngle, 0.1f);
							Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0f).RotatedBy(desiredRot);
						}
					}
					Projectile.velocity *= 1.058f;					
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Grass, (int)Projectile.position.X, (int)Projectile.position.Y);
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			for (int index = 0; index < 8; ++index)
			{
				int i = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AmethystBolt, 0.0f, 0.0f, 0, Color.Purple, 1f);
				Main.dust[i].noGravity = true;
			}
		}
		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);
		public override void PostDraw(Color lightColor)
		{
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, (Projectile.height / Main.projFrames[Projectile.type]) * 0.5f);

        	float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;
            if (pulseTrail)
			{
				SpriteEffects spriteEffects3 = (Projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 vector33 = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) - Projectile.velocity;
				Color color29 = new Color(127 - Projectile.alpha, 127 - Projectile.alpha, 127 - Projectile.alpha, 0).MultiplyRGBA(Color.Orchid);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = Projectile.GetAlpha(color28);
					color28 *= 1f - num107;
					Vector2 vector29 = new Vector2(Projectile.Center.X, Projectile.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + Projectile.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) - Projectile.velocity * (float)num103;
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/ReachBoss/ReachBossFlower").Value, vector29, new Microsoft.Xna.Framework.Rectangle?(TextureAssets.Projectile[Projectile.type].Value.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame)), color28, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects3, 0f);
				}
			}
		}
	}
}

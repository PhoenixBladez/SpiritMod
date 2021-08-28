
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class BloodRuneEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodflames");
			Main.projFrames[base.projectile.type] = 7;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;       //projectile width
			projectile.height = 16;  //projectile height
			projectile.friendly = false;      //make that the projectile will not damage you
			projectile.hostile = false;        // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 200;   //how many time projectile projectile has before disepire
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
			projectile.aiStyle = -1;
		}
		int timer;
		int target;
		int aiTimer2;
		public override void AI()
		{
			if (projectile.frameCounter >= 8) {
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			Lighting.AddLight(projectile.position, 0.4f / 2, .12f / 2, .036f / 2);
			timer += 4;
			projectile.alpha += 1;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 10) {
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 10;
			}
			aiTimer2++;
			if (aiTimer2 >= 20) {
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
				projectile.velocity.X *= .99f;
				projectile.velocity.Y *= .99f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, 0f, -2f, 0, default, .6f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 1.5f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

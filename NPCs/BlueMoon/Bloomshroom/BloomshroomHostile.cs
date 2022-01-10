using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.BlueMoon.Bloomshroom
{
	public class BloomshroomHostile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomshroom");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
			projectile.height = 20;
			projectile.width = 20;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			switch (Main.rand.Next(2)) {
				case 0:
					tM.CreateTrail(projectile, new StandardColorTrail(new Color(120, 217, 255)), new RoundCap(), new SleepingStarTrailPosition(), 18f, 250f);
					break;
				case 1:
					tM.CreateTrail(projectile, new StandardColorTrail(new Color(218, 94, 255)), new RoundCap(), new SleepingStarTrailPosition(), 18f, 250f);
					break;
			}
		}

		int target;
		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list) {
				if (projectile != proj && proj.friendly && proj.damage > 0 && proj.active) {
					projectile.Kill();
				}
			}
			return true;
		}
		public override void AI()
		{
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

						float desiredRot = currentRot.AngleLerp(targetAngle, 0.13f);
						projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(desiredRot);
						projectile.velocity *= .996f;
					}
				};
			}
			projectile.rotation = projectile.velocity.X * .1f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			for (int i = 0; i < 5; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .6f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5.5f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 100);
	}
}

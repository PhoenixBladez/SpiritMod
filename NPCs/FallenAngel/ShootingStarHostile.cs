using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.FallenAngel
{
	public class ShootingStarHostile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 30;
			Projectile.width = 10;
		}

		int target;

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 215, 105), new Color(105, 213, 255)), new RoundCap(), new SleepingStarTrailPosition(), 14f, 150f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 215, 105) * .5f, new Color(105, 213, 255) * .5f), new RoundCap(), new SleepingStarTrailPosition(), 56f, 250f, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new SleepingStarTrailPosition(), 56f, 30f, new DefaultShader());
		}

		public override void AI()
		{
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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
				};
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, -2f, 0, default, .6f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 5.5f;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 100);
		}
	}
}

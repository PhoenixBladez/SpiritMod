using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
			Projectile.height = 20;
			Projectile.width = 20;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			switch (Main.rand.Next(2)) {
				case 0:
					tM.CreateTrail(Projectile, new StandardColorTrail(new Color(120, 217, 255)), new RoundCap(), new SleepingStarTrailPosition(), 18f, 250f);
					break;
				case 1:
					tM.CreateTrail(Projectile, new StandardColorTrail(new Color(218, 94, 255)), new RoundCap(), new SleepingStarTrailPosition(), 18f, 250f);
					break;
			}
		}

		int target;
		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.friendly && proj.damage > 0 && proj.active) {
					Projectile.Kill();
				}
			}
			return true;
		}
		public override void AI()
		{
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

						float desiredRot = currentRot.AngleLerp(targetAngle, 0.13f);
						Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0f).RotatedBy(desiredRot);
						Projectile.velocity *= .996f;
					}
				};
			}
			Projectile.rotation = Projectile.velocity.X * .1f;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
			for (int i = 0; i < 5; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, .6f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 5.5f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 100);
	}
}

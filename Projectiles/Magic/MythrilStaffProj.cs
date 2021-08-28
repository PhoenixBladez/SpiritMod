using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class MythrilStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Pellet");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0) {
				target.StrikeNPC(projectile.damage / 2, 0f, 0, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Dig, projectile.Center);
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Mythril, 0f, 0f, 100, default, .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}

		public override bool PreAI()
		{
			bool chasing = false;
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 30f) {
				chasing = true;

				projectile.friendly = true;
				NPC target;
				if (projectile.ai[0] == -1f) {
					target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
				}
				else {
					target = Main.npc[(int)projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy())
						target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
				}

				if (target == null) {
					chasing = false;
					projectile.ai[0] = -1f;
				}
				else {
					projectile.ai[0] = target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing) {
				Vector2 dir = projectile.velocity;
				float vel = projectile.velocity.Length();
				if (vel != 0f) {
					if (vel < 4f) {
						dir *= 1 / vel;
						projectile.velocity += dir * 0.0625f;
					}
				}
				else {
					//Stops the projectiles from spazzing out
					projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}

			//Create particles
			return true;
		}
		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.3f;
			projectile.scale = num395 + 0.95f;
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.Tungsten, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / 5 * k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

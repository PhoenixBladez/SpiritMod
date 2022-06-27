using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class MythrilStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Pellet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0) {
				target.StrikeNPC(Projectile.damage / 2, 0f, 0, false);
			}
		}
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Mythril, 0f, 0f, 100, default, .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}

		public override bool PreAI()
		{
			bool chasing = false;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 30f) {
				chasing = true;

				Projectile.friendly = true;
				NPC target;
				if (Projectile.ai[0] == -1f) {
					target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
				}
				else {
					target = Main.npc[(int)Projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy())
						target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
				}

				if (target == null) {
					chasing = false;
					Projectile.ai[0] = -1f;
				}
				else {
					Projectile.ai[0] = target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing) {
				Vector2 dir = Projectile.velocity;
				float vel = Projectile.velocity.Length();
				if (vel != 0f) {
					if (vel < 4f) {
						dir *= 1 / vel;
						Projectile.velocity += dir * 0.0625f;
					}
				}
				else {
					//Stops the projectiles from spazzing out
					Projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}

			//Create particles
			return true;
		}
		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.3f;
			Projectile.scale = num395 + 0.95f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Tungsten, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / 5 * k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}

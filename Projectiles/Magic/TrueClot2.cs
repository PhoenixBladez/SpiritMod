using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TrueClot2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Blood Cluster");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = true;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);
			int dus3 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f);

			if (Projectile.localAI[0] == 0f) {
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance = 400f;
			bool target = false;
			for (int k = 0; k < 200; k++) {
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5) {
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance) {
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}

			if (target) {
				AdjustMagnitude(ref move);
				Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
				AdjustMagnitude(ref Projectile.velocity);
			}

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			Dust.NewDust(Projectile.position + Projectile.velocity * 0, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.oldVelocity.X * 0, Projectile.oldVelocity.Y * 0);
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
				vector *= 6f / magnitude;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) <= 4)
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileID.VampireHeal, 0, 0f, Projectile.owner, Projectile.owner, Main.rand.Next(4, 6));
		}

	}
}
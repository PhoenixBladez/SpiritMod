using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.Zipline
{
	public class RightZipline : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Right Zipline");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.alpha = 0;
			Projectile.tileCollide = true;
		}

		bool chain = false;
		int leftValue;
		int distance = 9999;
		bool stuck = false;
		float alphaCounter;

		public override bool PreAI()
		{
			alphaCounter += 0.04f;
			if (!stuck) {
				Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			}
			Projectile.timeLeft = 50;
			leftValue = (int)Projectile.ai[1];
			if (leftValue < (double)Main.projectile.Length && leftValue != 0) {
				Projectile other = Main.projectile[leftValue];
				if (other.active) {
					direction9 = other.Center - Projectile.Center;
					distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					chain = true;
				}
				else {
					chain = false;
				}
			}
			else {
				chain = false;
			}
			if (stuck) {
				Projectile.velocity = Vector2.Zero;
			}
			return true;
		}
		public override void AI()
		{
			if (stuck)
				DoDustEffect(Projectile.Center, 18f);
		}
		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Electric);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .26f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
		public override void PostDraw(Color lightColor)
		{
			if (chain && distance < 2000 && stuck) {
				Projectile other = Main.projectile[leftValue];
				direction9 = other.Center - Projectile.Center;
				direction9.Normalize();
				//	direction9 *= 6;
				ProjectileExtras.DrawChain(Projectile.whoAmI, other.Center,
				"SpiritMod/Projectiles/Summon/Zipline/Zipline_Chain", false, 0, true, direction9.X, direction9.Y);
			}

		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!stuck) {
				SoundEngine.PlaySound(SoundID.Item52, Projectile.Center);
			}
			if (oldVelocity.X != Projectile.velocity.X) //if its an X axis collision
				{
				if (Projectile.velocity.X > 0) {
					Projectile.rotation = 1.57f;
				}
				else {
					Projectile.rotation = 4.71f;
				}
			}
			if (oldVelocity.Y != Projectile.velocity.Y) //if its a Y axis collision
			{
				if (Projectile.velocity.Y > 0) {
					Projectile.rotation = 3.14f;
				}
				else {
					Projectile.rotation = 0f;
				}
			}
			stuck = true;
			return false;
		}
	}
}

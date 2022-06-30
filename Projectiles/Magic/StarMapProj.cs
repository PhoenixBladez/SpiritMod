using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class StarMapProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Map");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			Projectile.timeLeft = 999999;
		}

		int counter = 0;
		Vector2 holdOffset = new Vector2(0, -15);
		public override bool PreAI()
		{
			DoDustEffect(Projectile.Center, 14f);
			Player player = Main.player[Projectile.owner];
			Vector2 direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
			direction.Normalize();
			direction *= 12f;
			if (player.channel) {
				if (direction.X > 0) {
					holdOffset.X = 5;
					player.direction = 1;
				}
				else {
					holdOffset.X = -30;
					player.direction = 0;
				}

				Projectile.position = player.Center + holdOffset;
				player.velocity.X *= 0.97f;
				counter++;
				if (counter > 80) {
					DoDustEffect(Projectile.Center, 54f);
					if (counter % 3 == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<StarMapTrail>(), 0, 0, Projectile.owner);
					}
				}
			}
			else {
				if (counter > 80) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<TeleportBolt>(), 0, 0, Projectile.owner);
				}
				Projectile.active = false;
			}
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			return true;
		}
		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Electric);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .3f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
	}
}

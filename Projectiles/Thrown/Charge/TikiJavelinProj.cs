using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class TikiJavelinProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.timeLeft = 999999;
			projectile.tileCollide = false;
		}

		float counter = 3;
		int trailcounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			if(player.channel) {
				projectile.position = player.position + holdOffset;
				player.velocity.X *= 0.95f;
				if(counter < 14) {
					counter += 0.1f;
				}
				Vector2 direction = Main.MouseWorld - (projectile.position);
				direction.Normalize();
				direction *= counter;
				projectile.rotation = direction.ToRotation() - 1.57f;
				if(direction.X > 0) {
					holdOffset.X = -10;
					player.direction = 1;
				} else {
					holdOffset.X = 10;
					player.direction = 0;
				}
				trailcounter++;
				if(trailcounter % 5 == 0)
					Projectile.NewProjectile(projectile.Center + (direction * 3), direction, ModContent.ProjectileType<TikiJavelinProj1>(), 0, 0, projectile.owner); //predictor trail, please pick a better dust Yuy
			} else {
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 1);
				Vector2 direction = Main.MouseWorld - (projectile.position);
				direction.Normalize();
				direction *= counter;
				Projectile.NewProjectile(projectile.Center + (direction * 3), direction, ModContent.ProjectileType<TikiJavelinProj2>(), (int)(projectile.damage * Math.Sqrt(counter)), projectile.knockBack, projectile.owner);
				projectile.active = false;
			}
			player.heldProj = projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			//	player.itemRotation = 0;
			return true;
		}
	}
}

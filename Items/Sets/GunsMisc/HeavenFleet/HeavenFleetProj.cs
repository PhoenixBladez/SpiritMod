using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Bullet;
using Terraria.ID;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenFleetProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heaven Fleet");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.timeLeft = 999999;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
		}

		int counter = 1;
		int maxCounter = 1;
		bool firing = false;
		Vector2 direction = Vector2.Zero;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

			player.itemTime = 10; // Set item time to 10 frames while we are used
			player.itemAnimation = 10; // Set item animation time to 10 frames while we are used
			projectile.position = player.Center;
			direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
			direction.Normalize();
			direction *= 10f;
			if (player.channel && !firing) {
				if (counter < 100) {
					maxCounter++;
					counter++;
					if (counter % 10 == 9) {
						Main.PlaySound(SoundID.Item5, (int)projectile.position.X, (int)projectile.position.Y);
					}
					Vector2 dustUnit = (direction * 2.5f).RotatedBy(Main.rand.NextFloat(-1,1)) * 0.03f;
					Vector2 dustOffset = player.Center + (direction * 7f) + player.velocity;
					Dust dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), 242);
					dust.velocity = Vector2.Zero - (dustUnit * 4);
					dust.noGravity = true;
					dust.scale = (float)Math.Sqrt(counter / 100f);
				}
				direction = direction.RotatedBy(Main.rand.NextFloat(0 - ((float)Math.Sqrt(counter) / 80f), ((float)Math.Sqrt(counter) / 80f)));
				player.itemRotation = direction.ToRotation();
				if (player.direction != 1)
				{
					player.itemRotation -= 3.14f;
				}
			}
			else {
				firing = true;
				if (counter > 0)
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 11);
				while (counter >= 0) 
				{
					counter -= 10;
					Vector2 toShoot = direction.RotatedBy(Main.rand.NextFloat(0 - ((float)Math.Sqrt(200 - maxCounter) / 40f), ((float)Math.Sqrt(200 - maxCounter) / 40f)));
					player.itemRotation = (toShoot.ToRotation() + direction.ToRotation()) / 2;
					if (player.direction != 1)
						player.itemRotation -= 3.14f;
					projectile.velocity = toShoot;
					toShoot /= 2f;
					toShoot *= (float)Math.Pow(maxCounter, 0.18);
					player.GetModPlayer<MyPlayer>().Shake += 1;
					Projectile proj = Projectile.NewProjectileDirect(player.Center + (direction * 4), toShoot, (int)projectile.ai[0], projectile.damage, projectile.knockBack, projectile.owner);
					if (proj.modProjectile is ConfluxPellet modProj)
						modProj.bounce = true;
				}
				projectile.active = false;
			}
			return true;
		}
	}
}

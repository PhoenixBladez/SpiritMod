using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class InterstellarShield : ModProjectile
	{
		public bool shooting = false;
		double dist = 80;
		Vector2 direction = Vector2.Zero;
		int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("InterstellarShield");
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 600;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.damage = 1;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.width = Projectile.height = 24;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
			for (int k = 0; k < 15; k++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			//Factors for calculations
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (proj.hostile && proj.active && proj.timeLeft > 2) {
					if (proj.damage < 65) {
						counter++;
						proj.timeLeft = 2;
						SoundEngine.PlaySound(SoundID.Item93, Projectile.position);
						proj.active = false;
					}
					else {
						counter += 5;
					}
				}
			}
			if (counter >= 2) {
				Projectile.active = false;
				((MyPlayer)player.GetModPlayer(Mod, "MyPlayer")).shieldsLeft -= 1;
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 27);
			}
			Vector2 center = Projectile.Center;
			float num8 = (float)player.miscCounter / 60f;
			float num7 = 1.0471975512f * 2;
			for (int i = 0; i < 3; i++) {
				int num6 = Dust.NewDust(center, 0, 0, DustID.DungeonSpirit, 0f, 0f, 100, default, 1.3f);
				Main.dust[num6].noGravity = true;
				Main.dust[num6].velocity = Vector2.Zero;
				Main.dust[num6].noLight = true;
				Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
			}

			double deg = (double)Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
			double rad = deg * (Math.PI / 180); //Convert degrees to radians

			/*Position the projectile based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */

			//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
			Projectile.ai[1] += .38f;
			if (((MyPlayer)player.GetModPlayer(Mod, "MyPlayer")).ShieldCore) {
				Projectile.timeLeft = 2;
			}
			Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
			Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
			direction = player.Center - Projectile.Center;
			direction.Normalize();
		}
	}
}
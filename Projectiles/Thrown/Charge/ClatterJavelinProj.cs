using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class ClatterJavelinProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Javelin");
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
			Projectile.alpha = 0;
			Projectile.timeLeft = 999999;
			Projectile.tileCollide = false;
		}

		float counter = 3;
		int trailcounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
				if (Projectile.owner == Main.myPlayer)
				{
					Vector2 direction2 = Main.MouseWorld - (Projectile.position);
					direction2.Normalize();
					direction2 *= counter;
					Projectile.ai[0] = direction2.X;
					Projectile.ai[1] = direction2.Y;
					Projectile.netUpdate = true;
				}
			Vector2 direction = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			if (player.channel) {
				Projectile.position = player.position + holdOffset;
				player.velocity.X *= 0.95f;
				if (counter < 9) {
					counter += 0.05f;
				}
				Projectile.rotation = direction.ToRotation() - 1.57f;
				if (direction.X > 0) {
					holdOffset.X = -10;
					player.direction = 1;
				}
				else {
					holdOffset.X = 10;
					player.direction = 0;
				}
				trailcounter++;
				if (trailcounter % 5 == 0 && Projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + (direction * 6), direction, ModContent.ProjectileType<ClatterJavelinProj1>(), 0, 0, Projectile.owner); //predictor trail, please pick a better dust Yuy
			}
			else {
				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + (direction * 6), direction, ModContent.ProjectileType<ClatterJavelinProj2>(), (int)(Projectile.damage * Math.Sqrt(counter)), Projectile.knockBack, Projectile.owner);
				}
				Projectile.active = false;
			}
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			//	player.itemRotation = 0;
			return true;
		}
	}
}

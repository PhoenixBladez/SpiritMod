using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class ClatterJavelinProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Clatter Javelin");

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
			projectile.timeLeft = 2;
			projectile.tileCollide = false;
		}

		float counter = 3;
		int trailcounter = 0;
		Vector2 holdOffset = new Vector2(0, -3);

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			Vector2 direction = Vector2.Normalize(Main.MouseWorld - projectile.Center) * counter;

			projectile.timeLeft = 2;

			if(player.channel) {

				projectile.position = player.position + holdOffset;
				projectile.rotation = direction.ToRotation() - 1.57f;
				player.velocity.X *= 0.95f;

				if(counter < 9) {
					counter += 0.05f;
				}
			
				if(direction.X > 0) {
					holdOffset.X = -10;
					player.direction = 1;
				}
				else {
					holdOffset.X = 10;
					player.direction = 0;
				}

				trailcounter++;
				if(trailcounter % 5 == 0 && projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(projectile.Center + (direction * 6), direction, ProjectileType<ClatterJavelinProj1>(), 0, 0, projectile.owner); //predictor trail
			}
			else if (projectile.owner == Main.myPlayer){
				int damage = (int)(projectile.damage * Math.Sqrt(counter));

				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 1);
				int i = Projectile.NewProjectile(projectile.Center + (direction * 6), direction, ProjectileType<ClatterJavelinProj2>(), damage, projectile.knockBack, projectile.owner);
				Main.projectile[i].netUpdate = true;

				projectile.active = false;
			}

			player.heldProj = projectile.whoAmI;
			player.itemTime = 30;
			player.itemAnimation = 30;
			return true;
		}
	}
}

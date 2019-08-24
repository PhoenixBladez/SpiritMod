using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Zephyr : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyr");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.magic = true;
			projectile.width = 14;
			projectile.height = 26;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
			projectile.scale = 1.4f;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = true;
			//Just used the dust code i used for whirlpool lol <- scratch that 
			//i used it again, but from sandstorm this time
			//I'm lazy okay!
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, 0f, 0f); //to make some with gravity to fly all over the place :P

			projectile.velocity.Y += projectile.ai[0];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 2;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, 16, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 9);
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
		}

	}
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PalladiumStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Shot");
		}
		int bounce = 3;
		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			projectile.timeLeft = 1000;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 4; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 55, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
			}

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bounce--;
			if (bounce <= 0)
				projectile.Kill();

			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y;

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (crit && Main.rand.Next(4) == 0)
			{
				player.AddBuff(BuffID.RapidHealing, 120, true);
			}
		}


	}
}

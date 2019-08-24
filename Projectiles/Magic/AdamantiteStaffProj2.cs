using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AdamantiteStaffProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Blast");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			projectile.timeLeft = 80;
		}

		public override bool PreAI()
		{
			projectile.velocity = projectile.velocity.RotatedBy(System.Math.PI / 40);
			{
				for (int i = 0; i < 10; i++)
				{
					float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					int num = Dust.NewDust(new Vector2(x, y), 26, 26, 60);
					Main.dust[num].position.X = x;
					Main.dust[num].position.Y = y;
					Main.dust[num].velocity *= 0f;
					Main.dust[num].noGravity = true;
				}
			}
			return true;
		}

	}
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class ElectricChain : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ElectricChain");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 16;
			projectile.melee = true;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 2;
			projectile.tileCollide = false;
			projectile.extraUpdates = 7;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(9) == 1)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 226, 0f, 0f, 0, default(Color));
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
			}
			return true;
		}
		
	}
}
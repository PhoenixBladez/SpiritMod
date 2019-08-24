using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class LunarStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Star");
			Main.projFrames[base.projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.alpha = 255;
			projectile.width = 16;       //projectile width
			projectile.height = 16;  //projectile height
			projectile.friendly = false;      //make that the projectile will not damage you
			projectile.hostile = true;        // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 1)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 40; i++)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}
		}

	}
}

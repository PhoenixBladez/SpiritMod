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
	public class ToadStool : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Toadstool");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;       //projectile width
			projectile.height = 16;  //projectile height
			projectile.friendly = false;      //make that the projectile will not damage you
			projectile.hostile = true;        // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 75;   //how many time projectile projectile has before disepire
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Vector3 RGB = new Vector3(0f, 0.5f, 1.5f);
			float multiplier = 1;
			float max = 2.25f;
			float min = 1.0f;
			RGB *= multiplier;
			if (RGB.X > max)
				multiplier = 0.5f;
			else if (RGB.X < min)
				multiplier = 1.5f;

			Lighting.AddLight(projectile.position, RGB.X, RGB.Y, RGB.Z);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
			}
		}

	}
}

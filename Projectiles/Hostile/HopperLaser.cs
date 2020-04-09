using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class HopperLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deepspace Hopper");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 16;
			projectile.tileCollide = true;
			projectile.extraUpdates = 7;
		}


		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w+= 4)
			{
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 226, Vector2.Zero).noGravity = true;
			}
		}
		public override bool PreAI()
		{
			Trail(projectile.position, projectile.position + projectile.velocity);
			return true;
		}
		
	}
}
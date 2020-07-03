using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiritMod.Projectiles.Hostile
{
	public class RyTentacle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tentacle Spike");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 16;
			projectile.height = 30;
			projectile.hide = true;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 999;
			projectile.tileCollide = true;
			projectile.alpha = 255;
		//	projectile.extraUpdates = 1;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		bool activated = false;
		public override bool PreAI()
		{
			projectile.velocity.X = 0;
			if(!activated) {
				projectile.velocity.Y = 24;
			}
			 else
			{
				projectile.ai[0]++;
				//projectile.extraUpdates = 0;
				if (projectile.ai[0] < 50)
				{
					projectile.timeLeft = 60;
					for (float i = projectile.position.Y + 30; i > projectile.position.Y - 125; i -= 10)
					{
						Dust.NewDustPerfect(new Vector2(projectile.Center.X, i), 173).noGravity = true;
					}
					projectile.velocity = Vector2.Zero;
				}
				else if (projectile.ai[0] < 60)
				{
					projectile.hostile = true;
					projectile.velocity.Y = -10;
				}
				else
				{
					projectile.velocity.Y = 3;
					projectile.alpha += 10;
				}
				if (projectile.ai[0] == 50)
				{
					projectile.alpha = 0;
					//put the sound effect here
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(oldVelocity.Y != projectile.velocity.Y && !activated) {
				activated = true;
				projectile.tileCollide = false;
			}
			return false;
		}
	}
}


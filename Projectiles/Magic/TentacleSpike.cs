using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiritMod.Projectiles.Magic
{
	public class TentacleSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tentacle Spike");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 24;
			projectile.height = 24;
			projectile.hide = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 100;
			projectile.timeLeft = 40;
			projectile.tileCollide = false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public override void AI()
		{
			if (projectile.ai[0] > 7)
			{
				projectile.velocity = Vector2.Zero;
			}
			else
			{
				projectile.ai[0]++;
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			}
		}
	}
}


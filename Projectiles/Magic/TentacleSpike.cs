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
			projectile.width = 14;
			projectile.height = 26;
			projectile.hide = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 40;
			projectile.tileCollide = false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public override void AI()
		{
			if (projectile.timeLeft >= 35) {
				projectile.alpha = 255;
			}
			else {
				projectile.alpha = 0 + (40 - projectile.timeLeft) * 6;
			}
			if (projectile.ai[0] > 7) {
				projectile.velocity = Vector2.Zero;
			}
			else {
				projectile.ai[0]++;
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			}
			if (projectile.velocity != Vector2.Zero) {
				for (int i = 0; i < 4; i++) {
					int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height * 2, DustID.Butterfly, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.75f);
					Main.dust[dust].noGravity = true;
					int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height * 2, DustID.ShadowbeamStaff, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
					Main.dust[dust1].scale = Main.rand.NextFloat(.9f, 1.75f);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].noLight = true;
				}
			}
		}
	}
}


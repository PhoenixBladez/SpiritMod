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
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 14;
			Projectile.height = 26;
			Projectile.hide = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}
		public override void AI()
		{
			if (Projectile.timeLeft >= 35) {
				Projectile.alpha = 255;
			}
			else {
				Projectile.alpha = 0 + (40 - Projectile.timeLeft) * 6;
			}
			if (Projectile.ai[0] > 7) {
				Projectile.velocity = Vector2.Zero;
			}
			else {
				Projectile.ai[0]++;
				Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			}
			if (Projectile.velocity != Vector2.Zero) {
				for (int i = 0; i < 4; i++) {
					int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height * 2, DustID.Butterfly, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
					Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.75f);
					Main.dust[dust].noGravity = true;
					int dust1 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height * 2, DustID.ShadowbeamStaff, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
					Main.dust[dust1].scale = Main.rand.NextFloat(.9f, 1.75f);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].noLight = true;
				}
			}
		}
	}
}


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class BossSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Spike");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 32;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowHostile;
			projectile.tileCollide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height,
				2, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 180);
		}

	}
}
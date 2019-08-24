using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ThornBowThorn : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.height = 10;
			projectile.width = 14;
			projectile.timeLeft = 600;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.penetrate = 4;
			projectile.tileCollide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 2, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(BuffID.Poisoned, 380);
		}
	}
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class JadeScarab : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornate Scarab");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BabySlime);
			projectile.width = 32;
			projectile.height = 32;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.netImportant = true;
			aiType = ProjectileID.BabySlime;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 180;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			projectile.minionSlots = 0.5f;
			Main.projFrames[projectile.type] = 4;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate == 0)
				projectile.Kill();

			return false;
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 1)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			return true;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
		}

	}
}